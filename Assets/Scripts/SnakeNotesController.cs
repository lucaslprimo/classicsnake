using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNotesController : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource player;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine("PlaySound");
    }

    private IEnumerator PlaySound()
    {
        player.clip = GetRandomClip(sounds);
        player.Play();
        Destroy(gameObject, 2);
        yield return null;
    }

    void Update()
    {
        
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        return clips[index];
    }
}
