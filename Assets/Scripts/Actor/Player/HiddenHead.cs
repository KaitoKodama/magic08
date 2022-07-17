using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenHead : MonoBehaviour
{
    [SerializeField] Transform activeHead = default;

    void Update()
    {
        transform.position = activeHead.position;
        transform.rotation = activeHead.rotation;
    }
}
