using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomPlatformController : MonoBehaviour
{
    public List<CustomPlatformEffect> Effects;

    private GameObject PlatformPivot = null;
    private GameObject MovePoint = null;

    private CustomPlatformBase currentEffect = null;
    [SerializeField]
    private int currentindex = -1;

    public bool isRepeating = false;
    public bool isPlayingAgain = false;

    //void OnValidate()
    //{
    //    if(PlatformPivot == null && MovePoint == null)
    //    {
    //        PlatformPivot = new GameObject("Platform Pivot");
    //        MovePoint = new GameObject("Move Point");

    //        PlatformPivot.transform.localPosition = transform.position;
    //        MovePoint.transform.localPosition = transform.position;

    //        PlatformPivot.transform.SetParent(this.transform);
    //        MovePoint.transform.SetParent(this.transform);
    //    }
    //}

    void Start()
    {

    }

    void Update()
    {
        for(currentindex = 0; currentindex < Effects.Count; ++currentindex)
        {
            Debug.Log(currentindex);
        }
    }

    public void SetNextEffect()
    {

    }

    public void CompleteAllEffects()
    {
        if(isPlayingAgain)
        {

        }

        if(isRepeating)
        {

        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(MovePoint.transform.position, 0.2f);
    //}
}

[System.Serializable]
public class CustomPlatformEffect
{
    public int delay = 0;
    public int duration = 0;
}
