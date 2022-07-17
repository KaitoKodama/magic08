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
    private Image image;
    private Action callback;
    private bool isEntry = false;


    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void OnTriggerStay(Collider other)
    {
        var hand = other.GetComponent<PlayerHand>();
        if (hand != null && !isEntry && IsEnable)
        {
            if (callback != null)
            {
                if (image.fillAmount >= 1f)
                {
                    isEntry = true;

                    callback();
                    PlayerInput.VivrateHand vivrate = (hand.HandType == HandType.LeftHand) ? PlayerInput.VivrateHand.Left : PlayerInput.VivrateHand.Right;
                    Locator<PlayerInput>.I.OnVivration(0.1f, vivrate);
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
        var hand = other.GetComponent<PlayerHand>();
        if (hand != null)
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
