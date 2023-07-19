using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    }
}
