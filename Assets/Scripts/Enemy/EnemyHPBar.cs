using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    private Material material;
    private float startX = 0.47f;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    public void SetHPBar(float hp, float maxHP)
    {
        float fixHP = (hp / maxHP) * startX;
        material.mainTextureOffset = new Vector2(fixHP, 0);
    }
}
