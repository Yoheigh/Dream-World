using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractionObject
{
    public override ObjectType ObjectType => ObjectType.StageObject;

    public Vector3 AnchorPivot;

    [SerializeField]
    private GameObject LadderModel;

    //[SerializeField]
    //private int maxConstHeight = 3;

    [SerializeField]
    private int baseConstHeight = 1;

    [SerializeField]
    private int ConstHeight = 1;

    [SerializeField]
    private float baseReachHeight = 1.1f;

    // 얘 프로퍼티로 바꿔줄 예정
    public float ReachHeight = 1.1f;

    public bool isStageObject = false;

    private void Start()
    {
        if(isStageObject)
            Construct();
    }

    public void Construct()
    {
        if (ConstHeight == baseConstHeight) return;

        StartCoroutine(ConstructionCoroutine());

        ReachHeight = ConstHeight;
    }
    private IEnumerator ConstructionCoroutine()
    {
        for (int i = baseConstHeight; i < ConstHeight; i++)
        {
            // Instantiate(LadderModel, transform.position + (Vector3.up * i), Quaternion.Euler(PivotRot), this.transform);
            // Instantiate(Resources.Load<GameObject>("07.VFX/VFX_DustPoof"), transform.position + (Vector3.up * i), Quaternion.Euler(PivotRot), this.transform);
            Debug.Log("뚝 딱 뚝 딱");
            yield return new WaitForSeconds(0.8f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawSphere(Pivot, 0.1f);
    }

    public override void InteractWithPlayer(PlayerController _player)
    {
        _player.interaction.InteractionObj = this;
        _player.movement.SetVerticalPoint(AnchorPivot + transform.position, ReachHeight);
        _player.ChangeState(PlayerStateType.Climbing);
    }
}
