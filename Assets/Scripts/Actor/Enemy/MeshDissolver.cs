using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeshDissolver : MonoBehaviour
{
    [SerializeField] GameObject mesh = default;
    [SerializeField] GameObject meshDissolve = default;

    private Material[] materials;
    private Action callback;
    private float speed = 1f;
    private float cutoff = 0f;
    private bool isEntry = false;


    private void Update()
    {
        if (isEntry)
        {
            cutoff += speed * Time.deltaTime;
            foreach (var mat in materials)
            {
                mat.SetFloat("_Cutoff", cutoff);
            }

            if (cutoff > 1)
            {
                callback();
                isEntry = false;
            }
        }
    }

    public void OnDissolveEntry(float speed, float delay, Action callback)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            mesh.SetActive(false);
            meshDissolve.SetActive(true);
            var render = meshDissolve.GetComponent<SkinnedMeshRenderer>();
            this.materials = render.materials;
            this.callback = callback;
            this.speed = speed;
            this.cutoff = 0f;
            this.isEntry = true;
        });
    }
}