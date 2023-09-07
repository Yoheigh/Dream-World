using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
        WorldMapScene
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }


    public enum PlayerStateType
    {
        Default = 0,
        Falling,
        Dragging,
        Climbing,
        Interaction,
        Damaged,
        Cinematic
    }

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

    public enum ObjectType
    {
        Pickup,         // 1ȸ�� �ݴ� ������Ʈ
        Grabable,       // ��� �ٴ� �� �ִ� ������Ʈ 
        Dragable,       // ����ٴ� �� �ִ� ������Ʈ
        StageObject,    // ��ȣ�ۿ����� �۵��ϴ� ������Ʈ ( ���� : ��ư, ���� )
    }

    public enum ItemType
    {
        Armor,
        Potion,
        Food
    }

    public enum GraphType
    {
        Linear,
        Ease_Out,
        Ease_In,
        Smoothstep,
        Smootherstep
    }

    public static int ID_BRONZE_KEY = 201;
    public static int ID_SILVER_KEY = 202;
    public static int ID_GOLD_KEY = 203;
}
