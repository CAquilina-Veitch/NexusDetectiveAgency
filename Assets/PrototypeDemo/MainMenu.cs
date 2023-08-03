using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGameButton()
    {
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().StartGame();
    }
}
