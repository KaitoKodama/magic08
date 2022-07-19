using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Staff : MonoBehaviour
{
    [SerializeField] Transform tips = default;
    [SerializeField] Transform staffSelf = default;
    private Animator animator;

    private readonly int IsGrabHash = Animator.StringToHash("IsGrab");
    private readonly int IsShootHash = Animator.StringToHash("IsShoot");


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        OnGrabState(false);
    }


    public Transform Tips => tips;

    public void OnShootAnimation()
    {
        animator.SetTrigger(IsShootHash);
    }
    public void OnGrabState(bool isGrab, Transform hand = null)
    {
        staffSelf.parent = hand;
        if (hand != null)
        {
            staffSelf.localPosition = Vector3.zero;
            staffSelf.localRotation = Quaternion.Euler(Vector3.zero);
        }
        var scale = isGrab ? Vector3.one : Vector3.zero;
        staffSelf.DOScale(scale, 0.2f);
        animator.SetBool(IsGrabHash, isGrab);
    }
}
