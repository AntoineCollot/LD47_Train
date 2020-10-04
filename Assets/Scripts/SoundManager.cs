using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct SoundClip
    {
        [Range(0,1)] public float volume;
        public AudioClip clip;
    }

    /*
     * 0 Rail
     * 1 Remove Rail
     * 2 Can't rail
     * 3 Passenger spawn
     * 4 Passenger save
     * 5 bandit spawn
     * 6 Hazard start
     * 7 Station Spawn
     */
    [SerializeField] SoundClip[] clips=null;

    new AudioSource audio;
    public static SoundManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void Play(int id)
    {
        audio.PlayOneShot(clips[id].clip, clips[id].volume);
    }
}
