using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<Skelton>.State;
using DG.Tweening;

public class Skelton : Enemy
{
    [SerializeField]
    private AudioClip[] footSounds = default;
    private HPUI hpui;
    private FootSlide footSlide;
    private AudioSource audioSource;
    private StateMachine<Skelton> stateMachine;

    private readonly int IsAttack01Hash = Animator.StringToHash("IsAttack01");
    private readonly int IsDamageHash = Animator.StringToHash("IsDamaged");
    private readonly int IsDeathHash = Animator.StringToHash("IsDeath");


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private new void Start()
    {
        base.Start();
        footSlide = GetComponent<FootSlide>();
        audioSource = GetComponent<AudioSource>();
        hpui = GetComponentInChildren<HPUI>();

        stateMachine = new StateMachine<Skelton>(this);
        stateMachine.AddTransition<StateAttack, StateChase>(((int)Event.Chase));
        stateMachine.AddTransition<StateDamage, StateChase>(((int)Event.Chase));
        stateMachine.AddTransition<StateChase, StateChase>(((int)Event.Chase));
        stateMachine.AddTransition<StateChase, StateAttack>(((int)Event.Attack));
        stateMachine.AddTransition<StateChase, StateDamage>(((int)Event.Damage));
        stateMachine.AddTransition<StateChase, StateDeath>(((int)Event.Death));
        stateMachine.AddTransition<StateAttack, StateDeath>(((int)Event.Death));
        stateMachine.AddTransition<StateDamage, StateDeath>(((int)Event.Death));
        stateMachine.Start<StateChase>();
    }
    private new void Update()
    {
        base.Update();
        hpui.UpdateStatePanel(data, hp, mp);
        stateMachine.Update();
    }


    //------------------------------------------
    // インターフェイス
    //------------------------------------------
    public override void ApplyDamage(Transform transform, float damage)
    {
        base.ApplyDamage(transform, damage);
        int index = IsDeath ? ((int)Event.Death) : ((int)Event.Damage);
        stateMachine.Dispatch(index);
    }


    //------------------------------------------
    // アニメーション通知
    //------------------------------------------
    public override void OnStateExit()
    {
        stateMachine.Dispatch(((int)Event.Chase));
    }
    public void OnFootStep()
    {
        int index = Random.Range(0, footSounds.Length - 1);
        var clip = footSounds[index];
        audioSource.PlayOneShot(clip);
    }
    public void OnExcute()
    {
        if (IsPlayerExist)
        {
            RequestExcute(transform.forward);
        }
    }


    //------------------------------------------
    // ステートマシン
    //------------------------------------------
    enum Event
    {
        Chase, Attack, Damage, Death,
    }
    private class StateChase : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.SetAgentDestination(5f);
        }
        protected override void OnUpdate()
        {
            if (owner.IsInDestination())
            {
                var visual = owner.GetRandomVisualset();
                owner.RequestGenerate(owner.AncherMagic, visual, OnCompletedCallback, OnFailedCallback);
            }
            owner.UpdateRotation();
            owner.UpdateLocomotion();
            owner.footSlide.SetFootSpeed(owner.Agent.velocity);
        }
        private void OnCompletedCallback(DataVisual visual)
        {
            stateMachine.Dispatch(((int)Event.Attack));
        }
        private void OnFailedCallback(DataVisual visual)
        {
            stateMachine.Dispatch(((int)Event.Chase));
        }
    }
    private class StateAttack : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.Agent.isStopped = true;
            owner.Animator.SetBool(owner.IsAttack01Hash, true);
        }
        protected override void OnUpdate()
        {
            owner.UpdateRotation();
        }
        protected override void OnExit(State nextState)
        {
            owner.Agent.isStopped = false;
            owner.Animator.SetBool(owner.IsAttack01Hash, false);
        }
    }
    private class StateDamage : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.Agent.isStopped = true;
            owner.Animator.SetBool(owner.IsDamageHash, true);
        }
        protected override void OnExit(State nextState)
        {
            owner.Agent.isStopped = false;
            owner.Animator.SetBool(owner.IsDamageHash, false);
        }
    }
    private class StateDeath : State
    {
        protected override void OnEnter(State prevState)
        {
            Locator<Logger>.I.Log($"{owner.data.OwnerName}を討伐しました");

            owner.Agent.isStopped = true;
            owner.Agent.enabled = false;
            owner.gameObject.GetComponent<FootIK>().enabled = false;
            owner.gameObject.GetComponent<Collider>().enabled = false;

            owner.SetVHHashToOneDirection();
            owner.Animator.SetBool(owner.IsDeathHash, true);

            owner.GetComponent<MeshDissolver>().OnDissolveEntry(0.3f, 5f, () =>
            {
                Destroy(owner.gameObject, 1f);
            });
        }
    }
}
