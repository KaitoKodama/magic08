using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using CMN;
using State = StateMachine<Actor>.State;

public class Actor : MonoBehaviour
{
    [SerializeField] StaffLocater leftLocater = default;
    [SerializeField] StaffLocater rightLocater = default;
    [SerializeField] StaffHoldingHand staffHoldingHand = default;
    private OVRInput.Controller staffHoldController;
    private OVRInput.Controller staffHoldInverseController;

    private Staff staff;
    private Animator animator;
    private MagicSelecter selecter;
    private StaffLocater enableLocater;
    private OVRPlayerController playerController;
    private StateMachine<Actor> stateMachine;

    private readonly int GripLeftHash = Animator.StringToHash("GripLeft");
    private readonly int GripRightHash = Animator.StringToHash("GripRight");
    private readonly int TriggerLeftHash = Animator.StringToHash("TriggerLeft");
    private readonly int TriggerRightHash = Animator.StringToHash("TriggerRight");
    private bool isStaffHolding = false;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Awake()
    {
        staff = GetComponentInChildren<Staff>();
        animator = GetComponentInChildren<Animator>();
        selecter = GetComponentInChildren<MagicSelecter>();
        playerController = GetComponentInChildren<OVRPlayerController>();
        playerController.SetMoveScaleMultiplier(0.5f);
        SetStaffHoldingHand(staffHoldingHand);
    }
    void Start()
    {
        stateMachine = new StateMachine<Actor>(this);
        stateMachine.Start<StateNormal>();
    }
    void Update()
    {
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public delegate void OnStaffHolingNotifyer(bool isHolding);
    public OnStaffHolingNotifyer OnStaffHolingNotifyerHandler;
    public OVRInput.Controller StaffHoldController => staffHoldController;
    public OVRInput.Controller StaffHoldInverseController => staffHoldInverseController;


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void UpdateHandAnimation()
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
    private void UpdateStaffLocater()
    {
        float trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, staffHoldController);
        float index = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, staffHoldController);
        float gripAmount = index + trigger;
        float gripAmountRequire = 1.5f;
        if (gripAmount >= gripAmountRequire)
        {
            if (!isStaffHolding)
            {
                enableLocater.SetStaffLocation(staff.transform);
                OnStaffHolingNotifyerHandler?.Invoke(true);
                isStaffHolding = true;
            }
        }
        else
        {
            if (isStaffHolding)
            {
                enableLocater.UnsetStaffLocation(staff.transform);
                OnStaffHolingNotifyerHandler?.Invoke(false);
                isStaffHolding = false;
            }
        }
        staff.SetShader(((gripAmount / 2) * -1) + 1);
    }
    private void UpdateSelecterRequest()
    {
        bool trigger = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, staffHoldInverseController);
        bool index = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, staffHoldInverseController);
        if (isStaffHolding)
        {
            if (trigger)
            {
                selecter.RequestSwapGridContent(staff.Tips);
            }
            if (selecter.IsResisterExist)
            {
                if (index)
                {
                    selecter.RequestExcute(staff.Tips);
                }
            }
        }
        else
        {
            if (selecter.IsResisterExist)
            {
                selecter.RequestExcute(staff.Tips);
            }
        }
    }

    private void SetStaffHoldingHand(StaffHoldingHand expectHand)
    {
        staffHoldingHand = expectHand;
        var locater = (staffHoldingHand == StaffHoldingHand.LeftHand) ? leftLocater : rightLocater;
        enableLocater = locater;

        staffHoldController = Utility.GetEnableOVRController(staffHoldingHand);
        staffHoldInverseController = Utility.GetEnableOVRController(staffHoldingHand, true);
    }
    private void ResetStaffStates()
    {
        staff.SetShader(1f, true);
        enableLocater.UnsetStaffLocation(staff.transform);
        OnStaffHolingNotifyerHandler?.Invoke(false);
        isStaffHolding = false;
    }


    //------------------------------------------
    // イベント通知
    //------------------------------------------


    //------------------------------------------
    // ステートマシン
    //------------------------------------------
    enum Event
    {
        BeginNormal, BeginBattle, OpenMenu, GameOver,
    }
    private class StateNormal : State
    {
        protected override void OnUpdate()
        {
            owner.UpdateStaffLocater();
            owner.UpdateHandAnimation();
            owner.UpdateSelecterRequest();
        }
    }
    private class StateBattle : State
    {
        protected override void OnUpdate()
        {
            owner.UpdateStaffLocater();
            owner.UpdateHandAnimation();
            owner.UpdateSelecterRequest();
        }
    }
    private class StateMenuOperation : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.ResetStaffStates();
        }
        protected override void OnUpdate()
        {
            owner.UpdateHandAnimation();
        }
    }
}
