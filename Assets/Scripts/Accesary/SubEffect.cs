using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SubEffect : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips = default;
    [SerializeField] float disableTime = 2f;

    private AudioSource audioSource;
    private float time = 0;


    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        int index = Random.Range(0, audioClips.Length - 1);
        var clip = audioClips[index];
        audioSource.PlayOneShot(clip);
    }
    void Update()
    {
        if (gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if (time >= disableTime)
            {
                this.gameObject.SetActive(false);
                time = 0;
            }
        }
    }
}
