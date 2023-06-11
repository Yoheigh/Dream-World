using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationFlag : MonoBehaviour
{
    public List<DebugFlag> debugFlags = new List<DebugFlag>();

    public int currentStep = 0;

    private void Update()
    {
        
    }

    public void NextStep()
    {
        currentStep += 1;
    }

    public void StepComplete()
    {
        debugFlags[currentStep].flag = true;
        NextStep();
    }
}

public class DebugFlag
{
    public bool flag;
    public string description;
}
