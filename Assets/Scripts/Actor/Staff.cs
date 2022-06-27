using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] Transform tips = default;
    private Material material;
    private float timeScale = 5f;


    private void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        SetShader(1f, true);
    }


    public Transform Tips => tips;
    public void SetShader(float value, bool force = false)
    {
        float prev = material.GetFloat("_Cutoff");
        float lerp = Mathf.Lerp(prev, value, Time.deltaTime * timeScale);
        if (force == true) lerp = value;
        material.SetFloat("_Cutoff", lerp);
    }
}
