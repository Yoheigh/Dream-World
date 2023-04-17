using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTest : MonoBehaviour
{

    public Vector3 offset = new Vector3(0, 0, 0);

    [SerializeField]
    private GameObject PreviewObject;

    //[SerializeField]
    //private Material previewMaterial = Color.white;

    private FOVSystem fov;

    void Start()
    {
        fov = GetComponent<FOVSystem>();

        PreviewObject.GetComponent<Collider>().enabled = false;
        PreviewObject.GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        PreviewConstructure();
    }

    private void PreviewConstructure()
    {
        if(fov.ClosestTransform != null)
        {
            var temp = Instantiate(PreviewObject, fov.ClosestTransform);
            temp.transform.position = fov.ClosestTransform.position + offset;
        };
    }
}
