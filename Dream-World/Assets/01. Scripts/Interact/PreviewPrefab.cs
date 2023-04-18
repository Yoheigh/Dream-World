using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPrefab : Constructure
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


    protected void Construct()
    {

    }

    protected void ConstructionFinish()
    {

    }

    private IEnumerator ConstructEffect()
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

    }
}
