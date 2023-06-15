using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StageManager
{
    public StageData stageData;

    public IEnumerator LoadScene(int nextScene, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            Debug.Log("¿¡º£º£ ·Îµù¾ÈµÊ");
            yield return null;
        }

        Debug.Log("¿À ´Ù µÊ");
        op.allowSceneActivation = true;
    }
}
