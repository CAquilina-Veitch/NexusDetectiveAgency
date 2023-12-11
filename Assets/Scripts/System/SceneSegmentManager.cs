using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSegmentManager : MonoBehaviour
{
    public List<int> StructureSceneIds;
    public List<int> segmentSceneIds; // List of scene names for your segments
    public List<int> midSegmentSceneIds; // List of scene names for your segments

    int currentSegmentIndex = 0;
    int currentMidSegmentIndex = 0;


    void LoadSegment(int i)
    {
        if(i < segmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                SceneManager.LoadScene(segmentSceneIds[i], LoadSceneMode.Additive);
            }
        }
    }    

    void UnloadSegment(int i)
    {
        Debug.LogWarning($"unloading {i}, which is {segmentSceneIds[i]}");
        if (i >= 0)
        {
            if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                SceneManager.UnloadSceneAsync(segmentSceneIds[i]);
            }
        }
    }
    
    void LoadMidSegment(int i)
    {
        if(i < midSegmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                SceneManager.LoadSceneAsync(midSegmentSceneIds[i], LoadSceneMode.Additive);
            }
        }
    }
    void UnloadMidSegment(int i)
    {
        if (i >= 0)
        {
            if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                SceneManager.UnloadSceneAsync(midSegmentSceneIds[i]);
            }
        }
    }

    void LoadStructure(int i)
    {
        //string sceneName = StructureScenes[i].name;
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            SceneManager.LoadSceneAsync(StructureSceneIds[i], LoadSceneMode.Additive);
        }
    }
    void UnloadStructure(int i)
    {
        //string sceneName = StructureScenes[i].name;
        if (SceneManager.GetSceneByBuildIndex(StructureSceneIds[i]).isLoaded)
        {
            SceneManager.UnloadSceneAsync(StructureSceneIds[i]);
        }
    }

    public void LoadNextSegment()
    {
        currentSegmentIndex++;
        UnloadSegment(currentSegmentIndex - 1);
        LoadSegment(currentSegmentIndex);
    }    
    public void LoadNextMidSegment()
    {
        currentMidSegmentIndex++;
        UnloadMidSegment(currentMidSegmentIndex - 1);
        LoadMidSegment(currentMidSegmentIndex);
    }
    public void JumpLoadMidSegment(int which)
    {
        currentMidSegmentIndex = which;
        LoadMidSegment(which);
    }
    public void JumpLoadSegment(int which)
    {
        currentSegmentIndex = which;
        LoadSegment(which);
    }

    public void StartGame()
    {
        Debug.LogError("StartCalled");
        /*
        LoadStructure(1);
        UnloadStructure(0);
        UnloadStructure(3);
        LoadSegment();
        LoadMidSegment();
        LoadStructure(2);*/
    }
    public void Awake()
    {
        LoadStructure(0);
    }

    public void DetectiveAgency()
    {
        UnloadStructure(0);     //menu
        LoadStructure(2);       //ui
        LoadStructure(3);       //det start
    }
    public void FinalDetectiveAgency()
    {
        LoadStructure(4);
        UnloadStructure(1);
        UnloadMidSegment(currentMidSegmentIndex);
        UnloadSegment(currentSegmentIndex);
    }
    public void PlaytestDemoStart()
    {
        LoadStructure(1);       //player
        UnloadStructure(3);     //det start
        //UnloadStructure(3);     
        LoadSegment(0);
        LoadMidSegment(0);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            PlaytestDemoStart();
        }
    }

}
