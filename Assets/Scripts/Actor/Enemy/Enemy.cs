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
    }


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
    protected override void OnApplyDamage(float damage)
    {
        Locator<PlayerInput>.I?.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
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
        float x = Random.Range(radius * -1, radius);
        float z = Random.Range(radius * -1, radius);
        if (expect != null)
        {
            origin = expect;
        }
        else
        {
            origin = IsPlayerExist ? Locator<Player>.I.transform : transform;
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
            var look = Locator<Player>.I.transform.position;
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
    protected void GenerateMagic()
    {
        var visual = GetRandomVisualset();
        if (visual != null)
        {
            GenerateFromVisual(data.CharacterType, ancherMagic, visual);
        }
    }
    protected void ExcuteMagic()
    {
        var visual = GetRandomVisualset();
        if (visual != null && IsPlayerExist)
        {
            var pos = (Locator<Player>.I.transform.position - transform.position).normalized;
            pos.y = 0;
            ExcuteOrGenerateFromVisual(ancherMagic, visual, pos);
        }
    }


    //------------------------------------------
    // 内部共有関数 - 戻り値あり
    //------------------------------------------
    private bool IsPlayerExist { get { return Locator<Player>.I != null; } }
    private DataVisual GetRandomVisualset()
    {
        var data = this.data as EnemyData;
        int index = Random.Range(0, data.VisualList.Count);
        if (data.VisualList[index].RequireMP <= mp)
        {
            return data.VisualList[index];
        }
        foreach (var visual in data.VisualList)
        {
            if (visual.RequireMP <= mp)
            {
                return visual;
            }
        }
        return null;
    }
}