using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //으아악
    public int currentSceneIndex = 0;

    public void Init()
    {
        currentSceneIndex = 1;
    }

    public void Setup()
    {
        // 학교에서 시작할 때 컷씬 넣으려고 했던 것
        Cam.isFollowPlayer = true;
        Input.CanLook(true);
        Input.CanMove(true);

        //Input.CanMove(false);
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
        yield return new WaitForSecondsRealtime(1f);

        Cam.HandleCameraMove(cameraPoint);
        yield return new WaitForSecondsRealtime(1f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSecondsRealtime(1f);

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
        UI.PlayerUI.SetActive(true);
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);
        Manager.Instance.Player.enabled = true;
    }

    // 게임 오버
    public void GameOver()
    {
        StartCoroutine(GameOverCo());
    }

    public IEnumerator DoorAction001(Transform door, Transform cameraPoint)
    {
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        Cam.HandleCameraTarget(null);
        yield return new WaitForSecondsRealtime(1f);

        Cam.HandleCameraMove(cameraPoint);
        yield return new WaitForSecondsRealtime(1f);

        float currentTime = 0;
        float lerpTime = 2f;

        Vector3 돌문 = door.transform.position;

        while (currentTime < lerpTime)
        {
            float t = currentTime + Time.unscaledTime / lerpTime;
            door.transform.position = Vector3.Lerp(돌문, new Vector3(돌문.x, -3f, 돌문.z), t);
        }
        yield return new WaitForSecondsRealtime(3f);

        Cam.ReturnCameraToPlayer();
        yield return new WaitForSecondsRealtime(1f);

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        Cam.isFollowPlayer = true;
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);
    }

    // 게임 오버 기능
    private IEnumerator GameOverCo()
    {
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        // 얘네는 이따 수정해야 함!
        UI.PlayerUI.SetActive(false);
        Manager.Instance.Player.anim.ChangeAnimationState("Hit");
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(1f);

        Manager.Instance.Player.anim.ChangeAnimationState("Die");

        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        op.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(1f);

        Manager.Instance.UI.Transition.CircleIn();

        yield return new WaitForSecondsRealtime(3f);

        op.allowSceneActivation = true;

        Manager.Instance.UI.Transition.CircleOut();
    }

    public void OutOfBorder(Transform respawnPoint)
    {
        if(isFlagNotOver) return;
        StartCoroutine(OutOfBorderCo(respawnPoint));
    }

    private IEnumerator OutOfBorderCo(Transform respawnPoint)
    {
        isFlagNotOver = true;
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        // 플레이어 낙뎀 안 입게
        Manager.Instance.Player.isInvincible = true;

        Cam.HandleCameraTarget(null);
        Manager.Instance.UI.Transition.CircleIn();
        yield return new WaitForSecondsRealtime(2f);

        Cam.ReturnCameraToPlayer();
        Cam.isFollowPlayer = true;
        Manager.Instance.Player.transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
        Manager.Instance.UI.Transition.CircleOut();
        yield return new WaitForSecondsRealtime(2f);

        Manager.Instance.Player.ChangeState(PlayerStateType.Default);
        Manager.Instance.Player.isInvincible = false;

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);

        Manager.Instance.Player.Hit();
        isFlagNotOver = false;
    }

    public void NextScene()
    {
        if (isFlagNotOver) return;
        StartCoroutine(LoadNextSceneTransition());
    }

    private IEnumerator LoadNextSceneTransition()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(currentSceneIndex++);

        Manager.Instance.UI.Transition.CircleIn();

        op.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(2.5f);

        op.allowSceneActivation = true;

        Manager.Instance.UI.Transition.CircleOut();

        yield return new WaitForSecondsRealtime(2.5f);
        isFlagNotOver = false;
    }

    //public IEnumerator LoadScene(int nextScene, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
    //    op.allowSceneActivation = false;

    //    while (!op.isDone)
    //    {
    //        Debug.Log("에베베 로딩안됨");
    //        yield return null;
    //    }

    //    Debug.Log("오 다 됨");
    //    op.allowSceneActivation = true;
    //}

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