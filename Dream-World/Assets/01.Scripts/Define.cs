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
        Pickup,         // 1회성 줍는 오브젝트
        Grabable,       // 들고 다닐 수 있는 오브젝트 
        Dragable,       // 끌어다닐 수 있는 오브젝트
        StageObject,    // 상호작용으로 작동하는 오브젝트 ( 예시 : 버튼, 상자 )
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
