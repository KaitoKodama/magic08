using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMN;

public class ActorCalibrator : MonoBehaviour
{
    private OVRPlayerController controller;
    private float[] rotates = new float[] { 5f, 22.5f, 45f, 90f, };
    private int rotateIndex = 2;


    private void Start()
    {
        controller = GetComponent<OVRPlayerController>();
    }

    public float Rotate { get { return rotates[rotateIndex]; } }
    public void SetActorRotate(int add)
    {
        rotateIndex = Mathf.Clamp(rotateIndex + add, 0, rotates.Length - 1);
        controller.RotationRatchet = rotates[rotateIndex];
    }
}
