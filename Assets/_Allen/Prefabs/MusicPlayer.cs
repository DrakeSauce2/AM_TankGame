using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer1 : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [Space]
    [SerializeField] private List<AudioClip> musicClips;

    private void Awake()
    {
        musicSource.clip = musicClips[Random.Range(0, musicClips.Count - 1)];
        musicSource.Play();
    }

}
