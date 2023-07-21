using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EffectiveType
{
    Shovel,
    Axe,
    Pickaxe,
}

public class IngredientObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���� ȿ���� ���� Ÿ��")]
    private EffectiveType effectiveType;

    [SerializeField]
    [Tooltip("����� ������ ������")]
    private GameObject DropItemPrefab;

    [SerializeField]
    [Tooltip("����� ������ ����")]
    private int dropAmount = 1;

    [SerializeField]
    [Tooltip("�μ��ڸ��� ��� �Ǵ� ����")]
    private int GetAmount = 0;

    [SerializeField]
    private float spreadOffset = 0.03f;

    [SerializeField]
    private float spreadForce = 0.5f;

    public void AffectedByEquipment(EffectiveType _effectiveType)
    {
        if (_effectiveType != effectiveType) return;

        switch (effectiveType)
        {
            case EffectiveType.Shovel:
                Manager.Instance.Sound.PlaySFX(3);
                break;
            case EffectiveType.Axe:
                Manager.Instance.Sound.PlaySFX(5);
                break;
            case EffectiveType.Pickaxe:
                Manager.Instance.Sound.PlaySFX(6);
                break;
        }

        GameObject obj = Instantiate(Manager.Instance.Build.BuildVFX, transform.position, Quaternion.identity);
        Destroy(obj, 4f);
        Destruction();
    }

    public EffectiveType GetObjectType()
    {
        return effectiveType;
    }

    protected virtual void Destruction()
    {
        StartCoroutine(DestructionCoroutine());
    }

    private IEnumerator DestructionCoroutine()
    {
        gameObject.GetComponent<Collider>().enabled = false;

        if(DropItemPrefab != null)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                GameObject dropItem = Instantiate(DropItemPrefab);
                dropItem.transform.position = transform.position;
                dropItem.GetComponent<Rigidbody>().AddForce(new Vector2(Random.Range(-spreadForce, spreadForce), Random.Range(-spreadForce, spreadForce)));
            }
        }

        // �μ��ڸ��� ������ ���� �� �ְ� ����
        if(GetAmount != 0)
        {
            Manager.Instance.Inventory.AddItem(DropItemPrefab.GetComponent<Pickup>().item);
        }
        
        Destroy(gameObject);
        yield return null;
    }
}
