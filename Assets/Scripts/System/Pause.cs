using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    [SerializeField] UnityEvent PauseMenu;

    public void Unpause()
    {
        Time.timeScale = 1;
    }
private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.Invoke();
            Time.timeScale = 0.000000000001f;
        }
    }
}
