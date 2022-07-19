using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FootSlide))]
public abstract class Enemy : Actor
{
    [SerializeField]
    private Transform ancherMagic = default;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected int attackInedx;

    private readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    private readonly int DirectionZHash = Animator.StringToHash("DirectionZ");

    private FootSlide footSlide;
    private Transform playerform;
    private Vector3 lastPosition;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    protected new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        footSlide = GetComponent<FootSlide>();
        playerform = GameObject.FindWithTag("Player")?.transform;
    }

    protected Transform AncherMagic => ancherMagic;


    //------------------------------------------
    // 継承先共有抽象関数
    //------------------------------------------
    public abstract void OnStateExit();


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public bool IsDeath { get; private set; } = false;


    //------------------------------------------
    // インターフェイス
    //------------------------------------------
    public override void ApplyDamage(float damage)
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


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected bool IsInDestination(float radius = 0.01f)
    {
        if (agent != null)
        {
            float distance = Vector3.Distance(transform.position, agent.destination);
            if (distance <= radius)
            {
                return true;
            }
        }
        return false;
    }


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void SetAgentDestination(float radius = 0f, Transform expect = null)
    {
        Transform origin;
        Vector3 destination;
        float x = UnityEngine.Random.Range(radius * -1, radius);
        float z = UnityEngine.Random.Range(radius * -1, radius);
        if (expect != null)
        {
            origin = expect;
        }
        else
        {
            origin = IsPlayerExist ? playerform : transform;
        }
        destination.x = origin.position.x + x;
        destination.z = origin.position.z + z;
        destination.y = transform.position.y;
        agent.SetDestination(destination);
    }
    protected void UpdateRotation()
    {
        if (IsPlayerExist)
        {
            var look = playerform.position;
            look.y = transform.position.y;
            transform.LookAt(look, Vector3.up);
        }
    }
    protected void UpdateLocomotion()
    {
        var current = transform.position;
        var dir = lastPosition - current;
        var local = transform.InverseTransformVector(dir);
        var local3D = local / Time.deltaTime;

        animator.SetFloat(DirectionXHash, local3D.x);
        animator.SetFloat(DirectionZHash, local3D.z);
        footSlide.SetFootSpeed(agent.velocity);

        lastPosition = current;
    }
    protected void SetVHHashToOneDirection()
    {
        float currentX = animator.GetFloat(DirectionXHash);
        float currentZ = animator.GetFloat(DirectionZHash);

        if (currentX > currentZ)
        {
            animator.SetFloat(DirectionXHash, Mathf.Abs(currentX));
            animator.SetFloat(DirectionZHash, 0);
        }
        else
        {
            animator.SetFloat(DirectionZHash, Mathf.Abs(currentZ));
            animator.SetFloat(DirectionXHash, 0);
        }
    }


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected Transform Playerform => playerform;
    protected bool IsPlayerExist { get { return playerform != null; } }
    protected DataVisual GetRandomVisualset()
    {
        var data = this.data as EnemyData;
        int index = UnityEngine.Random.Range(0, data.VisualList.Count);
        return data.VisualList[index];
    }
}