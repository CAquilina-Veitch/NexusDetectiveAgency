using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelStartTrigger : MonoBehaviour
{
    public bool loadMidScene;
    public UnityEvent enterActions;
    private void OnTriggerEnter(Collider other)
    {
        if(loadMidScene)
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().LoadNextMidSegment();
        }
        else
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().LoadNextSegment();

        }
        enterActions.Invoke();
        Destroy(gameObject);
    }
}
