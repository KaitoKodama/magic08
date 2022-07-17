using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimator : MonoBehaviour
{
    private Animator animator;

    private readonly int GripLeftHash = Animator.StringToHash("GripLeft");
    private readonly int GripRightHash = Animator.StringToHash("GripRight");
    private readonly int TriggerLeftHash = Animator.StringToHash("TriggerLeft");
    private readonly int TriggerRightHash = Animator.StringToHash("TriggerRight");

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float triggerL = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        float indexL = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        float triggerR = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        float indexR = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        animator.SetFloat(GripRightHash, triggerR);
        animator.SetFloat(TriggerRightHash, indexR);
        animator.SetFloat(GripLeftHash, triggerL);
        animator.SetFloat(TriggerLeftHash, indexL);
    }
}
