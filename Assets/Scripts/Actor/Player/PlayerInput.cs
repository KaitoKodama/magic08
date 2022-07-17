using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using CMN;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform staffLeftAncher = default;
    [SerializeField] Transform staffRightAncher = default;
    [SerializeField] StaffHoldingHand staffHoldingHand = default;

    private Transform staffHandAncher;
    private OVRInput.Controller staffHoldController;
    private OVRInput.Controller staffHoldInverseController;


    private void Start()
    {
        Locator<PlayerInput>.Bind(this);
        SetStaffHoldingHand(staffHoldingHand);
    }
    private void OnDestroy()
    {
        Locator<PlayerInput>.Unbind(this);
    }


    public enum VivrateHand { Holding, Left, Right, Both, }
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
    public void OnVivration(float time, VivrateHand vivrate)
    {
        StartCoroutine(Vivration(time, vivrate));
    }


    private OVRInput.Controller GetController(bool inverse)
    {
        var controller = inverse ? staffHoldInverseController : staffHoldController;
        return controller;
    }
    private OVRInput.Controller GetVivrateController(VivrateHand vivrate)
    {
        if (vivrate == VivrateHand.Holding) return GetController(false);
        if (vivrate == VivrateHand.Left) return OVRInput.Controller.LTouch;
        if (vivrate == VivrateHand.Right) return OVRInput.Controller.RTouch;
        return default;
    }
    private IEnumerator Vivration(float time, VivrateHand vivrate)
    {
        if (vivrate == VivrateHand.Both)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            yield return new WaitForSeconds(time);

            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
        else
        {
            var controller = GetVivrateController(vivrate);
            OVRInput.SetControllerVibration(1, 1, controller);
            yield return new WaitForSeconds(time);

            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }
}
