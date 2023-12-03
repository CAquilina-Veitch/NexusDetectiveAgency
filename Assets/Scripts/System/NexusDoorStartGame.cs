using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusDoorStartGame : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().PlaytestDemoStart();
    }
}
