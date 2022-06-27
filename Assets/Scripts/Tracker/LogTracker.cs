using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogTracker : MonoBehaviour
{
    [SerializeField] Tracker tracker = default;
    [SerializeField] Text logText = default;
    private float time = 0f;
    private float elapseTime = 5f;
    private bool isShowing = false;


    private void Update()
    {
        tracker.UpdatePosition();
        tracker.UpdateRotation();

        if (isShowing)
        {
            time += Time.deltaTime;
            if (time >= elapseTime)
            {
                time = 0f;
                string empty = "";
                for (int i = 0; i < logText.text.Length; i++)
                    empty += " ";
                logText.DOText(empty, 1f, false, ScrambleMode.All);
                isShowing = false;
            }
        }
    }

    public void SetLogText(string text)
    {
        logText.text = "";
        tracker.ResetPosition();
        tracker.ResetRotation();

        logText.DOText(text, 1f, false, ScrambleMode.All);
        isShowing = true;
    }
}
