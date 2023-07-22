using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterSprite : Character
    {
        private CanvasGroup rootCanvasGroup => rootTransform.GetComponent<CanvasGroup>();
        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab)
        {
            rootCanvasGroup.alpha = 0;
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

    }
}