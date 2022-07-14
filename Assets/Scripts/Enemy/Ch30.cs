using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<Ch30>.State;

public class Ch30 : Enemy
{
    private StateMachine<Ch30> stateMachine;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Start()
    {
        stateMachine = new StateMachine<Ch30>(this);
        stateMachine.AddTransition<StateAttack, StateChase>(((int)Event.Chase));
        stateMachine.AddTransition<StateDamage, StateChase>(((int)Event.Chase));
        stateMachine.AddTransition<StateChase, StateAttack>(((int)Event.Attack));
        stateMachine.AddTransition<StateChase, StateDamage>(((int)Event.Damage));
        stateMachine.AddTransition<StateChase, StateDeath>(((int)Event.Death));
        stateMachine.AddTransition<StateAttack, StateDeath>(((int)Event.Death));
        stateMachine.AddTransition<StateDamage, StateDeath>(((int)Event.Death));
        stateMachine.Start<StateChase>();
    }
    private void Update()
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
    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);

        int index = IsDeath ? ((int)Event.Death) : ((int)Event.Damage);
        stateMachine.Dispatch(index);
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
            Debug.Log("Do Chase!!!");
            owner.SetAgentDestination(3f);
        }
        protected override void OnUpdate()
        {
            if (owner.IsInDestination())
            {
                stateMachine.Dispatch(((int)Event.Attack));
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
            Debug.Log("Do Attack!!!");
        }
        protected override void OnUpdate()
        {
            stateMachine.Dispatch(((int)Event.Chase));
        }
    }
    private class StateDamage : State { }
    private class StateDeath : State { }
}
