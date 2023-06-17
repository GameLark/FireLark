using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{

    public AudioClip[] treeHitSounds;
    public AudioClip[] treeFellSounds;

    private AudioSource audioSource;

    void Start()
    {
        // load treeSounds from assets/trees
        treeHitSounds = Resources.LoadAll<AudioClip>("tree/hit");
        treeFellSounds = Resources.LoadAll<AudioClip>("tree/fell");

        // create global AudioSource GameObject
        audioSource = new GameObject("TreeSound").AddComponent<AudioSource>();
    }

    void OnDestroy()
    {
        // wait for the sound to finish playing before destroying the object

        // clip duration 
        float clipDuration = audioSource.clip.length;

        // destroy the object after the clip duration
        Destroy(audioSource.gameObject, clipDuration);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        // adjust pitch and volume
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
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
