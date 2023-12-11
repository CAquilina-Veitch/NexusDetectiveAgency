using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusDoorStartGame : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().PlaytestDemoStart();
    }
    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
