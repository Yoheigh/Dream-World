using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    EffectiveType toolEffectiveType = EffectiveType.Axe;

    public GameObject Axe;
    public GameObject Pickaxe;
    public GameObject Shovel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeTool(EffectiveType _toolEffectiveType)
    {
        if (toolEffectiveType == _toolEffectiveType)
            return;

        toolEffectiveType = _toolEffectiveType;
        switch (toolEffectiveType)
        {
            case EffectiveType.Axe:
                Axe.SetActive(true);
                Pickaxe.SetActive(false);
                Shovel.SetActive(false);
                break;
            case EffectiveType.Pickaxe:
                Axe.SetActive(false);
                Pickaxe.SetActive(true);
                Shovel.SetActive(false);
                break;
            case EffectiveType.Shovel:
                Axe.SetActive(false);
                Pickaxe.SetActive(false);
                Shovel.SetActive(true);
                break;
        }
    }
}
