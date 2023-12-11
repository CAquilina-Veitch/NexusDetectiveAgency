using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    [SerializeField] UnityEvent PauseMenu;

    bool mouseWasOn;
    bool paused;

    public void Unpause()
    {
        Debug.LogWarning(111221);
        Time.timeScale = 1;
        Cursor.lockState = mouseWasOn ? CursorLockMode.Locked : CursorLockMode.None;
        Debug.Log(Time.timeScale);
    }



private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
            {
                paused = true;
                mouseWasOn = Cursor.lockState == CursorLockMode.None;
                Cursor.lockState = CursorLockMode.None;
                PauseMenu.Invoke();
                Time.timeScale = 0.000000000001f;
                Debug.Log(Time.timeScale);
            }
        }
    }
}
