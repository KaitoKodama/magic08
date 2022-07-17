using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMN;
using State = StateMachine<Player>.State;

public class Player : Actor
{
    [SerializeField] Staff staff = default;
    [SerializeField] float moveSpeed = 1f;

    private OVRPlayerController controller;
    private StateMachine<Player> stateMachine;

    private float[] rotates = new float[] { 5f, 22.5f, 45f, 90f, };
    private int rotateIndex = 2;


    //------------------------------------------
    // Unity�����^�C��
    //------------------------------------------
    private void Awake()
    {
        Locator<Player>.Bind(this);
    }
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
        stateMachine.Update();
    }
    private void OnDestroy()
    {
        Locator<Player>.Unbind(this);
    }


    //------------------------------------------
    // �O�����L�֐�
    //------------------------------------------
    public float Rotate { get { return rotates[rotateIndex]; } }
    public bool IsStaffHolding { get; private set; } = false;
    public void SetActorRotate(int add)
    {
        rotateIndex = Mathf.Clamp(rotateIndex + add, 0, rotates.Length - 1);
        controller.RotationRatchet = rotates[rotateIndex];
    }
    public void GenerateMagic(Transform origin, DataVisual visual)
    {
        GenerateFromVisual(data.CharacterType, origin, visual);
    }


    //------------------------------------------
    // �������L�֐�
    //------------------------------------------
    private void UpdateStaffLocater()
    {
        var input = Locator<PlayerInput>.I;
        float gripAmount = input.GetIndex() + input.GetTrigger();
        float gripAmountRequire = 1.5f;
        if (gripAmount >= gripAmountRequire)
        {
            if (!IsStaffHolding)
            {
                staff.OnGrabState(true, input.StaffHoldAncher);
                IsStaffHolding = true;
            }
        }
        else
        {
            if (IsStaffHolding)
            {
                staff.OnGrabState(false);
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
                staff.RequestToSwapOrAnimateBegin(false);
            }
            if (EnableMagic)
            {
                if (input.IsIndex(true))
                {
                    TryExcuteMagic(staff.Tips.forward);
                    staff.RequestToSwapOrAnimateBegin(true);
                    input.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
                }
            }
        }
        else
        {
            if (EnableMagic)
            {
                TryExcuteMagic(staff.Tips.forward);
                staff.RequestToSwapOrAnimateBegin(true);
            }
        }
    }


    //------------------------------------------
    // �C���^�[�t�F�C�X
    //------------------------------------------
    protected override void OnApplyDamage(float damage)
    {
        Debug.Log($"Hit To Actor {damage}");
    }


    //------------------------------------------
    // �X�e�[�g�}�V��
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
