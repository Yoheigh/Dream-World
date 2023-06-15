using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acorn_low : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("∞‘¿”∆˜¿Œ∆Æ »πµÊ!");
        Destroy(gameObject);
        
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    public float rotateSecond;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSecond);
    }
}
