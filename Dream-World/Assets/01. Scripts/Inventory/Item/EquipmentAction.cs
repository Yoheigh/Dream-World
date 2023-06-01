using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentAction
{
    /* 어드레서블 가능? */
    [Header("장비 설정")]
    public string EquipPrefabPath;          // 장비 모델 경로
    public string EquipAnimName;            // 플레이어 동작 애니메이션 이름

    public float EquipActionSpeed;          // 장비 동작 속도 -> 기본 : 1배속

    public float EquipRange;                // 장비 동작 범위
    public float EquipAnimationDelay;       // 장비 동작 후 딜레이
    public float EquipAfterDelay;           // 장비 동작 후 딜레이

    public abstract void Action();           // 장비 실제 기능

    public virtual void AfterAction() { }   // 장비 동작이 끝난 후 필요하다면 사용
}