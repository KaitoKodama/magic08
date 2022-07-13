using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using CMN;

public class ActorInput : MonoBehaviour
{
    [SerializeField] Transform staffLeftAncher = default;
    [SerializeField] Transform staffRightAncher = default;
    [SerializeField] StaffHoldingHand staffHoldingHand = default;

    private Transform staffHandAncher;
    private OVRInput.Controller staffHoldController;
    private OVRInput.Controller staffHoldInverseController;


    private void Start()
    {
        Locator<ActorInput>.Bind(this);
        SetStaffHoldingHand(staffHoldingHand);
    }
    private void OnDestroy()
    {
        Locator<ActorInput>.Unbind(this);
    }


    public Transform StaffHoldAncher => staffHandAncher;
    public bool IsTrigger(bool inverse = false)
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, GetController(inverse));
    }
    public bool IsIndex(bool inverse = false)
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, GetController(inverse));
    }
    public bool IsStart(bool inverse = false)
    {
        return OVRInput.GetDown(OVRInput.Button.Start, GetController(inverse));
    }
    public bool IsThumb(bool inverse = false)
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, GetController(inverse));
    }
    public float GetTrigger(bool inverse = false)
    {
        return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, GetController(inverse));
    }
    public float GetIndex(bool inverse = false)
    {
        return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, GetController(inverse));
    }
    
    public void SetStaffHoldingHand(StaffHoldingHand expectHand)
    {
        staffHoldingHand = expectHand;
        var staffAncher = (staffHoldingHand == StaffHoldingHand.LeftHand) ? staffLeftAncher : staffRightAncher;
        staffHandAncher = staffAncher;

        staffHoldController = Utility.GetEnableOVRController(staffHoldingHand);
        staffHoldInverseController = Utility.GetEnableOVRController(staffHoldingHand, true);
    }


    private OVRInput.Controller GetController(bool inverse)
    {
        var controller = inverse ? staffHoldInverseController : staffHoldController;
        return controller;
    }
}
