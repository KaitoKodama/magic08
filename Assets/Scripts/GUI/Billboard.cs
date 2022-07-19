using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Billboard : MonoBehaviour
{
    [Header("camformがnullの場合メインカメラが正面になります")]
    [SerializeField] Transform camform = null;

    private void Start()
    {
        if (camform == null)
        {
            camform = Camera.main.transform;
        }
    }
    private void Update()
    {
        if (camform != null)
        {
            var pos = camform.position;
            transform.LookAt(pos, Vector3.up);
        }
    }
}
