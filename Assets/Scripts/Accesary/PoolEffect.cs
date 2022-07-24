using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEffect : MonoBehaviour
{
    [SerializeField] GameObject damagePrefab = default;
    [SerializeField] GameObject breakPrefab = default;
    [SerializeField] GameObject[] hitPrefabs = default;

    private List<DamageBox> damageBoxList = new List<DamageBox>();
    private List<GameObject> breakPoolList = new List<GameObject>();
    private List<GameObject> hitPoolList = new List<GameObject>();
    private int poolNum = 5;
    private int dPoolNum = 10;


    private void Awake()
    {
        Locator<PoolEffect>.Bind(this);

        for (int i = 0; i < dPoolNum; i++)
        {
            var obj = Instantiate(damagePrefab);
            var box = obj.GetComponent<DamageBox>();
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            damageBoxList.Add(box);
        }

        SetPool(ref breakPoolList, breakPrefab, poolNum);
        foreach (var prefab in hitPrefabs)
        {
            SetPool(ref hitPoolList, prefab, poolNum);
        }
    }
    private void OnDestroy()
    {
        Locator<PoolEffect>.Unbind(this);
    }


    public void SetDamageText(Vector3 startPos, float value)
    {
        foreach (var box in damageBoxList)
        {
            if (!box.gameObject.activeSelf)
            {
                box.SetDamageText(startPos, value);
                break;
            }
        }
    }
    public GameObject GetHitPool()
    {
        Shuffle(hitPoolList);
        foreach (var obj in hitPoolList)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
    public GameObject GetBreakPool()
    {
        foreach (var obj in breakPoolList)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }


    private void SetPool(ref List<GameObject> list, GameObject prefab, int num)
    {
        for (int i = 0; i < num; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            list.Add(obj);
        }
    }
    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
