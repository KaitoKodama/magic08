using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : Magic
{
    private Transform trackTarget;
    private float trackSpeed = 3f;
    private float forceSpeed = 15f;
    private float destroyTime = 10f;
    private bool isExcute = false;


    private void Update()
    {
        if (!isExcute)
        {
            var lerp = Vector3.Lerp(transform.position, trackTarget.position, Time.deltaTime * trackSpeed);
            transform.position = lerp;
        }
    }


    public override void OnGenerate(DataVisual data, Transform origin)
    {
        this.data = data;
        trackTarget = origin;
        transform.position = origin.position;
    }
    public override void OnExcute(Transform expect)
    {
        isExcute = true;
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = expect.forward * forceSpeed;
        Destroy(this.gameObject, destroyTime);
    }
}
