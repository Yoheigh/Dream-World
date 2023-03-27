using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public float moveSpeed = 1;
    public GameObject PlayerPivot;              //ĳ���� �Ǻ�
    Camera viewCamera;                          //���콺 ��ġ�� ���� �þ� ����
    Vector3 velocity;
    Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;
        // ������ ���� �ڵ� ��
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        PlayerPivot.transform.LookAt(mousePos + Vector3.up * PlayerPivot.transform.position.y);
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        // ���� �ڵ�
        // GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + velocity * Time.fixedDeltaTime);
    }
}