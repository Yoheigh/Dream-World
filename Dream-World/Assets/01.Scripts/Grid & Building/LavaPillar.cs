using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPillar : MonoBehaviour
{
    public bool isActivated = false;
    public float lerpTime = 4f;
    public float stopTime = 4f;

    public void Activate()
    {
        isActivated = true;
    }

    IEnumerator PillarActivateCO()
    {
        yield return new WaitForSeconds(stopTime);
        StartCoroutine("PillarActivateCO");
    }
}
