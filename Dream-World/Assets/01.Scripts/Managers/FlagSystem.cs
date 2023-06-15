using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FlagSystem : MonoBehaviour
{
    CameraSystem Cam => Manager.Instance.Camera;
    CustomInput Input => Manager.Instance.Input;

    // 실제 작동할 오브젝트 키 모음
    public Dictionary<int, StageFlag> stageFlags;

    // 우오옹
    public List<StageFlag> tests;

    public bool isFlagNotOver = false;

    public void Setup()
    {
        stageFlags = new Dictionary<int, StageFlag>
        {
            { 100, new StageFlag() }
        };

        tests = new List<StageFlag>()
        {
            new StageFlag(),
            new CameraCutscene() as StageFlag
        };

    }

    public void ExcuteFlag(int flagID)
    {
        if (isFlagNotOver) return;

        isFlagNotOver = true;
        StartCoroutine(stageFlags[100].FlagAction());
    }

}

public enum FlagPlayerState
{
    OnPlaying,
    CutScene
}

[System.Serializable]
public class StageFlag
{
    CameraSystem Cam => Manager.Instance.Camera;
    CustomInput Input => Manager.Instance.Input;
    UISystemManager UI => Manager.Instance.UI;

    public int flagID;

    public FlagPlayerState flagPlayerState = FlagPlayerState.CutScene;

    private Transform newPoint;

    // 작동 가능 여부
    public bool isAvailable = false;

    // flag 활성화 조건
    public List<Action> requireActions;

    public IEnumerator FlagAction()
    {
        switch (flagPlayerState)
        {
            case FlagPlayerState.OnPlaying:
                break;

            case FlagPlayerState.CutScene:
                Cam.VerticalBar.SetActive(true);
                Cam.isFollowPlayer = false;
                Input.CanMove(false);
                Input.CanLook(false);
                Input.CanInteract(false);
                UI.CloseAll();
                break;
        }

        Cam.HandleCameraTarget(null);
        yield return new WaitForSeconds(1f);

        Cam.HandleCameraMove(newPoint);
        yield return new WaitForSeconds(2f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSeconds(1f);


        // 컷씬 타입이었으면 다시 상태로 돌아오도록
        if (flagPlayerState == FlagPlayerState.CutScene)
        {
            Cam.isFollowPlayer = true;
            Cam.VerticalBar.SetActive(false);
            Input.CanMove(true);
            Input.CanLook(true);
            Input.CanInteract(true);
        }
    }
}

[System.Serializable]
public class CameraCutscene : StageFlag
{

}