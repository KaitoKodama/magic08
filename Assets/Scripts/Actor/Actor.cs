using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    protected ActorData data = default;
    protected float hp, mp;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    protected void Start()
    {
        hp = data.MaxHP;
        mp = data.MaxMP;
    }
    protected void Update()
    {
        mp = Mathf.Clamp(mp + (data.RefillSpeed * Time.deltaTime), 0, data.MaxMP);
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public Magic EnableMagic { get; private set; }


    //------------------------------------------
    // 外部共有抽象関数
    //------------------------------------------
    public abstract void ApplyDamage(Transform transform, float damage);


    //------------------------------------------
    // デリゲート
    //------------------------------------------
    public delegate void OnDeathNotifyer();
    public OnDeathNotifyer OnDeathNotifyerHandler;


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void RequestGenerate(Transform origin, DataVisual visual, Action<DataVisual> completeCallback, Action<DataVisual> failCallback)
    {
        if (EnableMagic == null)
        {
            if ((mp - visual.RequireMP) < 0)
            {
                failCallback(visual);
            }
            else
            {
                mp -= visual.RequireMP;
                var obj = Instantiate(visual.Prefab, origin.position, Quaternion.identity);
                var magic = obj.GetComponent<Magic>();
                magic.OnGenerate(this, visual, origin);
                EnableMagic = magic;
                completeCallback(visual);
            }
        }
        else failCallback(visual);
    }
    protected void RequestExcute(Vector3 expect)
    {
        if (EnableMagic != null)
        {
            EnableMagic.OnExcute(expect);
            EnableMagic = null;
        }
    }
}
