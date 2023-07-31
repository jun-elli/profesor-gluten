using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Characters
{
    public class CharacterSpriteLayer
    {
        private CharacterManager chaManager => CharacterManager.Instance;

        // Const
        private const float DefaultTransitionSpeed = 3f;

        // Vars
        public int LayerNum { get; private set; } = 0;
        public Image Renderer { get; private set; } = null;
        public CanvasGroup rendererCG => Renderer.GetComponent<CanvasGroup>();
        private List<CanvasGroup> _oldRenderers = new List<CanvasGroup>();
        private float _transitionSpeedMultiplier = 1f;

        // Coroutines
        private Coroutine co_transitioningSprite = null;
        public bool isTransitioningSprite => co_transitioningSprite != null;
        private Coroutine co_levelingAlpha = null;
        public bool isLevelingAlpha => co_levelingAlpha != null;

        // Constructor
        public CharacterSpriteLayer(Image defaultRenderer, int layerNum = 0)
        {
            LayerNum = layerNum;
            Renderer = defaultRenderer;
        }

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
                chaManager.StopCoroutine(co_transitioningSprite);
            }

            co_transitioningSprite = chaManager.StartCoroutine(TransitioningSprite(sprite, speed));
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
            co_levelingAlpha = chaManager.StartCoroutine(RunLevelingAlpha());
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
    }

}
