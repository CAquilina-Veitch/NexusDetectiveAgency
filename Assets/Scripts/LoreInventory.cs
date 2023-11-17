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
    public string title;
    public string[] subtitle;


    public Sprite[] images;
    public string bodyText;


}


public class LoreInventory : MonoBehaviour
{

    public List<LoreItem> allLore;

    public List<Image> invTabs;

    public int currentTab = -1;
    public float fadeDuration = 0.2f;

    bool inventoryOpen;

    [Header("Info")]
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup info;
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI bodyText;


    public void InteractWithObject(LoreObject lO)
    {
        if (!allLore[lO.id].collected)
        {
            allLore[lO.id].collected = true;
            UpdateLoreItems();
        }
    }

    public void UpdateLoreItems()
    {
        for(int i = 0; i < invTabs.Count; i++)
        {
            bool isCollected = allLore[i].collected;

            Debug.Log($"{invTabs.Count}, {i}");
            Debug.Log($"{invTabs.Count}, {i}, {invTabs[i]}");
            Debug.Log($"{invTabs[i].GetComponent<Button>()}");


            invTabs[i].GetComponent<Button>().enabled = isCollected;

            if (i == currentTab && allLore[currentTab].collected)
            {

            }

            
        }


    }
    private void OnEnable()
    {
        //UpdateLoreItems();
    }
    private void Start()
    {
        for (int i = 0; i < invTabs.Count; i++)
        {
            invTabs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = allLore[i].label;
        }
    }
    public void SwitchCurrentLore(int to)
    {
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
            bodyText.text = item.bodyText;
            title.text = item.title;
            if(item.images.Length>0) 
            {
                img.sprite = item.images[0];
            }
        }
    }

    IEnumerator switchAnimation(int from, int to)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            info.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }
        info.alpha = 0;

        switchInfo(to);

        // Fade in
        timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            info.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        info.alpha = 1;



    }



}
