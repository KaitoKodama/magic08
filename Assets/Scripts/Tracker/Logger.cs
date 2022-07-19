using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Logger : MonoBehaviour
{
    [SerializeField] Text logText = default;
    private float time = 0f;
    private float elapseTime = 5f;
    private bool isShowing = false;


    private void Update()
    {
        if (isShowing)
        {
            time += Time.deltaTime;
            if (time >= elapseTime)
            {
                time = 0f;
                isShowing = false;
                ResetLogText();
            }
        }
    }

    public void Log(string text)
    {
        logText.text = "";
        logText.DOText(text, 1f, false, ScrambleMode.All);
        isShowing = true;
    }

    private void ResetLogText()
    {
        string empty = "";
        for (int i = 0; i < logText.text.Length; i++)
        {
            empty += " ";
        }
        logText.DOText(empty, 1f, false, ScrambleMode.All);
    }
}
