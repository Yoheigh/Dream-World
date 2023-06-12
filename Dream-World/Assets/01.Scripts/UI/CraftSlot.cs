using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftSlot : MonoBehaviour
{
    InventoryV2 Inventory => Manager.Instance.Inventory;

    // �ش� ������
    public ItemRecipe itemRecipe; 

    // �տ��� ������ �͵�
    public GameObject[] slotView;
    public Image[] needImage;
    public Text[] needCount;

    public Button CreateButton;

    // �ϼ� ������ �������ε� �̰� ���̾ƿ� �ٲ�� Ȯ �ٲ� ��
    // public Image resultImage;

    // ���� ����
    private string tempString;

    private void Start()
    {
        Draw();
    }

    public void Draw()
    {
        tempString = null;

        // ũ����Ʈ ���� �ʱ�ȭ
        for (int i = 0; i < needImage.Length; i++)
        {
            needImage[i].gameObject.SetActive(false);
            needCount[i].gameObject.SetActive(false);
        }
        Debug.Log("���� �ʱ�ȭ");

        for (int i = 0; i < itemRecipe.needItemCount; i++)
        {
            if (Inventory.GetInventoryItem(itemRecipe.ingredients[i]) != null)
            {
                needCount[i].text = $"{Inventory.GetInventoryItem(itemRecipe.ingredients[i]).itemCount} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
            }
            else
            {
                needCount[i].text = $"{0} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
                Debug.Log("������ ������ 0��");
            }

            needImage[i].gameObject.SetActive(true);
            needCount[i].gameObject.SetActive(true);
        }

        
        // resultImage.sprite = itemRecipe.result.itemIcon;
    }
}
