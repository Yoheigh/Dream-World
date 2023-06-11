//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(FOVSystem))]
//public class FOVEditor : Editor
//{
//    private void OnSceneGUI()
//    {
//        FOVSystem fov = (FOVSystem)target;
//        Handles.color = Color.blue;
//        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
//        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, 1f);
//        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
//        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

//        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
//        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

//        Handles.color = Color.red;
//        foreach (Transform visible in fov.visibleTargets)
//        {
//            Handles.DrawLine(fov.transform.position, visible.transform.position);
//        }
//    }
//}