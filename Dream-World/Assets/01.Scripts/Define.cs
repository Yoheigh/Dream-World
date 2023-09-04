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

    public enum ObjectType
    {
        Player,
        Monster,
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
