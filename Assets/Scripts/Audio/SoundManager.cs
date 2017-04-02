using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : AManager<SoundManager> {

    public GameObject audioSourceSingle;
    public GameObject audioSourceSingleRandomPitch;
    public GameObject audioSourceMusic;
    public GameObject audioSourceAcid;

    public AudioClip jump;
    public AudioClip acid_intro;
    public AudioClip acid_loop;
    public AudioClip Shotgun;
    public AudioClip bullet;
    public AudioClip game_over;
    public AudioClip piece;
    public AudioClip darkness;
    public AudioClip dying;
    public AudioClip explosion;
    public AudioClip mähHüh;
    public AudioClip gravity;

    public List<GameObject> currentlyPlaying;
    public Dictionary<Sound, AudioClip> soundDic;

    public enum Sound
    {
        Acid,
        Dying,
        GameOver,
        Piece,
        Darkness,
        Jump,
        Bullet,
        Shotgun,
        Explosion,
        MähHüh,
        Gravity
    };

    // Use this for initialization
    void Start() {
        soundDic = new Dictionary<Sound, AudioClip>();
        soundDic.Add(Sound.Dying, dying);
        soundDic.Add(Sound.GameOver, game_over);
        soundDic.Add(Sound.Piece, piece);
        soundDic.Add(Sound.Darkness, darkness);
        soundDic.Add(Sound.Jump, jump);
        soundDic.Add(Sound.Bullet, bullet);
        soundDic.Add(Sound.Shotgun, Shotgun);
        soundDic.Add(Sound.Explosion, explosion);
        soundDic.Add(Sound.MähHüh, mähHüh);
        soundDic.Add(Sound.Gravity, gravity);

        currentlyPlaying = new List<GameObject>();

        Instantiate(audioSourceMusic);
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void OnAwake()
    {

    }

    public void StartSingleSound(Sound sound, float volume = 1f)
    {
        GameObject audioSource = Instantiate(audioSourceSingle);
        AudioSource ac = audioSource.GetComponent<AudioSource>();
        ac.clip = soundDic[sound];
        ac.volume = volume;
        ac.Play();
        //currentlyPlaying.Add(audioSource);
    }

    public void StartSingleSoundRandomPitch(Sound sound, float volume = 1f)
    {
        GameObject audioSource = Instantiate(audioSourceSingleRandomPitch);
        AudioSource ac = audioSource.GetComponent<AudioSource>();
        ac.clip = soundDic[sound];
        ac.volume = volume;
        ac.Play();
    }

    public void StartAcid()
    {
        GameObject audioSource = Instantiate(audioSourceAcid);
        audioSource.name = "AssetAudio";
        currentlyPlaying.Add(audioSource);
    }

    public void EndAcid()
    {
        Destroy(currentlyPlaying.Find(a => a.name == "AssetAudio"));
    }
}