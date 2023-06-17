using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitRandomSound : MonoBehaviour
{
    public AudioClip[] sounds;
    public float secTimeBetweenSounds = 5f;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound() {
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        yield return new WaitForSeconds(secTimeBetweenSounds);
        audioSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
