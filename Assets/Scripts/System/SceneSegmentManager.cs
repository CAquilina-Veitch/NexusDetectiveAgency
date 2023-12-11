using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneSegmentManager : MonoBehaviour
{
    public List<int> StructureSceneIds;
    public List<int> segmentSceneIds; // List of scene names for your segments
    public List<int> midSegmentSceneIds; // List of scene names for your segments

    int currentSegmentIndex = 0;
    int currentMidSegmentIndex = 0;


    async void LoadSegment(int i)
    {
        if (i < segmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(segmentSceneIds[i], LoadSceneMode.Additive);
                await WaitUntilSceneLoaded(asyncOperation);
            }
        }
    }

    async void UnloadSegment(int i)
    {
        Debug.LogWarning($"unloading {i}, which is {segmentSceneIds[i]}");
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(segmentSceneIds[i]);
                await WaitUntilSceneUnloaded(asyncOperation);
            }
        }
    }

    async void LoadMidSegment(int i)
    {
        if (i < midSegmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(midSegmentSceneIds[i], LoadSceneMode.Additive);
                await WaitUntilSceneLoaded(asyncOperation);
            }
        }
    }

    async void UnloadMidSegment(int i)
    {
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(midSegmentSceneIds[i]);
                await WaitUntilSceneUnloaded(asyncOperation);
            }
        }
    }

    async void LoadStructure(int i)
    {
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(StructureSceneIds[i], LoadSceneMode.Additive);
            await WaitUntilSceneLoaded(asyncOperation);
        }
    }

    async void UnloadStructure(int i)
    {
        if (SceneManager.GetSceneByBuildIndex(StructureSceneIds[i]).isLoaded)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(StructureSceneIds[i]);
            await WaitUntilSceneUnloaded(asyncOperation);
        }
    }

    async Task WaitUntilSceneLoaded(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            // Optionally, you can yield return null or another WaitForSeconds
            await Task.Yield();
        }
    }
    async Task WaitUntilSceneUnloaded(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            // Optionally, you can yield return null or another WaitForSeconds
            await Task.Yield();
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
