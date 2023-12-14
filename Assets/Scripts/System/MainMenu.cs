using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    bool started = false;
    public void StartGameButton()
    {
        if (!started)
        {
            started = true;
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().DetectiveAgency();
        }
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }


}
