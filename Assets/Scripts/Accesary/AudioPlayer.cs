using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClip = default;
    private AudioSource audioSource;
    private int index = 0;
    private bool isPlaying;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!isPlaying)
        {
            StartCoroutine(AudioPlay());
        }
    }


    //private void Shuffle(AudioClip[] num)
    //{
    //    for (int i = 0; i < num.Length; i++)
    //    {
    //        AudioClip temp = num[i];
    //        int randomIndex = Random.Range(0, num.Length);
    //        num[i] = num[randomIndex];
    //        num[randomIndex] = temp;
    //    }
    //}
    private IEnumerator AudioPlay()
    {
        isPlaying = true;
        for (index = 0; index < audioClip.Length; index++)
        {
            audioSource.clip = audioClip[index];
            audioSource.Play();
            yield return new WaitForSeconds(audioClip[index].length);
        }
        isPlaying = false;
    }
}
