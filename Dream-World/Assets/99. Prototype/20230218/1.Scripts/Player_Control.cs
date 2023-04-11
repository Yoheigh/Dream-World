using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public int JumpPower;
 
    private Rigidbody PlayerRigidbody;
    public float speed = 10f;

    private bool IsJumping;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void Update()
    {
        //������� �������� �Է°��� �����Ͽ� ����
        float xInput = Input.GetAxis("Horizontal"); //������
        float zInput = Input.GetAxis("Vertical"); //������

        //���� �̵� �ӵ��� �Է°��� �̵� �ӷ��� ����� ����
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        //Vector3 �ӵ��� (xSpeed,0f,zSpeed)�� ����
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        //������ �ٵ��� �ӵ��� newVelocity �Ҵ�
        PlayerRigidbody.velocity = newVelocity;
        Jump();
    }

    void Jump()
    {
        //�����̽� Ű�� ������ ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�ٴڿ� ������ ������ ����
            if (!IsJumping)
            {
                //print("���� ���� !");
                IsJumping = true;
                PlayerRigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            }

            //���߿� ���ִ� �����̸� �������� ���ϵ��� ����
            else
            {
                //print("���� �Ұ��� !");
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�ٴڿ� ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            //������ ������ ���·� ����
            IsJumping = false;
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}