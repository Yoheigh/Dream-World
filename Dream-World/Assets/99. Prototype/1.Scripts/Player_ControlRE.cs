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
        rigid = GetComponent<Rigidbody>(); //Rigidbody 컴포넌트를 받아옴
        IsJumping = false;  //점프 중인지 판단할 수 있도록 bool 값 생성, 초기화
    }

    void Update()
    {
        Move();
        Jump();
        CheckUseItem();
    }

    #region 열지 마시오
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

        // 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(h == 0 && v == 0))
        {
            // 이동과 회전을 함께 처리
            transform.position += dir * MoveSpeed * Time.deltaTime;
            // 회전하는 부분. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotateSpeed);
        }
    }

    void Jump()
    {
        //스페이스 키를 누르면 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //바닥에 있으면 점프를 실행
            if (!IsJumping)
            {
                //print("점프 가능 !");
                IsJumping = true;
                rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            }

            //공중에 떠있는 상태이면 점프하지 못하도록 리턴
            else
            {
                //print("점프 불가능 !");
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //바닥에 닿으면
        if (collision.gameObject.CompareTag("Ground"))
        {
            //점프가 가능한 상태로 만듦
            IsJumping = false;
        }
    }
}

//public enum PlayerState
//{
//    Idle, UseItem
//}