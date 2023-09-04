using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Define;

[System.Serializable]
public class StageFlag
{
    CameraSystem Cam => Managers.Cam;
    CustomInput Input => Managers.Input;
    UISystemManager UI => Managers.UI;

    // public int flagID;

    public FlagPlayerState flagPlayerState = FlagPlayerState.CutScene;
    public FlagActionEnum flagActionEnum = FlagActionEnum.CameraMove;

    private Transform newPoint;

    // �۵� ���� ����
    public bool isAvailable = false;

    // flag Ȱ��ȭ ����
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

        // �ƾ� Ÿ���̾����� �ٽ� �÷��̾�� ���·� ���ƿ�����
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
