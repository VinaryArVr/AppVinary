using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour
{

    public Button buttonComponent;
    public RawImage iconImage;
  


    private Item item;
    private ShopScrollList scrollList;

    // Use this for initialization
    void Start()
    {
       
    }

    public void Setup(Item currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        iconImage.texture= item.icon;
        scrollList = currentScrollList;

    }

    
}