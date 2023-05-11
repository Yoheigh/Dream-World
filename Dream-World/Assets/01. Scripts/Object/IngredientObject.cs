using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum IngredientObjectType
{
    Wood = 0, Rock, Dirt
}

public class IngredientObject : BlockV1
{
    [SerializeField]
    private IngredientObjectType objectType;

    [SerializeField]
    private GameObject DropItemPrefab;

    [SerializeField]
    private int dropAmount = 1;

    [SerializeField]
    private float spreadOffset = 0.03f;

    [SerializeField]
    private float spreadForce = 0.5f;

    public bool isConstrcution = false;

    private void Update()
    {
        if (isConstrcution)
        {
            StartCoroutine(DestructionCoroutine());
            isConstrcution = !isConstrcution;
        }
    }

    public void AffectedByEquipment()
    {
        Destruction();
    }

    public IngredientObjectType GetObjectType()
    {
        return objectType;
    }

    private void Destruction()
    {
        StartCoroutine(DestructionCoroutine());
    }

    private IEnumerator DestructionCoroutine()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("ºÎ¼­Áü!");

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
