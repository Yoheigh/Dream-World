using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("�� �̲����׿�");
            other.GetComponent<PlayerController>().movement.isSlippery = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().movement.isSlippery = false;
        }
    }
}
