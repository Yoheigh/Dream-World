using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientObject : Entity
{
    [SerializeField]
    private GameObject DropItemPrefab;

    [SerializeField]
    private int dropAmount = 3;

    [SerializeField]
    private float spreadOffset = 0.03f;

    [SerializeField]
    private float spreadForce = 0.5f;

    private void Update()
    {
        if (currentHP <= 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            Debug.Log("ºÎ¼­Áü!");

            for (int i = 0; i < dropAmount; i++)
            {
                GameObject dropItem = Instantiate(DropItemPrefab);
                dropItem.transform.position = transform.position;
                dropItem.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-spreadForce, spreadForce), spreadForce, Random.Range(-spreadForce, spreadForce)));
            }

            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int _damage)
    {
        currentHP -= _damage;

        if (currentHP <= 0)
            Destruction();
    }

    private void Destruction()
    {
        StartCoroutine(DestructionCoroutine());
    }

    private IEnumerator DestructionCoroutine()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("ºÎ¼­Áü!");

        for (int i = 0; i < dropAmount; i++)
        {
            GameObject dropItem = Instantiate(DropItemPrefab);
            dropItem.transform.position = transform.position;
            dropItem.GetComponent<Rigidbody>().AddForce(new Vector2(Random.Range(-spreadForce, spreadForce), Random.Range(-spreadForce, spreadForce)));
        }

        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
}
