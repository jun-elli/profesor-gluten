using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Characters
{
    public class CharacterSpriteLayer
    {
        private CharacterManager chaManager => CharacterManager.Instance;

        public int LayerNum { get; private set; } = 0;
        public Image Renderer { get; private set; } = null;
        public CanvasGroup rendererCG => Renderer.GetComponent<CanvasGroup>();
        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        public CharacterSpriteLayer(Image defaultRenderer, int layerNum = 0)
        {
            LayerNum = layerNum;
            Renderer = defaultRenderer;
        }

        public void SetSprite(Sprite sprite)
        {
            Renderer.sprite = sprite;
        }

    }

}
