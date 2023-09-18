using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Characters
{
    public class CharacterSpriteLayer
    {
        private CharacterManager characterManager => CharacterManager.Instance;

        // Const
        private const float DefaultTransitionSpeed = 3f;

        // Vars
        public int LayerNum { get; private set; } = 0;
        public Image Renderer { get; private set; } = null;
        public CanvasGroup rendererCG => Renderer.GetComponent<CanvasGroup>();
        private List<CanvasGroup> _oldRenderers = new List<CanvasGroup>();
        private float _transitionSpeedMultiplier = 1f;
        private bool isFacingLeft = Character.DefaultOrientationIsFacingLeft;

        // Coroutines
        private Coroutine co_transitioningSprite = null;
        public bool isTransitioningSprite => co_transitioningSprite != null;
        private Coroutine co_levelingAlpha = null;
        public bool isLevelingAlpha => co_levelingAlpha != null;
        private Coroutine co_changingColor = null;
        public bool isChangingColor => co_changingColor != null;
        private Coroutine co_flipping = null;
        public bool isFlipping => co_flipping != null;

        // Constructor
        public CharacterSpriteLayer(Image defaultRenderer, int layerNum = 0)
        {
            LayerNum = layerNum;
            Renderer = defaultRenderer;
        }

        /// ///////////////////
        /// // Sprites ///////
        /// //////////////////

        public void SetSprite(Sprite sprite)
        {
            Renderer.sprite = sprite;
        }

        // Tranistion sprite - manage coroutine
        public Coroutine TransitionSprite(Sprite sprite, float speed)
        {
            // If we're already showing the same sprite, do nothing
            if (Renderer.sprite == sprite)
            {
                return null;
            }

            // If running, stop it first
            if (isTransitioningSprite)
            {
                characterManager.StopCoroutine(co_transitioningSprite);
            }

            co_transitioningSprite = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));
            return co_transitioningSprite;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            _transitionSpeedMultiplier = speedMultiplier;
            Image newRenderer = CreateRenderer(Renderer.transform.parent);

            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlpha();
            co_transitioningSprite = null;
        }

        // Create new renderer to fade in and out smoothly, instead of just changing sprite
        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(Renderer, parent);
            // Put the old one to the list
            _oldRenderers.Add(rendererCG);
            // Make new as a copy almost
            newRenderer.name = Renderer.name;
            Renderer = newRenderer;
            Renderer.gameObject.SetActive(true);
            rendererCG.alpha = 0;

            return newRenderer;
        }

        // Try to level alpha if none is already going
        private Coroutine TryStartLevelingAlpha()
        {
            if (isLevelingAlpha)
            {
                return co_levelingAlpha;
            }
            co_levelingAlpha = characterManager.StartCoroutine(RunLevelingAlpha());
            return co_levelingAlpha;
        }

        private IEnumerator RunLevelingAlpha()
        {
            float speed = DefaultTransitionSpeed * _transitionSpeedMultiplier * Time.deltaTime;
            // If sprite is not yet visible OR old sprites are still fading out
            while (rendererCG.alpha < 1 || _oldRenderers.Any(oldCG => oldCG.alpha > 0))
            {
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

                for (int i = _oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = _oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

                    if (oldCG.alpha <= 0)
                    {
                        _oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }
                yield return null;
            }
            co_levelingAlpha = null;
        }

        /// ///////////////////
        /// /// Colors  ///////
        /// //////////////////

        public void SetColor(Color color)
        {

            Renderer.color = color;

            foreach (CanvasGroup oldCG in _oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public void StopChangingColor()
        {
            if (!isChangingColor) return;

            characterManager.StopCoroutine(co_changingColor);
            co_changingColor = null;
        }

        public Coroutine TransitionColor(Color color, float speed)
        {
            if (isChangingColor)
            {
                characterManager.StopCoroutine(co_changingColor);
            }

            co_changingColor = characterManager.StartCoroutine(ChangingColor(color, speed));
            return co_changingColor;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            // We cache current color
            Color oldColor = Renderer.color;
            // We cache old images, even if fading out it will look weird if not changed
            List<Image> oldImages = new List<Image>();
            foreach (CanvasGroup oldCG in _oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }
            // Keep track of percentage changed
            float colorPercentage = 0;

            while (colorPercentage < 1)
            {
                colorPercentage += DefaultTransitionSpeed * speedMultiplier * Time.deltaTime;

                Renderer.color = Color.Lerp(oldColor, color, colorPercentage);
                foreach (Image image in oldImages)
                {
                    image.color = Renderer.color;
                }
                yield return null;
            }
            co_changingColor = null;
        }

        /// ///////////////////
        /// ////  Flip  ///////
        /// //////////////////

        public Coroutine Flip(float speed = 1, bool isImmediate = false)
        {
            if (isFacingLeft)
            {
                return FaceRight(speed, isImmediate);
            }
            else
            {
                return FaceLeft(speed, isImmediate);
            }
        }

        public Coroutine FaceLeft(float speed = 1, bool isImmediate = false)
        {
            if (isFlipping)
            {
                characterManager.StopCoroutine(co_flipping);
            }
            isFacingLeft = true;
            co_flipping = characterManager.StartCoroutine(FacingDirection(isFacingLeft, speed, isImmediate));
            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1, bool isImmediate = false)
        {
            if (isFlipping)
            {
                characterManager.StopCoroutine(co_flipping);
            }
            isFacingLeft = false;
            co_flipping = characterManager.StartCoroutine(FacingDirection(isFacingLeft, speed, isImmediate));
            return co_flipping;
        }

        private IEnumerator FacingDirection(bool faceLeft, float speedMultiplier, bool isImmediate)
        {
            float xScale = faceLeft ? 1 : -1;
            Vector3 newScale = new Vector3(xScale, 1, 1);

            if (!isImmediate)
            {
                Image newRenderer = CreateRenderer(Renderer.transform.parent);
                newRenderer.transform.localScale = newScale;

                _transitionSpeedMultiplier = speedMultiplier;
                TryStartLevelingAlpha();
                while (isLevelingAlpha)
                {
                    yield return null;
                }
            }
            else
            {
                Renderer.transform.localScale = newScale;
            }
            yield return null;
            co_flipping = null;
        }
    }

}
