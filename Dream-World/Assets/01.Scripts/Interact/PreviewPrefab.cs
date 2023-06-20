using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPrefab : MonoBehaviour
{
    

    [SerializeField]
    private Material previewMaterial;

    // ���� ����
    private Vector3 beforePos;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos(transform.forward);
    }

    public void UpdatePos(Vector3 blockPointerPos)
    {
        if(beforePos != blockPointerPos)
        {
            beforePos = blockPointerPos;
        }
    }
}
