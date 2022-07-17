using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Tracker
{
    [SerializeField] Transform trackTarget = default;
    [SerializeField] Transform tracker = default;
    [SerializeField] Vector3 rotateOffset = default;
    [SerializeField] Vector3 positionOffset = default;
    [SerializeField] float rotateSpeed = 5;
    [SerializeField] float positionSpeed = 5;

    public enum VecOp { X, Y, Z }
    public void UpdatePosition()
    {
        if (trackTarget != null)
        {
            var pos = trackTarget.position;
            pos.x += positionOffset.x;
            pos.y += positionOffset.y;
            pos.z += positionOffset.z;
            this.tracker.position = Vector3.Lerp(tracker.position, pos, Time.deltaTime * positionSpeed);
        }
    }
    public void UpdateRotation()
    {
        if (trackTarget != null)
        {
            var euler = trackTarget.rotation.eulerAngles;
            euler.x += rotateOffset.x;
            euler.y += rotateOffset.y;
            euler.z += rotateOffset.z;
            var rot = Quaternion.Euler(euler);
            this.tracker.rotation = Quaternion.Lerp(tracker.rotation, rot, Time.deltaTime * rotateSpeed);
        }
    }
    public void UpdateRotation(VecOp op, float scale = 1f)
    {
        if (trackTarget != null)
        {
            var euler = trackTarget.rotation.eulerAngles;
            euler.x += rotateOffset.x;
            euler.y += rotateOffset.y;
            euler.z += rotateOffset.z;

            if (op == VecOp.X) euler.x *= scale;
            if (op == VecOp.Y) euler.y *= scale;
            if (op == VecOp.Z) euler.z *= scale;

            var rot = Quaternion.Euler(euler);
            this.tracker.rotation = Quaternion.Lerp(tracker.rotation, rot, Time.deltaTime * rotateSpeed);
        }
    }
    public void UpdateFixRotation(VecOp op, float expect = 0f)
    {
        if (trackTarget != null)
        {
            var vec = trackTarget.rotation.eulerAngles;
            var lerp = Quaternion.Lerp(tracker.rotation, Quaternion.Euler(vec), Time.deltaTime * rotateSpeed);
        
            var euler = lerp.eulerAngles;
            if (op == VecOp.X) euler.x = expect;
            if (op == VecOp.Y) euler.y = expect;
            if (op == VecOp.Z) euler.z = expect;

            this.tracker.rotation = Quaternion.Euler(euler);
        }
    }

    public void ResetPosition()
    {
        if (trackTarget != null)
        {
            this.tracker.position = trackTarget.position;
        }
    }
    public void ResetRotation()
    {
        if (trackTarget != null)
        {
            this.tracker.rotation = trackTarget.rotation;
        }
    }
}
