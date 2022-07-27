﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : MonoBehaviour
{
    private DataVisual data;
    private Actor owner;


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public DataVisual Data => data;
    public Actor Owner => owner;
    public void OnGenerate(Actor owner, DataVisual data, Transform origin)
    {
        this.data = data;
        this.owner = owner;
        TrackTarget = origin;
        transform.position = origin.position;
        Generate(data, origin);
    }
    public void OnExcute(Vector3 expect)
    {
        Excute(expect);
        IsExcute = true;
    }


    //------------------------------------------
    // 継承先共有抽象関数
    //------------------------------------------
    protected virtual void Generate(DataVisual data, Transform origin) { }
    protected virtual void Excute(Vector3 expect) { }
    protected virtual void OnTriggerActorCompleted(Actor actor) { }
    protected virtual void OnTriggerMagicCompleted(Magic magic) { }
    protected virtual void OnTriggerFieldCompleted() { }


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected Transform TrackTarget { get; private set; }
    protected bool IsExcute { get; private set; } = false;
    protected bool IsSameOwner(Actor target)
    {
        var ownerIsEnemy = owner.GetType().IsSubclassOf(typeof(Enemy));
        var targetIsEnemy = target.GetType().IsSubclassOf(typeof(Enemy));
        var ownerType = ownerIsEnemy ? typeof(Enemy) : typeof(Player);
        var targetType = targetIsEnemy ? typeof(Enemy) : typeof(Player);
        if (ownerType != targetType)
        {
            return true;
        }
        return false;
    }


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void CloningGenerate<T>(GameObject prefab) where T : Magic
    {
        var obj = Instantiate(prefab, transform.position, Quaternion.identity);
        var magic = obj.GetComponent<T>();
        magic.OnGenerate(Owner, Data, transform);
        magic.OnExcute(default);
    }
    protected void UpdateLerpIfNotExcute(float speed = 3f)
    {
        if (!IsExcute)
        {
            var lerp = Vector3.Lerp(transform.position, TrackTarget.position, Time.deltaTime * speed);
            transform.position = lerp;
            transform.forward = TrackTarget.forward;
        }
    }
    protected void UpdateChaseIfNotExcute()
    {
        if (!IsExcute)
        {
            transform.position = TrackTarget.position;
            transform.forward = TrackTarget.forward;
        }
    }
    protected void SetHitEffect()
    {
        var effect = Locator<PoolEffect>.I;
        if (effect != null)
        {
            var pool = effect.GetHitPool();
            if (pool != null)
            {
                pool.transform.position = transform.position;
            }
        }
    }
    protected void SetDamageBox(Vector3 startPos, float damage)
    {
        var effect = Locator<PoolEffect>.I;
        if (effect != null)
        {
            effect.SetDamageText(startPos, damage);
        }
    }
    protected void SetBreakEffect()
    {
        var effect = Locator<PoolEffect>.I;
        if (effect != null)
        {
            var pool = effect.GetBreakPool();
            if (pool != null)
            {
                pool.transform.position = transform.position;
            }
        }
    }
    protected void SetRigidVelocity(Vector3 expect, float speed = 5f)
    {
        var rigid = GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = expect * speed;
        }
    }
    protected void OnTriggerActor(Collider col)
    {
        var target = col.gameObject.GetComponent<Actor>();
        if (target != null)
        {
            if (IsSameOwner(target))
            {
                if (target.GetType() != typeof(Player))
                {
                    Locator<PlayerInput>.I.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
                }
                OnTriggerActorCompleted(target);
            }
        }
    }
    protected void OnTriggerMagic(Collider col)
    {
        var magic = col.gameObject.GetComponent<Magic>();
        if (magic != null)
        {
            if (IsSameOwner(magic.Owner))
            {
                OnTriggerMagicCompleted(magic);
            }
        }
    }
    protected void OnTriggerField(Collider col)
    {
        if (col.gameObject.CompareTag("Field"))
        {
            if (owner.GetType() == typeof(Player) && owner.EnableMagic != null)
            {
                Locator<PlayerInput>.I.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
            }
            OnTriggerFieldCompleted();
        }
    }
}
