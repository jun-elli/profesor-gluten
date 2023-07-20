using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dialogue.Characters;


namespace Dialogue
{
    /// <summary>
    /// Control speaker name container behaviour
    /// </summary>
    /// 
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameToShow = "")
        {
            root.SetActive(true);
            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
                Debug.Log($"Active in hierarchy?: {root.activeInHierarchy}");
            }
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        void SetNameColor(Color color) => nameText.color = color;
        void SetNameFont(TMP_FontAsset font) => nameText.font = font;

        public void SetConfig(CharacterConfigData config)
        {
            SetNameColor(config.nameColor);
            SetNameFont(config.nameFont);
        }

    }
}
