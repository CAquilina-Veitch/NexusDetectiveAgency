using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoreGroup : MonoBehaviour
{
    public CanvasGroup cg;
    public TextMeshProUGUI title;
    public TextMeshProUGUI bodyText;
    public Image img;
    public TextMeshProUGUI subtitle;

    public void ChangeLore( LoreItem lO)
    {
        title.text = lO.title;
        bodyText.text = lO.bodyText;
        img.sprite= lO.images[0];
        subtitle.text = lO.subtitle[0];
    }

}
