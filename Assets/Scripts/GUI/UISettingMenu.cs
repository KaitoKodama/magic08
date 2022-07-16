using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Controller;

public class UISettingMenu : MonoBehaviour
{
    [System.Serializable]
    class CUnit
    {
        [SerializeField] UICButton button = default;
        [SerializeField] UICButton prev = default;
        [SerializeField] UICButton next = default;
        [SerializeField] Text text = default;
        [SerializeField] RectTransform panelRect = default;

        public UICButton UIC => button;
        public UICButton UICPrev => prev;
        public UICButton UICNext => next;
        public void SetText(string str)
        {
            text.text = str;
        }
        public void OnEnable(bool enable)
        {
            if (enable)
            {
                panelRect.DOAnchorPosX(11.34f, 0.2f).OnComplete(() =>
                {
                    prev.IsEnable = true;
                    next.IsEnable = true;
                });
            }
            else
            {
                panelRect.DOAnchorPosX(-11.43f, 0.2f);
                prev.IsEnable = false;
                next.IsEnable = false;
            }
        }
    }

    [SerializeField] GameObject menuCanvas = default;

    [SerializeField] CUnit rotate = default;
    [SerializeField] CUnit hand = default;

    private ActorCalibrator calibrator;
    private ActorInput actorInput;
    private bool enable = false;


    // Unityランタイム
    private void Start()
    {
        var actor = GameObject.FindWithTag("Actor");
        actorInput = actor.GetComponent<ActorInput>();
        calibrator = actor.GetComponent<ActorCalibrator>();

        rotate.UIC.AddListener(() => { OnButtonDown(rotate); });
        hand.UIC.AddListener(() => { OnButtonDown(hand); });

        rotate.UICNext.AddListener(() => { OnRotateButton(1); });
        rotate.UICPrev.AddListener(() => { OnRotateButton(-1); });
        hand.UICNext.AddListener(() => { OnHandButton(StaffHoldingHand.RightHand); });
        hand.UICPrev.AddListener(() => { OnHandButton(StaffHoldingHand.LeftHand); });

        menuCanvas.SetActive(false);
        ResetCUnitEnable();
    }
    private void Update()
    {
        var input = Locator<ActorInput>.I;
        if (input.IsStart(true) || input.IsThumb(true))
        {
            enable = !enable;
            menuCanvas.SetActive(enable);
            if (!enable)
            {
                ResetCUnitEnable();
            }
        }
    }


    // 内部共有関数
    private void ResetCUnitEnable()
    {
        rotate.OnEnable(false);
        hand.OnEnable(false);
    }


    // イベントハンドラ
    private void OnButtonDown(CUnit selected)
    {
        rotate.OnEnable(selected == rotate);
        hand.OnEnable(selected == hand);
    }
    private void OnRotateButton(int add)
    {
        calibrator.SetActorRotate(add);
        float rot = calibrator.Rotate;
        rotate.SetText($"{rot}°");
    }
    private void OnHandButton(StaffHoldingHand expectHand)
    {
        actorInput.SetStaffHoldingHand(expectHand);
        string txt = (expectHand == StaffHoldingHand.LeftHand) ? "左手" : "右手";
        hand.SetText(txt);
    }
}
