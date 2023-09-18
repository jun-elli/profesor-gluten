using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Level.CustomSO;
using Dialogue;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage;
    private bool itemHasGluten;
    private Item item;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInformation(GameObject itemToDisplay)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        item = itemToDisplay.GetComponent<Item>();

        // name
        nameText.text = item.ItemName;
        // image
        itemImage.sprite = item.Sprite;
        // has gluten
        itemHasGluten = item.HasGluten;
    }

    public void HandleHasGlutenClick(bool userAnswer)
    {
        if (itemHasGluten == userAnswer)
        {
            Debug.Log("You're correct");

            // Call Dialogue System to run success text
            DialogueSystem.Instance.PlayConversation(item.SuccessDialogueFile);

            // add point
            levelManager.SetPoints(1);
        }
        else
        {
            Debug.Log("You're wrong!");

            // Call Dialogue System to run failure text
            DialogueSystem.Instance.PlayConversation(item.FailureDialogueFile);

            // subtract life
            levelManager.SetLives(-1);
        }
        // Disable items
        item.gameObject.SetActive(false);
        // If there are no items left to click, end level
        levelManager.CheckForActiveItemsOnScene();

    }

}
