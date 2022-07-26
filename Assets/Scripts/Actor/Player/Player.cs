using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMN;
using State = StateMachine<Player>.State;

public class Player : Actor
{
    [SerializeField] HPUI hpui = default;
    [SerializeField] Staff staff = default;
    [SerializeField] Selecter selecter = default;
    [SerializeField] float moveSpeed = 1f;

    private OVRPlayerController controller;
    private StateMachine<Player> stateMachine;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private new void Start()
    {
        base.Start();

        controller = GetComponent<OVRPlayerController>();
        controller.SetMoveScaleMultiplier(moveSpeed);
        stateMachine = new StateMachine<Player>(this);
        stateMachine.Start<StateNormal>();
    }
    private new void Update()
    {
        base.Update();

        hpui.UpdateStatePanel(data, hp, mp);
        stateMachine.Update();
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public bool IsStaffHolding { get; private set; } = false;


    //------------------------------------------
    // 外部共有継承関数
    //------------------------------------------
    public override void ApplyDamage(Transform transform, float damage)
    {
        hp -= damage;
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void UpdateStaffAndSelecterState()
    {
        var input = Locator<PlayerInput>.I;
        if (input.GetIndex() + input.GetTrigger() >= 1.5f)
        {
            if (!IsStaffHolding)
            {
                staff.OnGrabState(true, input.StaffHoldAncher);
                selecter.SetSelecterState(true);
                IsStaffHolding = true;
            }
        }
        else
        {
            if (IsStaffHolding)
            {
                staff.OnGrabState(false);
                selecter.SetSelecterState(false);
                IsStaffHolding = false;
            }
        }
    }
    private void UpdateSelecterRequest()
    {
        if (IsStaffHolding)
        {
            var input = Locator<PlayerInput>.I;
            if (input.IsTrigger(true))
            {
                selecter.SetGridContentsOrGenerate(OnGenerateCallback);
            }
            if (input.IsIndex(true))
            {
                TryExcuteIfEnable();
            }
        }
        else TryExcuteIfEnable();
    }
    private void OnGenerateCallback(DataVisual visual)
    {
        if (EnableMagic == null)
        {
            RequestGenerate(staff.Tips, visual, OnCompleted, OnFailed);
        }
        else Locator<Logger>.I.Log($"連続で発動準備は行えません\n{EnableMagic.Data.NameJa}の発動を完了させてください");
        selecter.SetGridContents();
    }
    private void OnCompleted(DataVisual visual)
    {
        Locator<Logger>.I.Log($"{EnableMagic.Data.NameJa}の発動準備を完了しました");
    }
    private void OnFailed(DataVisual visual)
    {
        Locator<Logger>.I.Log($"{visual.NameJa}の発動準備に十分な魔力がありません\n回復を待つかほかの魔法を使用してください");
    }
    private void TryExcuteIfEnable()
    {
        if (EnableMagic)
        {
            var input = Locator<PlayerInput>.I;
            staff.OnShootAnimation();
            RequestExcute(staff.Tips.forward);
            input.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
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
            owner.UpdateStaffAndSelecterState();
            owner.UpdateSelecterRequest();
        }
    }
}
