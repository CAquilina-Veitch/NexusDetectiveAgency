using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TestingLoadScene : MonoBehaviour
{
    public Object[] scenesToLoad;


    private void OnEnable()
    {
        foreach (var scene in scenesToLoad)
        {
            string sceneName = scene.name;
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
        Destroy(this);
    }
}
