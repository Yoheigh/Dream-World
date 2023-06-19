using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject ConstructurePrefab;

    [SerializeField]
    private GameObject BuildVFX;

    [SerializeField]
    private Material previewMaterial;

    // 내부 변수
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

    protected void Construct()
    {
        StartCoroutine(ConstructWithEffect());
    }

    protected void ConstructionFinish()
    {
        Instantiate(ConstructurePrefab, transform.position, transform.rotation);
    }

    private IEnumerator ConstructWithEffect()
    {
        var wait = new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            float z = Random.Range(-0.5f, 0.5f);
            GameObject obj = Instantiate(BuildVFX, new Vector3(x, y, z), Quaternion.identity);
            Destroy(obj, 4f);
            yield return wait;
        }

        ConstructionFinish();
        Destroy(gameObject);
    }
}
