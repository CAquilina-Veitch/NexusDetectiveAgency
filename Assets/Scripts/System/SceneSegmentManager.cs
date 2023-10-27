using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSegmentManager : MonoBehaviour
{
    public List<int> StructureScenIds;
    public List<int> segmentSceneIds; // List of scene names for your segments
    public int segmentsToLoadAtOnce = 2 ;

    private int currentSegmentIndex = 0;

    private void LoadSegments()
    {
        for (int i = currentSegmentIndex; i < Mathf.Min(currentSegmentIndex + segmentsToLoadAtOnce, segmentSceneIds.Count); i++)
        {
            /*Debug.Log(i);
            string sceneName = segmentScenes[i].name;*/
            if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[0]).isLoaded)
            {
                SceneManager.LoadSceneAsync(segmentSceneIds[0], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadPastSegments()
    {/*
        for (int i = 0; i < currentSegmentIndex; i++)
        {
            string sceneName = segmentScenes[i].name;
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }*/
    }

    private void LoadStructure(int i)
    {
        //string sceneName = StructureScenes[i].name;
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            SceneManager.LoadScene(StructureScenIds[i], LoadSceneMode.Additive);
        }
    }
    void UnloadStructure(int i)
    {
        //string sceneName = StructureScenes[i].name;
        if (SceneManager.GetSceneByBuildIndex(StructureScenIds[i]).isLoaded)
        {
            SceneManager.UnloadSceneAsync(StructureScenIds[i]);
        }
    }

    public void LoadNextSegment()
    {
        currentSegmentIndex++;
        LoadSegments();
        UnloadPastSegments();
    }
    public void loadSegment(int which)
    {
        currentSegmentIndex = which;
        LoadSegments();
        UnloadPastSegments();
    }

    public void StartGame()
    {
        LoadStructure(1);
        UnloadStructure(0);
        UnloadStructure(3);
        loadSegment(0);
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
        loadSegment(0);
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
