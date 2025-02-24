using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject FadePanel;
    // Start is called before the first frame update
    private void Awake()
    {
        FadePanel.SetActive(false);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        FadePanel.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = FadePanel.GetComponent<Image>().color;
            c.a = f;
            FadePanel.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        FadePanel.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        FadePanel.SetActive(true);
        for (float f = 0f; f < 1f; f += 0.02f)
        {
            Color c = FadePanel.GetComponent<Image>().color;
            c.a = f;
            FadePanel.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
