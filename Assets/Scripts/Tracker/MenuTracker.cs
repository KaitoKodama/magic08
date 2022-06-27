using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuTracker : MonoBehaviour
{
    [SerializeField] Tracker menuTracker = default;
    [SerializeField] CanvasGroup group = default;
    [SerializeField] float opacity = 1f;

    private Actor actor;
    private bool enable = false;

    private void Start()
    {
        actor = GetComponentInParent<Actor>();
        group.alpha = 0f;
    }
    void Update()
    {
        if (enable)
        {
            menuTracker.UpdatePosition();
            menuTracker.UpdateRotation();
        }

        bool start = OVRInput.GetDown(OVRInput.Button.Start, actor.StaffHoldInverseController);
        bool thumb = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, actor.StaffHoldInverseController);
        if (start || thumb)
        {
            enable = !enable;
            float alpha = enable ? opacity : 0f;
            group.DOFade(alpha, 0.5f);
        }
    }
}
