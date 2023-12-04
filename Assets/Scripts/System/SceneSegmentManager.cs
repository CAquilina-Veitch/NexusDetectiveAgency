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

    void LoadSegment()
    {
        if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[currentSegmentIndex]).isLoaded)
        {
            SceneManager.LoadSceneAsync(segmentSceneIds[currentSegmentIndex], LoadSceneMode.Additive);
        }
        if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[currentSegmentIndex - 1]).isLoaded)
        {
            SceneManager.UnloadSceneAsync(segmentSceneIds[currentSegmentIndex - 1]);
        }
    }    
    
    void LoadMidSegment()
    {
        if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[currentMidSegmentIndex]).isLoaded)
        {
            SceneManager.LoadSceneAsync(midSegmentSceneIds[currentMidSegmentIndex], LoadSceneMode.Additive);
        }
        if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[currentMidSegmentIndex - 1]).isLoaded)
        {
            SceneManager.UnloadSceneAsync(midSegmentSceneIds[currentMidSegmentIndex - 1]);
        }
    }

    void LoadStructure(int i)
    {
        //string sceneName = StructureScenes[i].name;
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            SceneManager.LoadScene(StructureSceneIds[i], LoadSceneMode.Additive);
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
        LoadSegment();
    }    
    public void LoadNextMidSegment()
    {
        currentMidSegmentIndex++;
        LoadMidSegment();
    }
    public void JumpLoadMidSegment(int which)
    {
        currentMidSegmentIndex = which;
        LoadMidSegment();
    }    public void JumpLoadSegment(int which)
    {
        currentSegmentIndex = which;
        LoadSegment();
    }

    public void StartGame()
    {
        LoadStructure(1);
        UnloadStructure(0);
        UnloadStructure(3);
        LoadSegment();
        LoadMidSegment();
        LoadStructure(2);
    }
    public void Awake()
    {
        LoadStructure(0);
    }

    public void DetectiveAgency()
    {
        UnloadStructure(0);
        LoadStructure(3);
    }
    public void PlaytestDemoStart()
    {
        LoadStructure(1);
        UnloadStructure(0);
        UnloadStructure(3);
        LoadSegment();
        LoadStructure(2);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            PlaytestDemoStart();
        }
    }

}
