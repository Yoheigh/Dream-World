using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneManager : MonoBehaviour
{
    public StageData stageData;

    private static SceneManager instance;
    public static SceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneManager>();
            }
            return instance;
        }
    }

    public IEnumerator LoadScene(int nextScene, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        op.allowSceneActivation = true;
    }

    public SerializableDictionary<string, BaseScene> sceneDictionary = new SerializableDictionary<string, BaseScene>();
    private string currentSceneName = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<SceneManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        if (currentSceneName == null)
        {
            currentSceneName = "Login";
        }
        else
        {
            currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        // RegisterScene(currentSceneName);
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public BaseScene GetSceneClass(string sceneName)
    {
        //if (sceneName == "Login") return gameObject.AddComponent<LoginScene>();
        //if (sceneName == "Lobby") return gameObject.AddComponent<LobbyScene>();

        return null;
    }

    public void RemoveSceneClass(string sceneName)
    {
        //if (sceneName == "Login") Destroy(GetComponent<LoginScene>());
        //if (sceneName == "Lobby") Destroy(GetComponent<LobbyScene>());
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public string GetCurrentSceneName()
    {
        return currentSceneName;
    }

    public void RegisterScene(string sceneName)
    {
        currentSceneName = sceneName;
        if (!sceneDictionary.ContainsKey(sceneName))
        {
            BaseScene tempScene = GetSceneClass(sceneName);
            tempScene.sceneName = sceneName;
            sceneDictionary.Add(sceneName, tempScene);
        }
    }

    public BaseScene GetScene(string sceneName)
    {
        if (sceneDictionary.ContainsKey(sceneName))
        {
            return sceneDictionary[sceneName];
        }
        return null;
    }

    public void UnloadScene(string sceneName)
    {
        if (sceneDictionary.ContainsKey(sceneName))
        {
            sceneDictionary.Remove(sceneName);
            RemoveSceneClass(sceneName);
        }
    }

    public IEnumerator LoadSceneButton(string sceneName)
    {
        UnloadScene(currentSceneName);
        yield return new WaitForSeconds(0.5f);
        LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        RegisterScene(currentSceneName);
    }
}
