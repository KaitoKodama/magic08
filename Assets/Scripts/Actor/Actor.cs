using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMN;
using State = StateMachine<Actor>.State;

public class Actor : MonoBehaviour
{
    [SerializeField] Staff staff = default;
    [SerializeField] float moveSpeed = 1f;

    private OVRPlayerController playerController;
    private StateMachine<Actor> stateMachine;

    private bool isStaffHolding = false;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Awake()
    {
        playerController = GetComponent<OVRPlayerController>();
        playerController.SetMoveScaleMultiplier(moveSpeed);
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
    public delegate void OnMagicShoot(bool enable);
    public event OnStaffHolingNotifyer OnStaffHolingNotifyerHandler;
    public event OnMagicShoot OnMagicShootHandler;


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void UpdateStaffLocater()
    {
        var input = Locator<ActorInput>.I;
        float gripAmount = input.GetIndex() + input.GetTrigger();
        float gripAmountRequire = 1.5f;
        if (gripAmount >= gripAmountRequire)
        {
            if (!isStaffHolding)
            {
                OnStaffHolingNotifyerHandler?.Invoke(true);
                isStaffHolding = true;
            }
        }
        else
        {
            if (isStaffHolding)
            {
                OnStaffHolingNotifyerHandler?.Invoke(false);
                isStaffHolding = false;
            }
        }
    }
    private void UpdateSelecterRequest()
    {
        bool exist = staff.IsShootEnable;
        if (isStaffHolding)
        {
            var input = Locator<ActorInput>.I;
            if (input.IsTrigger(true))
            {
                OnMagicShootHandler?.Invoke(false);
            }
            if (exist)
            {
                if (input.IsIndex(true))
                {
                    OnMagicShootHandler?.Invoke(true);
                }
            }
        }
        else
        {
            if (exist)
            {
                OnMagicShootHandler?.Invoke(true);
            }
        }
    }


    //------------------------------------------
    // ステートマシン
    //------------------------------------------
    enum Event
    {
        BeginNormal
    }
    private class StateNormal : State
    {
        protected override void OnUpdate()
        {
            owner.UpdateStaffLocater();
            owner.UpdateSelecterRequest();
        }
    }
}
