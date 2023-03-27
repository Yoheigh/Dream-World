using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public Item item = null;
    public TextMeshProUGUI countText;

    public void UpdateState()
    {
        if(item != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = Resources.Load<Sprite>(item.itemIconPath);

            if(item.itemCurrentCount >= 2)
            {
                countText.text = ""+item.itemCurrentCount;
                countText.gameObject.SetActive(true);
            }
        }
        

        else
        {
            GetComponent<Button>().interactable = false;
            icon.gameObject.SetActive(false);
            icon.sprite = null;
            countText.gameObject.SetActive(false);
        }
    }
}
