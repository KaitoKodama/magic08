using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;
using DG.Tweening;

public class MagicCircleArrow : Magic
{
    [SerializeField] GameObject enemyIcon = default;
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GradiantSet gradiantSet = default;
    [SerializeField] MagicCircle circlePrefab = default;
    private Rigidbody rigid;
    private Tweener circleTween;
    private float downSpeed = 60f;
    private float time = 0;
    private float lifeTime = 5f;
    private bool isGenerated = false;
    private bool isActivated = false;
    private bool isLifeOver = false;


    private void Update()
    {
        UpdateChaseIfNotExcute();
        if (isGenerated)
        {
            transform.rotation = Quaternion.identity;
        }
        if (isActivated && !isLifeOver)
        {
            time += Time.deltaTime;
            if (time >= lifeTime)
            {
                isLifeOver = true;
                circleTween = transform.DOScale(0, 0.5f).OnComplete(() =>
                {
                    SetBreakEffect();
                    circleTween.Kill(true);
                    Destroy(this.gameObject);
                });
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsExcute && !isActivated)
        {
            rigid.AddForce(Vector3.down * Time.fixedDeltaTime * downSpeed);
            if (transform.position.y <= 0)
            {
                isActivated = true;
                rigid.isKinematic = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsExcute)
        {
            OnTriggerActor(other);
            if (isActivated)
            {
                OnTriggerMagic(other);
            }
        }
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        if (Owner.GetType() != typeof(Player))
        {
            enemyIcon.SetActive(true);
        }
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
        isGenerated = true;
    }
    protected override void Excute(Vector3 expect)
    {
        rigid = GetComponent<Rigidbody>();
        var velocity = Vector3.up + expect;
        rigid.velocity = velocity;
    }
    protected override void OnTriggerActorCompleted(Actor actor)
    {
        SetBreakEffect();
        CloningGenerate<MagicCircle>(circlePrefab.gameObject);
        Destroy(this.gameObject);
    }
    protected override void OnTriggerMagicCompleted(Magic magic)
    {
        SetBreakEffect();
        Destroy(this.gameObject);
    }
}
