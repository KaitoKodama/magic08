using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioShot : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips = default;

    void Start()
    {
        int index = Random.Range(0, audioClips.Length - 1);
        var clip = audioClips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
