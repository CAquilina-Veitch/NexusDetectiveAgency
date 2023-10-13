using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteInteractionUI : MonoBehaviour
{
    PlayerController controller;
    List<Button> collectedButtons = new List<Button>();

    [SerializeField] GameObject segmentPrefab;

    List<GameObject> children;

    private void OnEnable()
    {
        
        UpdateUI();




    }

    public void UpdateUI()
    {
        collectedButtons = controller.CollectedButtons;

        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        for(int i = 0; i < collectedButtons.Count; i++)
        {
            float angle = 360 / collectedButtons.Count;

            GameObject temp = Instantiate(segmentPrefab, transform);
            children.Add(temp);

            temp.GetComponent<Image>().fillAmount = (i + 1) * angle;
            temp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if(collectedButtons.Count > 1)
            {
                temp.GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
            else
            {

            }

        }




    }


}
