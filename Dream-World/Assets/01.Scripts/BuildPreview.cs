using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BuildPreview : MonoBehaviour
{
    // 제작할 건물 모습 가져오기
    [SerializeField]
    private GameObject entity;

    // 지을 건물
    public GameObject ConstructurePrefab;

    private bool isConstructing = false;
    private Vector3 tempVector3;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        tempVector3 = ConstructurePrefab.GetComponent<BlockV1>().PivotRot;
        entity = Instantiate(ConstructurePrefab);
        // entity.GetComponentInChildren<Renderer>().material = previewMaterial;
        foreach(Collider item in entity.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }
    }

    private void OnDisable()
    {
        ConstructurePrefab.GetComponent<BlockV1>().PivotRot = tempVector3;
        tempVector3 = Vector3.zero;
        Destroy(entity);
    }

    public void ChangePrefab(GameObject newPrefab)
    {
        ConstructurePrefab = newPrefab;
    }

    private void Update()
    {
        if (isConstructing)
            return;

        entity.transform.position = gameObject.transform.position;
        RotatePreviewBlock();
    }

    public void Build()
    {
        StartCoroutine(ConstructWithEffect());
    }

    private void ConstructionFinish()
    {
        var temp = Instantiate(ConstructurePrefab, entity.transform.position, Quaternion.identity);

        // 캡슐화 필요
        temp.GetComponent<Ladder>().Construct();
    }

    private void RotatePreviewBlock()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            entity.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
            // PivotRot += new Vector3(0f, -90f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            entity.transform.rotation *= Quaternion.Euler(0f, 90f, 0);
            // PivotRot += new Vector3(0f, 90f, 0f);
        }
    }

    private IEnumerator ConstructWithEffect()
    {
        isConstructing = !isConstructing;
        var wait = new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            //float y = Random.Range(-0.5f, 0.5f);
            //float z = Random.Range(-0.5f, 0.5f);
            GameObject obj = Instantiate(Managers.Build.BuildVFX, entity.transform.position, Quaternion.identity);
            Destroy(obj, 3f);
            yield return wait;
        }

        ConstructionFinish();
        isConstructing = !isConstructing;
            gameObject.SetActive(false);
    }
}
