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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePos(Transform _blockPointer)
    {
        // 위치가 바뀔 때마다 반올림해서 변경
        // _blockPointer.position.GetXYZRound(out x, out y, out z);
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
