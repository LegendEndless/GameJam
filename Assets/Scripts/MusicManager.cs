using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip solarStormClip;
    public List<AudioClip> ambientClips;
    static public MusicManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayRandomAmbientMusic();
    }

    public void PlayRandomAmbientMusic()
    {
        if (SolarStormManager.Instance.isInStorm) return;
        if (ambientClips.Count > 0)
        {
            AudioClip clip = ambientClips[Random.Range(0, ambientClips.Count)];
            audioSource.clip = clip;
            audioSource.Play();
            Invoke(nameof(PlayRandomAmbientMusic), clip.length);
        }
    }

    public void PlaySolarStormMusic()
    {
        audioSource.clip = solarStormClip;
        audioSource.Play();
    }
}