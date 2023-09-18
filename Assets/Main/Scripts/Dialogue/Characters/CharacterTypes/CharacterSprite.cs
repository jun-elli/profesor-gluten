using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Characters
{
    public class CharacterSprite : Character
    {
        private const string SpriteRenderersParentName = "Renderers";
        private const string SpriteSheetDefaultSheetName = "Default";
        private const string SpriteSheetTextureSpriteDelimiter = ": ";
        private string artAssetsFolder = "";

        public override bool IsVisible
        {
            get { return isRevealing || rootCanvasGroup.alpha == 1; }
            set { rootCanvasGroup.alpha = value ? 1 : 0; }
        }

        // Visuals
        private CanvasGroup rootCanvasGroup => rootTransform.GetComponent<CanvasGroup>();
        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();



        // Constructor
        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCanvasGroup.alpha = ShowOnStart ? 1 : 0;
            artAssetsFolder = rootAssetsFolder + "/Images";

            GetLayers();

            Debug.Log($"Character Sprite created: {name}");
        }

        protected override IEnumerator ShowOrHide(bool show)
        {
            float targetAlpha = show ? 1f : 0;

            while (rootCanvasGroup.alpha != targetAlpha)
            {
                rootCanvasGroup.alpha = Mathf.MoveTowards(rootCanvasGroup.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }
            co_hiding = null;
            co_revealing = null;
        }

        private void GetLayers()
        {
            Transform rendererRoot = animator.transform.Find(SpriteRenderersParentName);

            if (rendererRoot == null)
            {
                return;
            }

            for (int i = 0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);
                Image rendererImage = child.GetComponentInChildren<Image>();

                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }

            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (config.characterType == CharacterType.SpriteSheet)
            {
                string[] names = spriteName.Split(SpriteSheetTextureSpriteDelimiter);
                string spriteToLookFor;
                Sprite[] sheetSprites;

                // Example -> "Deafult: A_happy"
                if (names.Length == 2)
                {
                    // Load all sprites in spritesheet = names[0]
                    sheetSprites = Resources.LoadAll<Sprite>($"{artAssetsFolder}/{names[0]}");
                    spriteToLookFor = names[1];
                }
                else
                {
                    sheetSprites = Resources.LoadAll<Sprite>($"{artAssetsFolder}/{SpriteSheetDefaultSheetName}");
                    spriteToLookFor = spriteName;
                }

                // Check we have sprites
                if (sheetSprites.Length < 1)
                {
                    Debug.LogWarning($"Sprite sheet for '{spriteName}' not found or empty.");
                }

                // Get sprite
                return Array.Find<Sprite>(sheetSprites, sprite => sprite.name == spriteToLookFor);
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsFolder}/{spriteName}");
            }
        }

        // Exchange sprite over time
        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer csLayer = layers[layer];
            return csLayer.TransitionSprite(sprite, speed);
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);
            color = displayColor;

            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color, speed);
            }
            yield return null;

            while (layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }

            co_changingColor = null;
        }

        protected override IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Color targetColor = displayColor;

            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(targetColor, speedMultiplier);
            }

            yield return null;

            while (layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }

            co_highlighting = null;
        }

        protected override IEnumerator FacingDirection(bool faceLeft, float speedMultiplier, bool isImmediate)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                if (faceLeft)
                {
                    layer.FaceLeft(speedMultiplier, isImmediate);
                }
                else
                {
                    layer.FaceRight(speedMultiplier, isImmediate);
                }
            }
            yield return null;

            while (layers.Any(l => l.isFlipping))
            {
                yield return null;
            }

            co_flipping = null;
        }

        // Expressions

        public override void SetCastingExpression(int layer, string expression)
        {
            Sprite sprite = GetSprite(expression);
            if (sprite == null)
            {
                Debug.LogWarning($"Sprite '{expression}' couldn't be found for character '{name}'.");
                return;
            }
            TransitionSprite(sprite, layer);
        }
    }
}