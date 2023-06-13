using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // No setter because it can't be changed from the outside
    [SerializeField] private bool _hasGluten = true;
    public bool HasGluten
    {
        get => _hasGluten;
    }
    [SerializeField] private GameObject itemPopupUI;
    private ItemPopup popup;

    // private reference to dialogue resource here once implemented

    // Start is called before the first frame update
    void Start()
    {
        popup = itemPopupUI.GetComponent<ItemPopup>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseDown()
    {
        // transfer item information to pop up, maybe just reference of itself
        popup.ShowInformation(gameObject);
    }
}
