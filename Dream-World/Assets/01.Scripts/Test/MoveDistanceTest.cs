using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveDistanceTest : MonoBehaviour
{
    enum MoveType { A, B, C, D };

    [SerializeField]
    private MoveType moveType = MoveType.A;
    
    public float moveSpeed = 1f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        switch (moveType)
        {
            case MoveType.A:
                rb.position += new Vector3(moveSpeed, 0f, 0f) * Time.fixedDeltaTime;
                break;
            case MoveType.B:
                transform.Translate(new Vector3(moveSpeed, 0f, 0f) * Time.fixedDeltaTime);
                break;
            case MoveType.C:
                rb.velocity = new Vector3(moveSpeed, 0f, 0f);
                break;
            case MoveType.D:
                rb.MovePosition(rb.position + new Vector3(moveSpeed, 0f, 0f) * Time.fixedDeltaTime);
                break;
        }
        
    }
}
