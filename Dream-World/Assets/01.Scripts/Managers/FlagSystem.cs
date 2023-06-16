using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;

public enum FlagPlayerState
{
    OnPlaying,
    CutScene
}

public enum FlagActionEnum
{
    CameraMove,
    CameraReturnToPlayer,
    StartDialog
}

[System.Serializable]
public class FlagSystem : MonoBehaviour
{
    CameraSystem Cam => Manager.Instance.Camera;
    CustomInput Input => Manager.Instance.Input;
    UISystemManager UI => Manager.Instance.UI;

    // 실제 작동할 오브젝트 키 모음
    /* 플래그들을 한 번에 저장하고 StateMachine 처럼 재생시키는 법 연구해야 함 */
    public Dictionary<int, StageFlag> stageFlags;

    public List<StageFlag> tests;

    // 스테이지 별로 카메라 포인트 따로 둡시다
    // 나중에는 StageFlag 에서 관리할 거임
    public Transform[] cameraPoints;

    public bool isFlagNotOver = false;

    public void Setup()
    {
        // 학교에서 시작할 때 컷씬 넣으려고 했던 것
        //Cam.isFollowPlayer = false;
        //Input.CanMove(false);
        //Input.CanLook(false);
        //Input.CanInteract(false);
        //UI.CloseAll();
        //UI.SystemUI.SetActive(false);
        //Cam.HandleCameraTarget(null);
        //Manager.Instance.Player.enabled = false;

        //Cam.HandleCameraMove(cameraPoints[0], 1f);
    }

    public void ForestCutsceneStart()
    {
        StartCoroutine(CameraMove2(cameraPoints[1]));
    }

    public void ExcuteFlag(int flagsID)
    {
        if (isFlagNotOver) return;

        isFlagNotOver = true;
        // 코루틴 시작
        // 모든 코루틴이 종료되면
        // 다음 flag의 코루틴이 시작

        // flag가 없을 경우
        isFlagNotOver = false;
    }

    public void CameraMoveFlag(Transform cameraPoint)
    {
        StartCoroutine(CameraMove(cameraPoint));
    }

    #region 나중에 수정할 거임
    public IEnumerator CameraMove(Transform cameraPoint)
    {
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        Cam.HandleCameraTarget(null);
        yield return new WaitForSeconds(1f);

        Cam.HandleCameraMove(cameraPoint);
        yield return new WaitForSeconds(2f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSeconds(1f);

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        Cam.isFollowPlayer = true;
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);
    }

    public IEnumerator CameraMove2(Transform cameraPoint)
    {
        UI.VerticalBar.SetActive(true);

        yield return new WaitForSeconds(2f);

        Debug.Log("카메라 이동 시작");
        Cam.HandleCameraMove(cameraPoint, 8f);
        yield return new WaitForSeconds(8f);
        Debug.Log("카메라 이동 종료");

        Debug.Log("플레이어한테 이동 시작");
        Cam.ReturnCameraToPlayer();
        yield return new WaitForSeconds(1f);
        Debug.Log("플레이어한테 이동 종료");

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        Cam.isFollowPlayer = true;
        UI.SystemUI.SetActive(true);
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);
        Manager.Instance.Player.enabled = true;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCo());
    }

    private IEnumerator GameOverCo()
    {
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        // 얘네는 이따 수정해야 함!
        UI.HP.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        Manager.Instance.UI.Transition.CircleIn();

        yield return new WaitForSeconds(3f);
    }

    #endregion
}

[System.Serializable]
public class StageFlag
{
    CameraSystem Cam => Manager.Instance.Camera;
    CustomInput Input => Manager.Instance.Input;
    UISystemManager UI => Manager.Instance.UI;

    // public int flagID;

    public FlagPlayerState flagPlayerState = FlagPlayerState.CutScene;
    public FlagActionEnum flagActionEnum = FlagActionEnum.CameraMove;

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
                UI.VerticalBar.SetActive(true);
                Cam.isFollowPlayer = false;
                Input.CanMove(false);
                Input.CanLook(false);
                Input.CanInteract(false);
                UI.CloseAll();
                break;
        }

        switch (flagActionEnum)
        {
            case FlagActionEnum.CameraMove:
                #region CameraMove
                Cam.HandleCameraTarget(null);
                yield return new WaitForSeconds(1f);

                Cam.HandleCameraMove(newPoint);
                yield return new WaitForSeconds(2f);

                Cam.ReturnCameraToPlayer();
                yield return new WaitForSeconds(1f);
                #endregion
                break;

            case FlagActionEnum.CameraReturnToPlayer:
                #region CameraReturnToPlayer
                Cam.ReturnCameraToPlayer();
                yield return new WaitForSeconds(1f);
                #endregion
                break;

            case FlagActionEnum.StartDialog:
                #region StartDialog
                #endregion
                break;
        }

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        if (flagPlayerState == FlagPlayerState.CutScene)
        {
            Cam.isFollowPlayer = true;
            UI.VerticalBar.SetActive(false);
            Input.CanMove(true);
            Input.CanLook(true);
            Input.CanInteract(true);

        }

    }

    private IEnumerator CameraMove()
    {
        Cam.HandleCameraTarget(null);
        yield return new WaitForSeconds(1f);

        Cam.HandleCameraMove(newPoint);
        yield return new WaitForSeconds(2f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSeconds(1f);
    }
}

[System.Serializable]
public class ForestStage : StageFlag
{

}