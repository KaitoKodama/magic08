using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour, IApplyDamage
{
    [SerializeField]
    protected EnemyData data = default;
    protected Transform actorform;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float hp, mp;
    protected bool IsDeath { get; private set; } = false;

    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");


    private void Start()
    {
        actorform = GameObject.FindGameObjectWithTag("Actor").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        hp = data.HP;
        mp = data.MP;
    }


    public delegate void OnDeathNotifyer();
    public OnDeathNotifyer OnDeathNotifyerHandler;
    public virtual void OnAttackStateExit() { }
    public virtual void ApplyDamage(float damage)
    {
        if (!IsDeath)
        {
            hp -= damage;
            if (hp <= 0)
            {
                OnDeathNotifyerHandler?.Invoke();
                IsDeath = true;
                hp = 0;
            }
        }
    }


    protected void SetAgentDestination(Transform expect = null)
    {
        if (actorform != null)
        {
            var pos = expect == null ? actorform : expect;
            agent.SetDestination(pos.position);
        }
    }
    protected void LookToActorPosition()
    {
        if (actorform != null)
        {
            transform.LookAt(actorform.position, Vector3.up);
        }
    }
    protected void UpdateLocomotion()
    {

    }
}
