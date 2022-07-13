using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireToach : MonoBehaviour
{
    [SerializeField] GameObject fire = default;

    private void Start()
    {
        fire.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        var magic = other.GetComponent<Magic>();
        if (magic != null)
        {
            if (magic.Data.Attribute == MagicContext.MagicAttribute.Flame)
            {
                if (!fire.activeSelf)
                {
                    fire.SetActive(true);
                    Destroy(magic.gameObject);
                }
            }
            else if (magic.Data.Attribute == MagicContext.MagicAttribute.Water)
            {
                if (fire.activeSelf)
                {
                    fire.SetActive(false);
                    Destroy(magic.gameObject);
                }
            }
        }
    }
}
