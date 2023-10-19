using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

[Serializable]
public class UIElement
{
    public string name;
    public List<GameObject> objects;
    public bool isActive;
    public UnityEngine.Object manager;
}

public class UIManager : MonoBehaviour
{
    public List<UIElement> elements;

    public void ToggleUI(string name, bool to)
    {
        UIElement foundElement = elements.Find(element => element.name == name);

        if (foundElement != null)
        {
            foreach (GameObject uiObject in foundElement.objects)
            {
                uiObject.SetActive(to);
            }
        }
        else
        {
            Debug.LogWarning("UIElement with name " + name + " not found.");
        }
    }
}
