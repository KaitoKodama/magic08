using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

[RequireComponent(typeof(Animator))]
public class VRRigController : MonoBehaviour
{
    [SerializeField] VRMap head = default;
    [SerializeField] VRMap leftHand = default;
    [SerializeField] VRMap rightHand = default;
    [SerializeField] Transform headConstraint = default;
    [SerializeField] Vector3 headBodyOffset = default;
    [SerializeField] float directionalAnimationSpeed = 5f;

    private readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    private readonly int DirectionYHash = Animator.StringToHash("DirectionY");

    private Animator animator;
    private Vector3 prevHeadPosition;


    //------------------------------------------
    // Unity�����^�C��
    //------------------------------------------
    private void Start()
    {
        animator = GetComponent<Animator>();

        headBodyOffset = transform.position - headConstraint.position;
        prevHeadPosition = head.vrTarget.position;
        IsAcceptRigUpdate = true;
    }
    private void FixedUpdate()
    {
        if (IsAcceptRigUpdate)
        {
            SetVRConstraint();
            SetDirectionalAnimation();
        }
    }


    //------------------------------------------
    // �O�����L�֐�
    //------------------------------------------
    public bool IsAcceptRigUpdate { private get; set; }


    //------------------------------------------
    // �������L�֐�
    //------------------------------------------
    private void SetVRConstraint()
    {
        transform.position = headConstraint.position + headBodyOffset;
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
    private void SetDirectionalAnimation()
    {
        float currentX = animator.GetFloat(DirectionXHash);
        float currentY = animator.GetFloat(DirectionYHash);

        var world = (head.vrTarget.position - prevHeadPosition) / Time.fixedDeltaTime;
        world.y = 0f;
        var local = transform.InverseTransformDirection(world);
        prevHeadPosition = head.vrTarget.position;

        float expectX = local.x;
        float expectY = local.z;
        float smoothX = Mathf.Clamp(Mathf.Lerp(currentX, expectX, Time.fixedDeltaTime * directionalAnimationSpeed), -1, 1);
        float smoothY = Mathf.Clamp(Mathf.Lerp(currentY, expectY, Time.fixedDeltaTime * directionalAnimationSpeed), -1, 1);

        animator.SetFloat(DirectionXHash, smoothX);
        animator.SetFloat(DirectionYHash, smoothY);
    }
}
