using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

[System.Serializable]
public class FlagSystem : MonoSingleton<FlagSystem>
{
    CameraSystem Cam => Managers.Cam;
    CustomInput Input => Managers.Input;
    UISystemManager UI => Managers.UI;

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
        Input.CanMove(false);
        Input.CanInteract(false);
        UI.CloseAll();
        UI.SystemUI.SetActive(false);
        Cam.HandleCameraTarget(null);
        Managers.Instance.Player.enabled = false;

        Cam.HandleCameraMove(cameraPoints[0], 1f);
    }

    public void Setup()
    {
        Cam.isFollowPlayer = true;
        Input.CanLook(true);
        Input.CanMove(true);
        Input.CanInteract(true);
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
        Managers.Instance.Player.enabled = true;
    }

    // 게임 오버
    public void GameOver()
    {
        StartCoroutine(GameOverCo());
    }

    // 게임 오버 기능
    private IEnumerator GameOverCo()
    {
        Managers.Sound.PlaySFX(1);
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        // 얘네는 이따 수정해야 함!
        UI.PlayerUI.SetActive(false);
        Managers.Instance.Player.anim.ChangeAnimationState("Hit");
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(1f);

        Managers.Instance.Player.anim.ChangeAnimationState("Die");

        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        op.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(1f);

        Managers.UI.Transition.CircleIn();

        yield return new WaitForSecondsRealtime(3f);

        op.allowSceneActivation = true;

        Managers.UI.Transition.CircleOut();
    }

    public void OutOfBorder(Transform respawnPoint)
    {
        if (isFlagNotOver) return;
        StartCoroutine(OutOfBorderCo(respawnPoint));
    }

    private IEnumerator OutOfBorderCo(Transform respawnPoint)
    {
        isFlagNotOver = true;
        Managers.Instance.Player.ChangeState(PlayerStateType.Cinematic);
        UI.VerticalBar.SetActive(true);
        Cam.isFollowPlayer = false;
        Input.CanMove(false);
        Input.CanLook(false);
        Input.CanInteract(false);
        UI.CloseAll();

        // 플레이어 낙뎀 안 입게
        Managers.Instance.Player.isInvincible = true;

        Cam.HandleCameraTarget(null);
        Managers.UI.Transition.CircleIn();
        yield return new WaitForSecondsRealtime(2f);

        Cam.ReturnCameraToPlayer();
        Cam.isFollowPlayer = true;
        Managers.Instance.Player.transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
        Managers.UI.Transition.CircleOut();
        yield return new WaitForSecondsRealtime(2f);

        Managers.Instance.Player.ChangeState(PlayerStateType.Default);
        Managers.Instance.Player.isInvincible = false;

        // 컷씬 타입이었으면 다시 플레이어블 상태로 돌아오도록
        UI.VerticalBar.SetActive(false);
        Input.CanMove(true);
        Input.CanLook(true);
        Input.CanInteract(true);

        Managers.Instance.Player.Hit();
        isFlagNotOver = false;
    }

    public void NextSceneWithTransition()
    {
        if (isFlagNotOver) return;
        StartCoroutine(LoadNextSceneTransition());
    }

    /* 비동기 씬 로드 하려고 했는데 안 되서 나중에 봐야 함 */
    private IEnumerator LoadNextSceneTransition()
    {
        currentSceneIndex++;
        // AsyncOperation op_before = SceneManager.UnloadSceneAsync(currentSceneIndex - 1);
        AsyncOperation op_next = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single);

        // op_before.allowSceneActivation = false;
        op_next.allowSceneActivation = false;

        Managers.UI.Transition.CircleIn();

        yield return new WaitForSecondsRealtime(2.5f);

        //if (op_next.isDone && op_before.isDone)
        //{ Debug.LogAssertion("씬 전환 준비됨!"); }

        //op_before.allowSceneActivation = true;
        op_next.allowSceneActivation = true;

        // SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentSceneIndex));

        Managers.UI.Transition.CircleOut();

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