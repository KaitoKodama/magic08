using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMN;
using State = StateMachine<Actor>.State;

public class Actor : MonoBehaviour, IApplyDamage
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
                staff.OnGrabState(true, input.StaffHoldAncher);
                isStaffHolding = true;
            }
        }
        else
        {
            if (isStaffHolding)
            {
                staff.OnGrabState(false);
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
                staff.OnMagicShoot(false);
            }
            if (exist)
            {
                if (input.IsIndex(true))
                {
                    staff.OnMagicShoot(true);
                    input.OnVivration(0.1f, ActorInput.VivrateHand.Holding);
                }
            }
        }
        else
        {
            if (exist)
            {
                staff.OnMagicShoot(true);
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log("Damaged" + damage);
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
