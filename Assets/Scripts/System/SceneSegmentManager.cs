using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using FMOD.Studio;

public class SceneSegmentManager : MonoBehaviour
{
    public List<int> StructureSceneIds;
    public List<int> segmentSceneIds; // List of scene names for your segments
    public List<int> midSegmentSceneIds; // List of scene names for your segments

    int currentSegmentIndex = 0;
    int currentMidSegmentIndex = 0;


    IEnumerator LoadNextA(int from, int to)
    {
        if(from >= 0)
        {
            AsyncOperation ao = SceneManager.UnloadSceneAsync(from);
            yield return ao;
        }
        AsyncOperation ae = SceneManager.LoadSceneAsync(to,LoadSceneMode.Additive);
        yield return ae;
    }


    AsyncOperation LoadSegmentA(int i)
    {
        if (i < segmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                StartCoroutine(gg(segmentSceneIds[i]));
                /*AsyncOperation aO = SceneManager.LoadSceneAsync(segmentSceneIds[i], LoadSceneMode.Additive);
                return aO;*/
            }
        }
        return null;
    }


    AsyncOperation UnloadSegmentA(int i)
    {
        Debug.LogWarning($"unloading {i}, which is {segmentSceneIds[i]}");
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                AsyncOperation aO = SceneManager.UnloadSceneAsync(segmentSceneIds[i]);
                return aO;
            }
        }
        return null;
    }

    AsyncOperation LoadMidSegmentA(int i)
    {
        if (i < midSegmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                AsyncOperation aO = SceneManager.LoadSceneAsync(midSegmentSceneIds[i], LoadSceneMode.Additive);
                return aO;
            }
        }
        return null;
    }

    AsyncOperation UnloadMidSegmentA(int i)
    {
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                AsyncOperation aO = SceneManager.UnloadSceneAsync(midSegmentSceneIds[i]);
                return aO;
            }
        }
        return null;
    }

    AsyncOperation LoadStructureA(int i)
    {
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            AsyncOperation aO = SceneManager.LoadSceneAsync(StructureSceneIds[i], LoadSceneMode.Additive);
            return aO;
        }
        return null;
    }

    AsyncOperation UnloadStructureA(int i)
    {
        if (SceneManager.GetSceneByBuildIndex(StructureSceneIds[i]).isLoaded)
        {
            AsyncOperation aO = SceneManager.UnloadSceneAsync(StructureSceneIds[i]);
            return aO;
        }
        return null;
    }


    public void LoadNextSegmentA()
    {
        currentSegmentIndex++;
        StartCoroutine(LoadNextA(segmentSceneIds[currentSegmentIndex - 1], segmentSceneIds[currentSegmentIndex]));
    }
    public void LoadNextMidSegmentA()
    {
        currentMidSegmentIndex++;
        StartCoroutine(LoadNextA(midSegmentSceneIds[currentMidSegmentIndex - 1], midSegmentSceneIds[currentMidSegmentIndex]));
    }
    public void JumpLoadMidSegment(int which)
    {
        currentMidSegmentIndex = which;
        LoadMidSegmentA(which);
    }
    public void JumpLoadSegment(int which)
    {
        currentSegmentIndex = which;
        LoadSegmentA(which);
    }

    public void Awake()
    {
        LoadStructureA(0);
    }

    public void DetectiveAgency()
    {
        UnloadStructureA(0);     //menu
        LoadStructureA(2);       //ui
        LoadStructureA(3);       //det start
    }
    public void FinalDetectiveAgency()
    {
        StartCoroutine(FinalDet());
    }
    IEnumerator FinalDet()
    {
        yield return UnloadStructureA(1);
        yield return LoadStructureA(4);
        yield return UnloadMidSegmentA(currentMidSegmentIndex);
        yield return UnloadMidSegmentA(currentMidSegmentIndex);
    }
    public void PlaytestDemoStart()
    {
        StartCoroutine(FirstLevel());
    }
    IEnumerator FirstLevel()
    {
        //player
        yield return UnloadStructureA(3);            //det start
        

        var ids = new List<int>()
        {
            segmentSceneIds[0],midSegmentSceneIds[0],StructureSceneIds[1]
        };
        StartCoroutine(gg(ids));
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlaytestDemoStart();
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log(6);
            StartCoroutine(gg(segmentSceneIds[0]));
        }
    }
    IEnumerator gg(List<int> idString)
    {
        if (idString.Count >0)
        {
            yield break;
        }
        var asyncOp = SceneManager.LoadSceneAsync(idString[0], LoadSceneMode.Additive); //< Load the scene asynchronously
        asyncOp.allowSceneActivation = false; //< Deactivate the load of gameobjects on scene load
        if (asyncOp != null)
        {
            while (!asyncOp.isDone)
            {
                // Check if the progress is less than 0.9 (if it's less, it means that we load gameobjects)
                // Else, it means that we load something else.
                if (asyncOp.progress >= 0.9f && !asyncOp.allowSceneActivation)
                {
                    yield return null;
                    asyncOp.allowSceneActivation = true; //< Once everything is loaded, reactive this variable
                }
                else
                {
                    yield return null; //< We still wait until the scene load is finished
                }
            }
        }
        idString.RemoveAt(0);
        if (idString.Count > 0)
        {
            StartCoroutine(gg(idString));
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().Ready();
        }

    }


    IEnumerator gg(int id)
    {
        var asyncOp = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive); //< Load the scene asynchronously
        asyncOp.allowSceneActivation = false; //< Deactivate the load of gameobjects on scene load
        if (asyncOp != null)
        {
            while (!asyncOp.isDone)
            {
                // Check if the progress is less than 0.9 (if it's less, it means that we load gameobjects)
                // Else, it means that we load something else.
                if (asyncOp.progress >= 0.9f && !asyncOp.allowSceneActivation)
                {
                    yield return null;
                    asyncOp.allowSceneActivation = true; //< Once everything is loaded, reactive this variable
                }
                else
                {
                    yield return null; //< We still wait until the scene load is finished
                }
            }
        }
    }

}
