using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIK : MonoBehaviour
{
    [SerializeField] LayerMask ground = default;
    [SerializeField] Vector3 footOffset = default;
    [SerializeField] float footPosWeight = 1f;
    [SerializeField] float footRotWeight = 1f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        SetFootIK(AvatarIKGoal.RightFoot);
        SetFootIK(AvatarIKGoal.LeftFoot);
    }


    private void SetFootIK(AvatarIKGoal avatarIK)
    {
        Vector3 foot = animator.GetIKPosition(avatarIK);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(foot + Vector3.up, Vector3.down, out hit, float.MaxValue, ground);
        if (hasHit)
        {
            var plane = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            var footRot = Quaternion.LookRotation(plane, hit.normal);
            animator.SetIKPositionWeight(avatarIK, footPosWeight);
            animator.SetIKPosition(avatarIK, hit.point + footOffset);
            animator.SetIKRotationWeight(avatarIK, footRotWeight);
            animator.SetIKRotation(avatarIK, footRot);
        }
        else animator.SetIKPositionWeight(avatarIK, 0);
    }
}
