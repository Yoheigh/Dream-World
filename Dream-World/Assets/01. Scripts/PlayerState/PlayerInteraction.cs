using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float TriggerTime = 0.5f;  // 사다리에 비비기 상호작용 테스트
    
    [SerializeField]
    private float triggerTime;

    [SerializeField]
    private bool isInteracting = false;
    public bool IsInteracting
    {
        get => isInteracting;
        set => isInteracting = value;
    }

    private PlayerEquipment currentEquipment;

    private Collider obj;

    private CustomInput input;
    private FOVSystem fov;
    private PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<CustomInput>();
        fov = GetComponent<FOVSystem>();
        move = GetComponent<PlayerMovement>();
        triggerTime = TriggerTime;
    }

    public void Interact()
    {

    }

    public void InteractWithEquipment()
    {
        currentEquipment.InteractWithEquipment();
    }

    private void Update()
    {
        if (isInteracting) return;

        if (triggerTime <= 0.0f)
        {
            if (obj != null)
            {
                SnapPlayerPos();
                isInteracting = true;
                triggerTime = TriggerTime;
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (isInteracting) return;

        if (col.CompareTag("Ladder"))
        {
            triggerTime = input.move.magnitude > 0.01f ? triggerTime - Time.deltaTime : TriggerTime;
            obj = col;
        }
    }
    
    private void SnapPlayerPos()
    {
        var ladder = obj.transform.GetComponent<Ladder>();
        move.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
        move.ChangeMoveState(PlayerStateType.Climbing);
        obj = null;
    }
}
