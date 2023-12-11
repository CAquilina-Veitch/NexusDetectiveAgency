using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Format { Terminal, Handwritten, Newspaper, File}
[Serializable]
public class LoreItem
{
    public bool collected;

    public Format format;

    public string label;
    [TextArea(1, 100)] public string title;
    [TextArea(1, 100)] public string[] subtitle;


    public Sprite[] images;

    [TextArea(1, 100)] public string bodyText;

    public int leather;
    public Dimension reality;


}

public class LoreBookSection
{
    public int id;
    public List<int> loreItems;
    public List<Image> tabs;
}
public class LoreInventory : MonoBehaviour
{
    public List<LoreBookSection> LoreBookSections = new List<LoreBookSection>();
    public List<LoreItem> allLore;

    public List<Image> invTabs;

    public int currentTab = -1;
    public float fadeDuration = 0.2f;

    public Color[] tabColours = new Color[2];

    public List<LoreGroup> displays = new List<LoreGroup>();

    public bool singleIsOpen;
    public bool invOpen;

    [Header("Info")]

    [SerializeField] CanvasGroup folder;
    [SerializeField] GameObject crosshair;

    [SerializeField] CanvasGroup inventoryCanvasGroup;


    [SerializeField] CanvasGroup leatherCanvasGroup;
    [SerializeField] CanvasGroup realityCanvasGroup;
    [SerializeField] CanvasGroup loreCanvasGroup;




    DetectiveAgencyPlayer detective;
    PlayerController player;
    bool isDetective;

    public void Set(DetectiveAgencyPlayer p)
    {
        isDetective = true;
        detective = p;
    }
    public void Set(PlayerController p)
    {
        isDetective = false;
        player = p;
    }


    public int collectedLore()
    {
        int total = 0;
        for(int i=0; i<allLore.Count; i++)
        {
            if (allLore[i].collected)
            {
                if ((i>=3&&i<=8) || (i>=14&&i<=17))
                {
                    total++;
                }
            }
        }
        return total;
    }

    public bool isEnough()
    {
        return (collectedLore() >= 7 || allLore[7].collected);
    }


    public int openingLeather;
    public Dimension openingDimension;
    public void ChooseLeather(int i)
    {
        Debug.Log(i);
        openingLeather = i;
        StartCoroutine(CloseCanvasGroup(leatherCanvasGroup));



        if (i < 2)
        {
            StartCoroutine(OpenCanvasGroup(realityCanvasGroup));
        }
        else
        {
            openingDimension = Dimension.Noir;
            changeTabTitles();
            StartCoroutine(OpenCanvasGroup(loreCanvasGroup));
        }


    }

    public void ChooseReality(int d)
    {
        Debug.LogWarning(d);
        openingDimension = (Dimension)d;
        changeTabTitles();
        StartCoroutine(CloseCanvasGroup(realityCanvasGroup));
        StartCoroutine(OpenCanvasGroup(loreCanvasGroup));
    }

    public void TabPressed(int e)
    {
        if (e < loreIDs.Count)
        {
            SwitchCurrentLore(loreIDs[e]);
        }
    }


    List<int> loreIDs = new List<int>();
    public void changeTabTitles()
    {
        loreIDs = new List<int>();

        for (int i = 0; i<allLore.Count; i++)
        {
            if (allLore[i].leather == openingLeather && allLore[i].reality == openingDimension)
            {
                loreIDs.Add(i);
            }
        }

        for (int i = 0; i < invTabs.Count; i++)
        {
            if (i >= loreIDs.Count)
            {
                invTabs[i].gameObject.SetActive(false);
            }
            else
            {
                invTabs[i].gameObject.SetActive(true);
                invTabs[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = allLore[loreIDs[i]].label;
            }
            
        }
    }



    IEnumerator OpenCanvasGroup(CanvasGroup cnvsG)
    {
        float timer = 0;
        float from = cnvsG.alpha;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, 1, timer / fadeDuration);
            cnvsG.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        cnvsG.blocksRaycasts = true;
        cnvsG.interactable = true;
        cnvsG.alpha = 1;
    }    
    IEnumerator CloseCanvasGroup(CanvasGroup cnvsG)
    {
        cnvsG.blocksRaycasts = false;
        cnvsG.interactable = false;
        float timer = 0;
        float from = cnvsG.alpha;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(from,0, timer / fadeDuration);
            cnvsG.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }
        cnvsG.alpha = 0;

    }













    public bool InteractWithObject(LoreObject lO)
    {
        if (!allLore[lO.id].collected)
        {
            allLore[lO.id].collected = true;
        }
        singleIsOpen = !singleIsOpen;
        StartCoroutine(showSingleCanvas(lO.id, singleIsOpen));
        return singleIsOpen;


    }


    void ToggleControls(bool on)
    {
        if (isDetective)
        {
            detective.ShowMouse(!on);
            detective.EnableControls(on);
        }
        else
        {
            player.ShowMouse(!on);
            player.EnableControls(on);
        }
    }

    public void openInvCanvas()
    {
        if (!singleIsOpen)
        {
            invOpen = !invOpen;
            ToggleControls(!invOpen);
            if (invOpen)
            {
                StartCoroutine(OpenCanvasGroup(leatherCanvasGroup));
                
            }
            else
            {
                closeAll();
            }
            
        }
        
    }

    public void closeAll()
    {
        closeCurrent();
        StartCoroutine(CloseCanvasGroup(loreCanvasGroup));
        StartCoroutine(CloseCanvasGroup(realityCanvasGroup));
        StartCoroutine(CloseCanvasGroup(leatherCanvasGroup));
    }
    public void closeCurrent()
    {
        foreach (LoreGroup d in displays)
        {
            StartCoroutine(CloseCanvasGroup(d.cg));
        }
    }

    IEnumerator startInvCanvas(bool to)
    {
        

        if (isDetective)
        {
            detective.ShowMouse(to);
            detective.EnableControls(to);
        }
        else
        {
            player.ShowMouse(to);
            player.EnableControls(!to);
        }



        if (!to)
        {
            currentTab = -1;
            StartCoroutine(showSingleCanvas(0,to));
        }
        invOpen = to;
        float timer = 0;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(to ? 0 : 1, to ? 1 : 0, timer / fadeDuration);
            folder.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }
        folder.alpha = to ? 1 : 0;

    }

    public void SwitchCurrentLore(int to)
    {
        if(currentTab == to)
        {
            return;
        }
        Debug.LogError(to);
        int from = currentTab;
        currentTab = to;
        StartCoroutine(switchAnimation(from, to));
        
    }

    public void switchInfo(int to)
    {
        if (to < 0)
        {
            return;
        }
        LoreItem item = allLore[to];
        if(item != null)
        {
            displays[(int)item.format].bodyText.text = item.bodyText;
            displays[(int)item.format].title.text = item.title;
            if(item.images.Length>0) 
            {
                displays[(int)item.format].img.sprite = item.images[0];
            }
            if(item.subtitle.Length>0)
            {
                displays[(int)item.format].subtitle.text = item.subtitle[0];
            }
        }
    }


    IEnumerator showSingleCanvas(int id, bool show)
    {
        LoreItem lI = allLore[id];
        CanvasGroup infoPage = displays[(int)lI.format].cg;
        
        
        switchInfo(id);


        float timer = 0;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(show ? 0 : 1, show ? 1 : 0, timer / fadeDuration);
            infoPage.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }
        infoPage.alpha = show ? 1 : 0;
    }



    IEnumerator switchAnimation(int from, int to)
    {
        Debug.Log("to");
        float timer = 0;
        if (from >= 0)
        {
            LoreItem lIf = allLore[from];
            CanvasGroup infoPagef = displays[(int)lIf.format].cg;


            while (timer < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                infoPagef.alpha = alpha;
                timer += Time.deltaTime;
                yield return null;
            }
            infoPagef.alpha = 0;
        }


        


        switchInfo(to);

        LoreItem lIt = allLore[to];
        CanvasGroup infoPaget = displays[(int)lIt.format].cg;








        // Fade in
        timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            infoPaget.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        infoPaget.alpha = 1;


    }



}
