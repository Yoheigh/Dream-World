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
    [Tooltip("도구 효과를 받을 타입")]
    private EffectiveType effectiveType;

    [SerializeField]
    [Tooltip("드롭할 아이템 프리팹")]
    private GameObject DropItemPrefab;

    [SerializeField]
    [Tooltip("드롭할 아이템 개수")]
    private int dropAmount = 1;

    [SerializeField]
    private float spreadOffset = 0.03f;

    [SerializeField]
    private float spreadForce = 0.5f;

    public void AffectedByEquipment(EffectiveType _effectiveType)
    {
        if (_effectiveType != effectiveType) return;
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
        
        Destroy(gameObject);
        yield return null;
    }
}
