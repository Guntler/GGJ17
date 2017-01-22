using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBehavior : MonoBehaviour {
    public AudioClip tensionBGM;
    public AudioClip normalBGM;

    public GameObject Character;
    public GameObject Monster;

    public float MinDangerDist = 5;
    public float FadeOutRate = 1f;

    private AudioClip CurrentSong;

    AudioSource src;

    float audioVolume = 1.0f;

    bool tensionPlaying = false;
    bool switchingSongs = false;

    bool switchedToTension = false;
    bool switchedToNormal = false;

    // Use this for initialization
    void Start () {
        src = GetComponent<AudioSource>();
        CurrentSong = normalBGM;
	}
	
	// Update is called once per frame
	void Update () {
        float dist = Vector3.Distance(Character.transform.position, Monster.transform.position);
        if(dist <= MinDangerDist && !switchedToTension)
        {
            switchedToNormal = false;
            if (!switchingSongs)
                FadeOut();

            if (switchingSongs && !tensionPlaying && audioVolume < 0.01f)
            {
                src.clip = tensionBGM;
                src.Play();
                tensionPlaying = true;
            }

            if(tensionPlaying)
            {
                FadeIn();
                if(audioVolume >= 0.08f)
                {
                    switchingSongs = false;
                    switchedToTension = true;
                }
            }
        }
        else if(dist > MinDangerDist && !switchedToNormal)
        {
            switchedToTension = false;
            if (!switchingSongs)
                FadeOut();

            if (switchingSongs && tensionPlaying && audioVolume < 0.01f)
            {
                src.clip = normalBGM;
                src.Play();
                tensionPlaying = false;
            }

            if (!tensionPlaying)
            {
                FadeIn();
                if (audioVolume >= 0.08f)
                {
                    switchingSongs = false;
                    switchedToNormal = true;
                }
            }
        }
	}

    IEnumerator SwitchSong(AudioClip bgm)
    {
        yield return null;
    }

    void FadeIn()
    {
        if (audioVolume < 0.08)
        {
            audioVolume += FadeOutRate * Time.deltaTime;
            audioVolume = Mathf.Clamp(audioVolume, 0, 0.08f);
            src.volume = audioVolume;
        }
        else
        {
            switchingSongs = false;
        }
    }

    void FadeOut()
    {
        if (audioVolume > 0.01f)
        {
            audioVolume -= FadeOutRate * Time.deltaTime;
            audioVolume = Mathf.Clamp(audioVolume, 0, 0.08f);
            src.volume = audioVolume;
        }
        else
        {
            switchingSongs = true;
        }
    }
}
