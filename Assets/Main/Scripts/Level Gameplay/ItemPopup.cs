using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    private GameObject item;
    [SerializeField] private GameObject levelManagerObj;
    private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private bool itemHasGluten;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = levelManagerObj.GetComponent<LevelManager>();
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
        item = itemToDisplay;
        // name
        nameText.text = item.name;
        // image
        itemImage.sprite = item.GetComponent<Image>().sprite;
        // has gluten
        itemHasGluten = item.GetComponent<Item>().HasGluten;
    }

    public void HandleHasGlutenClick()
    {
        if (itemHasGluten)
        {
            Debug.Log("You're correct");
            // add point
            levelManager.SetPoints(1);
            // call dialogue with explanation of why it was correct
        }
        else
        {
            Debug.Log("You're wrong!");
            // subtract life
            levelManager.SetLives(-1);
            // call dialogue with explanation why it was wrong answer
        }
    }

}