using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    [SerializeField] UnityEvent PauseMenu;

    bool mouseWasOn;

    public void Unpause()
    {
        Time.timeScale = 1;
        Cursor.lockState = mouseWasOn ? CursorLockMode.Locked : CursorLockMode.None;
    }



private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseWasOn = Cursor.lockState == CursorLockMode.None;
            Cursor.lockState = CursorLockMode.None;
            PauseMenu.Invoke();
            Time.timeScale = 0.000000000001f;
        }
    }
}
