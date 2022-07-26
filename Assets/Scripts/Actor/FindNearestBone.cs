using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearestBone : MonoBehaviour
{
    [SerializeField] GameObject decalPrefab = default;
    [SerializeField] float decalLifetime = 2f;
    [SerializeField] Transform[] bones = default;


    public void PaintDecalToBone(Transform origin)
    {
        float closet = float.MaxValue;
        Transform closetform = null;
        foreach (var bone in bones)
        {
            float distance = Vector3.Distance(bone.position, origin.position);
            if (distance <= closet)
            {
                closetform = bone;
                closet = distance;
            }
        }
        if (closetform != null)
        {
            var obj = Instantiate(decalPrefab, closetform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = origin.rotation;
            Destroy(obj, decalLifetime);
        }
    }
}
