using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteInteractionUI : MonoBehaviour
{
    PlayerController controller;
    List<TriggerableObject> CollectedActions = new List<TriggerableObject>();

    [SerializeField] GameObject segmentPrefab;

    List<GameObject> children = new List<GameObject>();

    float pixelScale = 200;



    private void OnEnable()
    {
        controller = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        UpdateUI();




    }

    public void UpdateUI()
    {
        CollectedActions = controller.CollectedActions;

        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        for(int i = 0; i < CollectedActions.Count; i++)
        {
            float angle = 360 / CollectedActions.Count;

            GameObject temp = Instantiate(segmentPrefab, transform);
            children.Add(temp);

            temp.GetComponent<Image>().fillAmount =  angle/360;
            temp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (i + 1) * angle));
            temp.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            Debug.Log(temp.transform.rotation.ToString());

            if(CollectedActions.Count > 1)
            {
                temp.transform.GetChild(0).transform.localPosition = new Vector2(pixelScale * Mathf.Sin((2 *angle * Mathf.Deg2Rad)), pixelScale * Mathf.Cos((2*angle * Mathf.Deg2Rad)));
                Debug.Log($"{pixelScale * Mathf.Sin((angle * Mathf.Deg2Rad))}, { pixelScale * Mathf.Cos(( angle * Mathf.Deg2Rad))}");
                Debug.Log($"{pixelScale} * Mathf.Sin(( {angle} * {Mathf.Deg2Rad})), {pixelScale} * Mathf.Cos(({angle} * {Mathf.Deg2Rad})))");
                Debug.Log($"{pixelScale} * {Mathf.Sin((angle * Mathf.Deg2Rad))}, {pixelScale} * {Mathf.Cos((angle * Mathf.Deg2Rad))}");

            }
            else
            {

            }

        }




    }

    



}
