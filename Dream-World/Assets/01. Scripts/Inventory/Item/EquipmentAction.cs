using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentActionType
{
    Melee,
    Ranged
}

public abstract class EquipmentAction
{
    /* 어드레서블 가능? */
    [Header("장비 기능 설정")]
    public EquipmentActionType EquipActionType;     // 장비 액션 타입 ( 근접, 원거리 등 )
    public string EquipPrefabPath;                  // 장비 모델 경로
    public string EquipAnimName;                    // 플레이어 동작 애니메이션 이름

    public float EquipRange;                        // 장비 동작 범위
    public float EquipAnimationDelay;               // 장비 동작 후 딜레이
    public float EquipAfterDelay;                   // 장비 동작 후 딜레이
}