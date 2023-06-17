using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{

    public AudioClip[] treeHitSounds;
    public AudioClip[] treeFellSounds;

    void Start()
    {
        // load treeSounds from assets/trees
        treeHitSounds = Resources.LoadAll<AudioClip>("tree/hit");
        treeFellSounds = Resources.LoadAll<AudioClip>("tree/fell");
    }
    private void PlayRandomSound(AudioClip[] sounds)
    {

        // create global audio source
        AudioSource audioSource = new GameObject("treeSound").AddComponent<AudioSource>();

        // adjust pitch and volume
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Random.Range(0.8f, 1.2f);

        var clip = sounds[Random.Range(0, sounds.Length)];

        audioSource.PlayOneShot(clip);

        // destroy audio source
        Destroy(audioSource, clip.length);
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
