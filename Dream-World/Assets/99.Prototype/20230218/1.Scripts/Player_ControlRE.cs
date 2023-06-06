using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ControlRE : MonoBehaviour
{
    private Rigidbody rigid;

    public float JumpPower;
    public float MoveSpeed;
    public float RotateSpeed;

    public Item handItem;

    public KeyCode item1;
    public KeyCode item2;
    public KeyCode item3;
    public KeyCode item4;
    public KeyCode structure1;
    public KeyCode structure2;
    public KeyCode structure3;
    public KeyCode structure4;
    public KeyCode stopItem;


    private bool IsJumping;

    void Start()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody ������Ʈ�� �޾ƿ�
        IsJumping = false;  //���� ������ �Ǵ��� �� �ֵ��� bool �� ����, �ʱ�ȭ
    }

    void Update()
    {
        Move();
        Jump();
        CheckUseItem();
    }

    #region ���� ���ÿ�
    public void CheckUseItem()
    {
        if(Input.GetKeyDown(item1) )
        {
            UseItem(1);

        }

        if (Input.GetKeyDown(item2))
        {
            UseItem(2);
        }

        if (Input.GetKeyDown(item3))
        {
            UseItem(3);
        }
        if (Input.GetKeyDown(item4))
        {
            UseItem(4);
        }
        if (Input.GetKeyDown(structure1))
        {
            UseItem(5);
        }
        if (Input.GetKeyDown(structure2))
        {
            UseItem(6);
        }
        if (Input.GetKeyDown(structure3))
        {
            UseItem(7);
        }
        if (Input.GetKeyDown(structure4))
        {
            UseItem(8);
        }
        if(Input.GetKeyDown(stopItem))
        {
            StopUseItem();
            Debug.Log("x");
        }
    }

    public void StopUseItem()
    {
        CraftingTable.instance.SlotOutLineRedrow(-1);

        List < GameObject > childs= new List<GameObject>();
        for(int j = 0; j < gameObject.transform.Find("HandItems").childCount; j++)
        {
            childs.Add(gameObject.transform.Find("HandItems").GetChild(j).gameObject);
        }

        foreach(GameObject gameObject_ in childs)
        {
            Destroy(gameObject_);
        }
    }

    public void UseItem(int itemSlotCount)
    {
        StopUseItem();

        int itemID = CraftingTable.instance.SlotOutLineRedrow(itemSlotCount);



        foreach(Item item in Inventory.instance.equipmentItems)
        {
            if(item.itemID == itemID)
            {   
                Instantiate(Resources.Load<GameObject>(item.itemGameObjectPath) , gameObject.transform.Find("HandItems"));
            }
        }

        //switch(itemSlotCount)
        //{
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //    case 5:
        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //    case 8:
        //        break;

        //}    
    }
    #endregion

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        // �ٶ󺸴� �������� ȸ�� �� �ٽ� ������ �ٶ󺸴� ������ ���� ���� ����
        if (!(h == 0 && v == 0))
        {
            // �̵��� ȸ���� �Բ� ó��
            transform.position += dir * MoveSpeed * Time.deltaTime;
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotateSpeed);
        }
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
                rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
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
}

//public enum PlayerState
//{
//    Idle, UseItem
//}