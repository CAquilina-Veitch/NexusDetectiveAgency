using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Format { Terminal, Handwritten, Newspaper, File}

public class LoreItem
{
    public int id;
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

    public List<LoreItem> collectedLore;



    






}
