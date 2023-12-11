using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using FMOD.Studio;
using System.Linq;
using System.ComponentModel;

public class SceneSegmentManager : MonoBehaviour
{
    public List<int> StructureSceneIds;
    public List<int> segmentSceneIds; // List of scene names for your segments
    public List<int> midSegmentSceneIds; // List of scene names for your segments

    int currentSegmentIndex = 0;
    int currentMidSegmentIndex = 0;

    public CanvasGroup loadingScreen;

    public swirl swirler;
    public Fadeblack fader;
    void LoadNextAB(int from, int to)
    {
        int[] ids = { -from, to };
        if (from < 0)
        {
            ids = new int[1] { to };
        }
        StartCoroutine(gg(ids.ToList()));
    }

    public void LoadNextSegment()
    {
        int from = currentSegmentIndex;
        currentSegmentIndex += 1;
        int[] ids = { -segmentSceneIds[from], segmentSceneIds[currentSegmentIndex] };

        StartCoroutine(gg(ids.ToList()));



    }    
    public void LoadNextMidSegment()
    {
        int from = currentMidSegmentIndex;
        currentMidSegmentIndex += 1;
        int[] ids = { -midSegmentSceneIds[from], midSegmentSceneIds[currentMidSegmentIndex] };

        StartCoroutine(gg(ids.ToList()));



    }



    void LoadSegmentA(int i)
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
    }


    void UnloadSegmentA(int i)
    {
        Debug.LogWarning($"unloading {i}, which is {segmentSceneIds[i]}");
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(segmentSceneIds[i]).isLoaded)
            {
                SceneManager.UnloadSceneAsync(segmentSceneIds[i]);
            }
        }
    }

    void LoadMidSegmentA(int i)
    {
        if (i < midSegmentSceneIds.Count)
        {
            if (!SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                SceneManager.LoadSceneAsync(midSegmentSceneIds[i], LoadSceneMode.Additive);
            }
        }
    }

    void UnloadMidSegmentA(int i)
    {
        if (i >= 0)
        {
            if (SceneManager.GetSceneByBuildIndex(midSegmentSceneIds[i]).isLoaded)
            {
                SceneManager.UnloadSceneAsync(midSegmentSceneIds[i]);
            }
        }
    }

    void LoadStructureA(int i)
    {
        if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
        {
            SceneManager.LoadSceneAsync(StructureSceneIds[i], LoadSceneMode.Additive);
        }
    }

    void UnloadStructureA(int i)
    {
        if (SceneManager.GetSceneByBuildIndex(StructureSceneIds[i]).isLoaded)
        {
            SceneManager.UnloadSceneAsync(StructureSceneIds[i]);
        }
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
        int[] ids =
{
            -StructureSceneIds[1],
            StructureSceneIds[4],
            -midSegmentSceneIds[currentMidSegmentIndex],
            -segmentSceneIds[currentSegmentIndex]
        };
        StartCoroutine(gg(ids.ToList()));
    }

    public void PlaytestDemoStart()
    {
        int[] ids =
{
            StructureSceneIds[1],
            -StructureSceneIds[3],
            segmentSceneIds[0],
            midSegmentSceneIds[0],
        };
        StartCoroutine(PauseForSwirler(ids.ToList()));

    }
    IEnumerator PauseForSwirler(List<int> ids)
    {
        swirler.Swirl();
        yield return new WaitForSeconds(1);
        fader.Fade(true, 0.5f);
        yield return new WaitForSeconds(0.5f);
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
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log(7);
            //LoadNextSegmentA();
        }
    }
    IEnumerator gg(List<int> idString)
    {
        Debug.LogWarning("List");
        if (idString.Count <=0)
        {
            yield break;
        }
        int id = idString[0];
        Debug.LogWarning(id);
        idString.RemoveAt(0);
        if (id > 0)
        {
            loadingScreen.alpha = 1;
            yield return null;
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
                        /*var objects = GameObject.FindObjectsOfType<GameObject>(); //< Get a list of gameObjects in the scene
                        for (int i = 0; i < objects.Length; i++)
                        {
                            var obj = objects[i]; //< Get the object
                            if (obj.scene.buildIndex == id) //< Check if the object is in the new scene
                            {
                                obj.SendMessage("Start", SendMessageOptions.DontRequireReceiver); //< Send it the Start Message
                                obj.SendMessage("Awake", SendMessageOptions.DontRequireReceiver); //< Send it the Awake Message
                            }

                            //LoadingScreenUpdate(); //< Update your loading screen animation here

                            yield return null; //< Do a little pause to have a smooth animation in the loading screen without freeze
                        }*/
                        yield return null;
                    }
                    else
                    {
                        // we are loading the rest of the scene
                        //LoadingScreenUpdate(); //< Update your loading screen animation here
                        Debug.Log(asyncOp.progress);
                        yield return null; //< We still wait until the scene load is finished
                                           //< Once everything is loaded, reactive this variable
                        asyncOp.allowSceneActivation = true;
                    }
                }
            }
            // We loaded the scene without any freezing!
            loadingScreen.alpha = 0;
        }
        else
        {
            Debug.LogWarning("meow" + id);
            var asyncOp = SceneManager.UnloadSceneAsync(-id); //< Load the scene asynchronously
            asyncOp.allowSceneActivation = false;
            if (asyncOp != null)
            {
                while (!asyncOp.isDone)
                {
                    yield return null;
                    if (asyncOp.progress >= 0.9f && !asyncOp.allowSceneActivation)
                    {
                        asyncOp.allowSceneActivation = true;
                    }
                }
            }
        }

        
        if (idString.Count > 0)
        {
            StartCoroutine(gg(idString));
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().Ready();
            fader.Fade(false, 0.2f);

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
                    var objects = GameObject.FindObjectsOfType<GameObject>(); //< Get a list of gameObjects in the scene
                    for (int i = 0; i < objects.Length; i++)
                    {
                        var obj = objects[i]; //< Get the object
                        if (obj.scene.buildIndex == id) //< Check if the object is in the new scene
                        {
                            obj.SendMessage("Start", SendMessageOptions.DontRequireReceiver); //< Send it the Start Message
                            obj.SendMessage("Awake", SendMessageOptions.DontRequireReceiver); //< Send it the Awake Message
                        }

                        //LoadingScreenUpdate(); //< Update your loading screen animation here

                        yield return null; //< Do a little pause to have a smooth animation in the loading screen without freeze
                    }
                    asyncOp.allowSceneActivation = true; //< Once everything is loaded, reactive this variable
                }
                else
                {
                    // we are loading the rest of the scene
                    //LoadingScreenUpdate(); //< Update your loading screen animation here

                    yield return null; //< We still wait until the scene load is finished
                }
            }
        }
        // We loaded the scene without any freezing!
    }

}
