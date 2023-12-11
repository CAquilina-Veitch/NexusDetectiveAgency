using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeblack : MonoBehaviour
{
    public Camera cam;
    public void Fade(bool f, float d)
    {
        StartCoroutine(fadeBlack(f, d));
    }    
    public void Fade(bool f, float d,bool b)
    {
        StartCoroutine(fadeBlack(f, d,b));
    }
    public CanvasGroup loadingScreen;
    IEnumerator fadeBlack(bool fade, float duration)
    {
        cam.enabled = true;
        float elapsedTime = 0f;


        float initialAlpha = loadingScreen.alpha;
        float goalAlpha = fade ? 1f : 0f;



        while (elapsedTime < duration)
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(initialAlpha, goalAlpha, elapsedTime / duration);

            // Update the CanvasGroup alpha
            loadingScreen.alpha = alpha;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
        if (!fade)
        {

            cam.enabled = false;
        }
        // Ensure that the final alpha is set to 1
        loadingScreen.alpha = goalAlpha;
    }IEnumerator fadeBlack(bool fade, float duration, bool b)
    {
        cam.enabled = true;
        float elapsedTime = 0f;


        float initialAlpha = loadingScreen.alpha;
        float goalAlpha = fade ? 1f : 0f;



        while (elapsedTime < duration)
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(initialAlpha, goalAlpha, elapsedTime / duration);

            // Update the CanvasGroup alpha
            loadingScreen.alpha = alpha;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure that the final alpha is set to 1
        loadingScreen.alpha = goalAlpha;
        camOff();
    }
    public void camOff()
    {
        cam.enabled = false;
    }
}
