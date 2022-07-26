using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleArrow : Magic
{ 
    [SerializeField] MagicCircle circlePrefab = default;
    private Rigidbody rigid;
    private float downSpeed = 60f;
    private bool isGenerated = false;


    private void Update()
    {
        UpdateChaseIfNotExcute();
        if (isGenerated)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    private void FixedUpdate()
    {
        if (IsExcute && !rigid.isKinematic)
        {
            rigid.AddForce(Vector3.down * Time.fixedDeltaTime * downSpeed);
            if (transform.position.y <= 0)
            {
                rigid.isKinematic = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsExcute)
        {
            var target = other.GetComponent<Actor>();
            var magic = other.GetComponent<Magic>();
            if (target != null)
            {
                if (target.GetType() != Owner.GetType())
                {
                    CloningGenerate<MagicCircle>(circlePrefab.gameObject);
                    SetBreakEffect();
                    Destroy(this.gameObject);
                }
            }
            if (magic != null && rigid.isKinematic)
            {
                SetBreakEffect();
                Destroy(this.gameObject);
            }
        }
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        isGenerated = true;
    }
    protected override void Excute(Vector3 expect)
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.up;
    }
}
