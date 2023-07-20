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
            yield return null;
        }

        Debug.Log("¿À ´Ù µÊ");
        yield return new WaitForSeconds(3f);
        op.allowSceneActivation = true;
    }
}
