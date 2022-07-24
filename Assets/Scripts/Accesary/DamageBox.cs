using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageBox : MonoBehaviour
{
    [SerializeField] CanvasGroup group = default;
    [SerializeField] Text damageText = default;


    public void SetDamageText(Vector3 startPos, float value)
    {
        group.alpha = 1f;
        damageText.text = ((int)value).ToString();
        transform.position = startPos;
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.
            Append(transform.DOMoveY(1f, 0.5f).SetRelative()).
            Append(group.DOFade(0, 0.5f)).
            OnComplete(OnTextingCompleted);
        sequence.Play();
    }


    private void OnTextingCompleted()
    {
        gameObject.SetActive(false);
    }
}
