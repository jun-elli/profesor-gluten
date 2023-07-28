using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Characters
{
    public class CharacterSprite : Character
    {
        private const string SpriteRenderersParentName = "Renderers";
        private CanvasGroup rootCanvasGroup => rootTransform.GetComponent<CanvasGroup>();
        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();
        private string artAssetsFolder = "";


        // Constructor
        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCanvasGroup.alpha = 0;
            artAssetsFolder = rootAssetsFolder + "/Images";

            GetLayers();

            Debug.Log($"Character Sprite created: {name}");
        }

        public override IEnumerator ShowOrHide(bool show)
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
                Image rendererImage = child.GetComponent<Image>();

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

    }
}