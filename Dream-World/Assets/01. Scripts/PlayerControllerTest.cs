using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public float moveSpeed = 1;
    public GameObject PlayerPivot;              //캐릭터 피봇
    Camera viewCamera;                          //마우스 위치에 따른 시야 결정
    Vector3 velocity;
    Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;
        // 원본에 없는 코드 ↓
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
        // 원본 코드
        // GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + velocity * Time.fixedDeltaTime);
    }
}