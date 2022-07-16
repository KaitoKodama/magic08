using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FootSlide))]
public class Enemy : MonoBehaviour, IApplyDamage
{
    [SerializeField]
    protected EnemyData data = default;
    protected Transform actorform;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float hp, mp;
    protected int attackInedx;

    private readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    private readonly int DirectionZHash = Animator.StringToHash("DirectionZ");

    private FootSlide footSlide;
    private Magic activeMagic;
    private Vector3 lastPosition;
    private bool refilling = false;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Start()
    {
        VirtualStart();
    }
    private void Update()
    {
        VirtualUpdate();
    }
    protected virtual void VirtualStart()
    {
        actorform = GameObject.FindGameObjectWithTag("Actor").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        footSlide = GetComponent<FootSlide>();

        hp = data.MaxHP;
        mp = data.MaxMP;
    }
    protected virtual void VirtualUpdate()
    {
        if (refilling)
        {
            mp = Mathf.Clamp(mp + (data.RefillSpeed * Time.deltaTime), 0, data.MaxMP);
            if (mp >= data.MaxMP)
            {
                refilling = false;
            }
        }
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public delegate void OnDeathNotifyer();
    public OnDeathNotifyer OnDeathNotifyerHandler;
    public bool IsDeath { get; private set; } = false;
    public virtual void OnStateExit() { }
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


    //------------------------------------------
    // 継承先共有関数
    //------------------------------------------
    protected bool IsEnableAttack { get { return (mp > 0 && !refilling); } }
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
    protected void SetAgentDestination(float radius = 0f, Transform expect = null)
    {
        if (actorform != null)
        {
            Vector3 destination = default;
            float x = Random.Range(radius * -1, radius);
            float z = Random.Range(radius * -1, radius);
            if (expect == null)
            {
                destination.x = actorform.position.x + x;
                destination.z = actorform.position.z + z;
            }
            else
            {
                destination.x = expect.position.x + x;
                destination.z = expect.position.z + z;
            }
            destination.y = transform.position.y;
            agent.SetDestination(destination);
        }
    }
    protected void UpdateRotation()
    {
        if (actorform != null)
        {
            var look = actorform.position;
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

    protected void OnGenerateMagic(Transform origin)
    {
        if (activeMagic == null)
        {
            var visual = GetRandomVisualset();
            if (visual != null)
            {
                mp -= visual.RequireMP;
                var obj = Instantiate(visual.Prefab, origin.position, Quaternion.identity);
                var magic = obj.GetComponent<Magic>();
                magic.OnGenerate(visual, origin);
                activeMagic = magic;

                if (mp <= 0)
                {
                    refilling = true;
                }
            }
        }
    }
    protected void OnExcuteMagic(Transform origin, Vector3 expect)
    {
        if (activeMagic == null)
        {
            OnGenerateMagic(origin);
        }
        activeMagic?.OnExcute(expect);
        activeMagic = null;
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private DataVisual GetRandomVisualset()
    {
        int index = Random.Range(0, data.VisualList.Count - 1);
        if (data.VisualList[index].RequireMP >= mp)
        {
            return data.VisualList[index];
        }
        foreach (var visual in data.VisualList)
        {
            if (visual.RequireMP >= mp)
            {
                return visual;
            }
        }
        return null;
    }
}
