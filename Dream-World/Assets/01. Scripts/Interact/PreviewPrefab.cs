using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPrefab : MonoBehaviour
{
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    }
}
