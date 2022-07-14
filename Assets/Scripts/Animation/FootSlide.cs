using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSlide : MonoBehaviour
{
    private Animator animator;
    private float walkAnimSpeed = 1f;

    private readonly int WalkSpeedHash = Animator.StringToHash("WalkSpeed");


    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void SetFootSpeed(Vector3 velocity)
    {
        var velocityXZ = Vector3.Scale(velocity, new Vector3(1, 0, 1));
        animator.SetFloat(WalkSpeedHash, velocityXZ.magnitude / walkAnimSpeed);
    }
}
