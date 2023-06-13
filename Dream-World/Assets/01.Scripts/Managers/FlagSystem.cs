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

    public bool isFlagNotOver = false;

    public void Setup()
    {
        stageFlags = new Dictionary<int, StageFlag>
        {
            { 100, new StageFlag() }
        };
        Debug.Log("생성함 ㅅㄱ");
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

    public int flagID;

    public FlagPlayerState flagPlayerState = FlagPlayerState.CutScene;

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
                Cam.isFollowPlayer = false;
                Input.CanMove(false);
                Input.CanLook(false);
                Input.CanInteract(false);
                break;
        }

        Cam.HandleCameraTarget(null);
        Cam.VerticalBar.SetActive(true);
        yield return new WaitForSeconds(1f);

        Cam.HandleCameraMove(Manager.Instance.Camera.newPoint.transform);
        yield return new WaitForSeconds(2f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSeconds(1f);

        Cam.VerticalBar.SetActive(false);

        if (flagPlayerState == FlagPlayerState.CutScene)
        {
            Cam.isFollowPlayer = true;
            Input.CanMove(true);
            Input.CanLook(true);
            Input.CanInteract(true);
        }

        Debug.Log("플래그 액션 끝~");
    }

}