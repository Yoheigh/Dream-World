using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{

    public StageManager Stage = new StageManager();
    public SoundManager SoundManager = new SoundManager();

    protected override void Awake2()
    {
        
    }
}
