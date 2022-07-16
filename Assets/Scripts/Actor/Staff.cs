using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Staff : MonoBehaviour
{
    [SerializeField] MagicSelecter magicSelecter = default;
    [SerializeField] StaffTracker staffTracker = default;

    [SerializeField] Transform tips = default;
    [SerializeField] Transform staffSelf = default;
    private Animator animator;

    private readonly int IsGrabHash = Animator.StringToHash("IsGrab");
    private readonly int IsShootHash = Animator.StringToHash("IsShoot");


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        OnGrabState(false);
    }


    public bool IsShootEnable { get { return magicSelecter.IsResisterExist; } }
    public void OnMagicShoot(bool enable)
    {
        if (enable)
        {
            animator.SetTrigger(IsShootHash);
            magicSelecter.RequestExcute(tips.forward);
        }
        else
        {
            magicSelecter.RequestSwapGridContent(tips);
        }
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
        staffTracker.SetStaffFade(isGrab);

        animator.SetBool(IsGrabHash, isGrab);
    }
}
