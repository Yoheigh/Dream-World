using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rotate : MonoBehaviour
{
    public GameObject stage;

    private float xRotateMove, yRotateMove;

    public float rotateSpeed = 500.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

            Vector3 stagePosition = stage.transform.position;

            transform.RotateAround(stagePosition, Vector3.up, xRotateMove);

            transform.LookAt(stagePosition);
        }
    }
}
