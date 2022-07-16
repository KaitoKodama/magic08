using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<Ch30>.State;
using DG.Tweening;

public class Ch30 : Enemy
{
    [SerializeField] Transform ancherMagic = default;
    [SerializeField] Transform ancherExpect = default;

    private EnemyHPBar hpBar;
    private StateMachine<Ch30> stateMachine;

    private readonly int IsAttack01Hash = Animator.StringToHash("IsAttack01");
    private readonly int IsAttack02Hash = Animator.StringToHash("IsAttack02");
    private readonly int IsDamageHash = Animator.StringToHash("IsDamaged");
    private readonly int IsDeathHash = Animator.StringToHash("IsDeath");

    private bool isInvincible = false;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    protected override void VirtualStart()
    {
        base.VirtualStart();
        hpBar = GetComponentInChildren<EnemyHPBar>();

        stateMachine = new StateMachine<Ch30>(this);
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
    protected override void VirtualUpdate()
    {
        base.VirtualUpdate();
        hpBar.SetHPBar(hp, data.MaxHP);

        stateMachine.Update();
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public override void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            base.ApplyDamage(damage);
            int index = IsDeath ? ((int)Event.Death) : ((int)Event.Damage);
            stateMachine.Dispatch(index);
        }
    }
    public override void OnStateExit()
    {
        stateMachine.Dispatch(((int)Event.Chase));
    }

    public void OnExcute()
    {
        OnExcuteMagic(ancherMagic, ancherExpect.forward);
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
                int index = (owner.IsEnableAttack) ? ((int)Event.Attack) : ((int)Event.Chase);
                stateMachine.Dispatch(index);
            }
            else
            {
                owner.UpdateRotation();
                owner.UpdateLocomotion();
            }
        }
    }
    private class StateAttack : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.agent.isStopped = true;
            owner.isInvincible = true;
            owner.animator.SetBool(owner.IsAttack01Hash, true);
            owner.OnGenerateMagic(owner.ancherMagic);
        }
        protected override void OnExit(State nextState)
        {
            owner.agent.isStopped = false;
            owner.isInvincible = false;
            owner.animator.SetBool(owner.IsAttack01Hash, false);
        }
    }
    private class StateDamage : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.IsDamageHash, true);
        }
        protected override void OnExit(State nextState)
        {
            owner.agent.isStopped = false;
            owner.animator.SetBool(owner.IsDamageHash, false);
        }
    }
    private class StateDeath : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.agent.isStopped = true;
            owner.agent.enabled = false;
            owner.gameObject.GetComponent<FootIK>().enabled = false;
            owner.gameObject.GetComponent<Collider>().enabled = false;

            owner.SetVHHashToOneDirection();
            owner.animator.SetBool(owner.IsDeathHash, true);

            owner.GetComponent<MeshDissolver>().OnDissolveEntry(0.3f, 5f, () =>
            {
                Destroy(owner.gameObject, 1f);
            });
        }
    }
}
