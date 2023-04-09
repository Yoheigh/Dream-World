using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomPlatformBase : MonoBehaviour
{
    public abstract void Enter();

    public abstract void Execute();

    public void Exit(CustomPlatformController customPlatformController)
    {
        customPlatformController.CompleteAllEffects();
    }
}
