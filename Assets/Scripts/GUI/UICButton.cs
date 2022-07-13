using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class UICButton : MonoBehaviour
{
    [SerializeField] float fillSpeed = 2f;
    [SerializeField, HideInInspector] string triggerTag = default;
    private Image image;
    private Action callback;
    private bool isEntry = false;


    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(triggerTag) && !isEntry && IsEnable)
        {
            if (callback != null)
            {
                if (image.fillAmount >= 1f)
                {
                    callback();
                    isEntry = true;
                }
                else
                {
                    image.fillAmount += fillSpeed * Time.deltaTime;
                }
            }
            else
            {
                Debug.Log("When Calback is null it do nothing");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            image.fillAmount = 0f;
            isEntry = false;
        }
    }


    public bool IsEnable { get; set; } = true;
    public void AddListener(Action callback)
    {
        this.callback = callback;
    }
}
