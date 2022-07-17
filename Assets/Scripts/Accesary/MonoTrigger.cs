using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTrigger : MonoBehaviour
{
    private Action<Collider> action;

    private void OnTriggerEnter(Collider other)
    {
        if (action != null)
        {
            action(other);
        }
    }
    public void AddTriggerEnterListner(Action<Collider> action)
    {
        this.action = action;
    }
}
