using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FootSlide))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected EnemyData data = default;
    protected Transform actorform;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float hp, mp;

    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");

    private FootSlide footSlide;
    private Magic activeMagic;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Awake()
    {
        actorform = GameObject.FindGameObjectWithTag("Actor").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        footSlide = GetComponent<FootSlide>();

        if (data != null)
        {
            hp = data.HP;
            mp = data.MP;
        }
        else
        {
            Debug.LogError("データを格納してください");
        }
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public delegate void OnDeathNotifyer();
    public OnDeathNotifyer OnDeathNotifyerHandler;
    public bool IsDeath { get; private set; } = false;
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


    //------------------------------------------
    // 継承先共有関数
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
    protected void UpdateLocomotion(float lerp = 1f)
    {
        var dir = (agent.destination - transform.position);
        var axis = Vector3.Cross(transform.forward, dir);
        var angle = Vector3.Angle(transform.forward, dir) * (axis.y < 0 ? -1 : 1);
        var vec = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;

        float currntX = animator.GetFloat(HorizontalHash);
        float currntZ = animator.GetFloat(VerticalHash);
        float smoothX = Mathf.Lerp(currntX, vec.x, Time.deltaTime * lerp);
        float smoothZ = Mathf.Lerp(currntZ, vec.z, Time.deltaTime * lerp);

        footSlide.SetFootSpeed(agent.velocity);
        animator.SetFloat(HorizontalHash, smoothX);
        animator.SetFloat(VerticalHash, smoothZ);
    }


    protected void OnGenerateMagic(Transform origin)
    {
        if (activeMagic == null)
        {
            var visual = GetRandomVisualData();
            if (visual != null)
            {
                var obj = Instantiate(visual.Prefab, origin.position, Quaternion.identity);
                var magic = obj.GetComponent<Magic>();
                magic.OnGenerate(visual, origin);
                activeMagic = magic;
            }
        }
    }
    protected void OnExcuteMagic(Transform origin, Transform expect)
    {
        if (activeMagic == null)
        {
            OnGenerateMagic(origin);
        }
        activeMagic.OnExcute(expect);
        activeMagic = null;
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private DataVisual GetRandomVisualData()
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
