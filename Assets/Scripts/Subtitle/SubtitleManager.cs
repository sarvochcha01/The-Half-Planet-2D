using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitleText;

    [SerializeField] private List<string> subtitleList = new List<string>();

    [SerializeField] private bool isSubtitleListEmpty;
    [SerializeField] private bool isSubtitlePlaying;

    [SerializeField] private float subtitleTime;
    [SerializeField] private float timer;

    void Update()
    {
        if (subtitleList.Count <= 0) isSubtitleListEmpty = true;
        else isSubtitleListEmpty = false;

        if (isSubtitlePlaying)
        {
            timer += Time.deltaTime;

            if (timer >= subtitleTime)
            {
                subtitleText.text = "";
                isSubtitlePlaying = false;
                timer = 0f;
            }
        }

        if (!isSubtitleListEmpty && !isSubtitlePlaying)
        {
            DisplaySubtitle(subtitleList[0]);
            subtitleList.RemoveAt(0);
        }
    }

    public void DisplaySubtitle(string text)
    {
        if (!isSubtitlePlaying)
        {
            subtitleText.text = text;
            isSubtitlePlaying = true;
        }
        else
        {
            subtitleList.Add(text);
        }
    }
}
