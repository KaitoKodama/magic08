using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IApplyDamage
{
    [SerializeField] 
    protected ActorData data = default;
    protected float hp, mp;
    private bool refilling = false;


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
    public Magic EnableMagic { get; private set; }
    public ActorData ActorData { get; private set; }


    //------------------------------------------
    // 継承先共有抽象関数
    //------------------------------------------
    protected abstract void OnApplyDamage(float damage);


    //------------------------------------------
    // 継承先・外部共有インターフェイス
    //------------------------------------------
    public virtual void ApplyDamage(Magic self, CharacterType character, float damage)
    {
        if (character != data.CharacterType)
        {
            self.DoDestroy();
            OnApplyDamage(damage);
        }
    }


    //------------------------------------------
    // デリゲート
    //------------------------------------------
    public delegate void OnDeathNotifyer();
    public OnDeathNotifyer OnDeathNotifyerHandler;


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected bool IsEnableAttack { get { return (mp > 0 && !refilling); } }


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void GenerateFromVisual(CharacterType character, Transform origin, DataVisual visual)
    {
        if (EnableMagic == null)
        {
            mp -= visual.RequireMP;
            var obj = Instantiate(visual.Prefab, origin.position, Quaternion.identity);
            var magic = obj.GetComponent<Magic>();
            magic.OnGenerate(character, visual, origin);
            EnableMagic = magic;
            if (mp <= 0) refilling = true;
        }
    }
    protected void ExcuteOrGenerateFromVisual(Transform origin, DataVisual visual, Vector3 expect)
    {
        if (EnableMagic == null)
        {
            GenerateFromVisual(data.CharacterType, origin, visual);
        }
        EnableMagic?.OnExcute(expect);
        EnableMagic = null;
    }
    protected void TryExcuteMagic(Vector3 expect)
    {
        EnableMagic?.OnExcute(expect);
        EnableMagic = null;
    }
}
