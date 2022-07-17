using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public abstract class Magic : MonoBehaviour
{
    protected DataVisual data;
    protected Transform trackTarget;


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public DataVisual Data => data;
    public void OnGenerate(CharacterType character, DataVisual data, Transform origin)
    {
        this.data = data;
        this.trackTarget = origin;
        this.CharacterType = character;
        transform.position = trackTarget.position;
        Generate(character, data, origin);
    }
    public void OnExcute(Vector3 expect)
    {
        IsExcute = true;
        Excute(expect);
    }
    public void DoDestroy()
    {
        this.Destroy();
    }


    //------------------------------------------
    // 継承先共有抽象関数
    //------------------------------------------
    protected virtual void Generate(CharacterType character, DataVisual data, Transform origin) { }
    protected abstract void Excute(Vector3 expect);
    protected abstract void Destroy();


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected CharacterType CharacterType { get; private set; }
    protected bool IsExcute { get; private set; } = false;


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void OnLerpToTarget(float speed = 3f)
    {
        var lerp = Vector3.Lerp(transform.position, trackTarget.position, Time.deltaTime * speed);
        transform.position = lerp;
        transform.forward = trackTarget.forward;
    }
    protected void OnChaseToTarget()
    {
        transform.position = trackTarget.position;
        transform.forward = trackTarget.forward;
    }
    protected void OnForceToRigidWithLifeTime(Vector3 expect, float speed = 5f, float lifeTime = 10f)
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = expect * speed;
        Destroy(this.gameObject, lifeTime);
    }
    protected void InstantinateResorces(string prefabName, float lifeTime = 5f)
    {
        var prefab = (GameObject)Resources.Load($"Prefabs/{prefabName}");
        var obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(obj, lifeTime);
    }
    protected void IApplyDamageTrigger(Collider other)
    {
        var target = other.GetComponent<IApplyDamage>();
        if (target != null)
        {
            target.ApplyDamage(this, CharacterType, data.Value);
        }
        OnBreakEffect(other);
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void OnBreakEffect(Collider other)
    {
        if (other.CompareTag("Field"))
        {
            var prefab = (GameObject)Resources.Load("Prefabs/BreakEffect");
            var obj = Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(obj, 5f);
            Destroy(this.gameObject);
        }
    }
}
