using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.CustomSO
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Custom SO/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public string itemName;
        public string description;
        public bool hasGluten;
        public Sprite sprite;
        public TextAsset successDialogueFile;
        public TextAsset failureDialogueFile;
    }
}
