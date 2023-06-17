using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip[] treeHitSounds;
    public AudioClip[] treeFellSounds;

    void Start()
    {
        // load treeSounds from assets/trees
        treeHitSounds = Resources.LoadAll<AudioClip>("tree/hit");
        treeFellSounds = Resources.LoadAll<AudioClip>("tree/fell");
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
    }
    private void PlayRandomSound(AudioClip[] sounds)
    {
        // adjust pitch and volume
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        var clip = sounds[Random.Range(0, sounds.Length)];
        audioSource.PlayOneShot(clip, 0.2f);
    }


    public void PlayHitSound()
    {
        PlayRandomSound(treeHitSounds);
    }

    public void PlayFellSound()
    {
        PlayRandomSound(treeFellSounds);
    }
}
