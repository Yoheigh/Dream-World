using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Click = 0, Hold, Both
}

public abstract class InteractionObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public InteractionType interactionType;
    // GameManager

    public abstract void Interact(PlayerController playerController);

    protected virtual void Setup()
    {
        switch(interactionType)
        {
            case InteractionType.Click:
                break;

            case InteractionType.Hold:
                break;

            case InteractionType.Both:
                break;
        }
    }
}
