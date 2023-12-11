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
                AsyncOperation aO = SceneManager.LoadSceneAsync(segmentSceneIds[i], LoadSceneMode.Additive);
                return aO;
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
        yield return LoadSegmentA(0);
        yield return LoadMidSegmentA(0);
        LoadStructureA (1);             //player
        UnloadStructureA(3);            //det start
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            PlaytestDemoStart();
        }
    }

}
