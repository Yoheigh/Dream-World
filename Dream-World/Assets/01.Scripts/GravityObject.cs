using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.position.y < gameObject.transform.position.y + 0.1f)
        {
            this.gameObject.transform.SetParent(col.transform);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.transform.position.y < gameObject.transform.position.y + 0.1f)
        {
            this.gameObject.transform.SetParent(null);
        }
    }
}
