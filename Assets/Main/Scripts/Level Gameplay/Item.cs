using System.Collections;
using System.Collections.Generic;
using Level.CustomSO;
using UnityEngine;
using UnityEngine.UI;

namespace Level.CustomSO
{
    public class Item : MonoBehaviour
    {

        // SO information
        [SerializeField] private ItemScriptableObject _itemSO;
        public string ItemName { get; private set; }
        public string Description { get; private set; }
        public bool HasGluten { get; private set; }
        public Sprite Sprite { get; private set; }
        public TextAsset SuccessDialogueFile { get; private set; }
        public TextAsset FailureDialogueFile { get; private set; }




        // Start is called before the first frame update
        void Start()
        {

            InitializeItem();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnMouseDown()
        {

        }

        private void InitializeItem()
        {
            ItemName = _itemSO.itemName;
            Description = _itemSO.description;
            HasGluten = _itemSO.hasGluten;
            Sprite = _itemSO.sprite;
            SuccessDialogueFile = _itemSO.successDialogueFile;
            FailureDialogueFile = _itemSO.failureDialogueFile;

            // Add sprite to image compoenent
            gameObject.GetComponent<Image>().sprite = Sprite;
        }
    }
}