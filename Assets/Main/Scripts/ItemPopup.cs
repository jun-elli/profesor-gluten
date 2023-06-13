using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    private GameObject item;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage;

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
        item = itemToDisplay;
        // name
        nameText.text = item.name;
        // image
        itemImage.sprite = item.GetComponent<Image>().sprite;
    }
}
