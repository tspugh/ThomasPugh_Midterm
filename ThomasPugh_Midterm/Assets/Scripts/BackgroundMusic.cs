using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    public Soundtrack bass;
    public Soundtrack upper;

    public float transitionTime = 1f;
    private float loopTime = 31.8f;
    private float keepTrackTime = 0f;

    private AudioSource bass1, bass2, upper1, upper2;

    private bool playingMain;

    private SoundtrackType currentSound = SoundtrackType.Intro;

    private void Awake()
    {
        GameEvents.ChangeSoundtrack += OnSoundChange;
    }

    public void Start()
    {
        bass1 = gameObject.AddComponent<AudioSource>();
        bass2 = gameObject.AddComponent<AudioSource>();
        upper1 = gameObject.AddComponent<AudioSource>();
        upper2 = gameObject.AddComponent<AudioSource>();
        playingMain = true;
        SwapTrack(SoundtrackType.Intro);

    }

    public void OnSoundChange(object sender, SoundArgs e)
    {
        SwapTrack(e.newSoundtrack);
    }

    public void SwapTrack(SoundtrackType sound)
    {
        StopAllCoroutines();
        currentSound = sound;
        StartCoroutine(FadeTrack(sound));
        playingMain = !playingMain;
    }

    public void RestartTrack(SoundtrackType sound)
    {
        StopAllCoroutines();
        currentSound = sound;
        SortaFadeTrack(sound);
        playingMain = !playingMain;
    }

    public IEnumerator FadeTrack(SoundtrackType sound)
    {
        float time = 0;
        AudioClip bassNewClip = bass.GetTrack(sound);
        AudioClip upperNewClip = upper.GetTrack(sound);
        if (playingMain)
        {

            bass2.clip = bassNewClip;
            upper2.clip = upperNewClip;
            bass2.time = keepTrackTime;
            upper2.time = keepTrackTime;
            bass2.Play();
            upper2.Play();

            while (time < transitionTime)
            {
                bass2.volume = Mathf.Lerp(0, 1, time / transitionTime);
                upper2.volume = Mathf.Lerp(0, 1, time / transitionTime);
                bass1.volume = Mathf.Lerp(1, 0, time / transitionTime);
                upper1.volume = Mathf.Lerp(1, 0, time / transitionTime);
                time += Time.deltaTime;
                yield return null;
            }

            bass1.Stop();
            upper1.Stop();
        }
        else
        {
            bass1.clip = bassNewClip;
            upper1.clip = upperNewClip;
            bass1.time = keepTrackTime;
            upper1.time = keepTrackTime;
            bass1.Play();
            upper1.Play();

            while (time < transitionTime)
            {
                bass1.volume = Mathf.Lerp(0, 1, time / transitionTime);
                upper1.volume = Mathf.Lerp(0, 1, time / transitionTime);
                bass2.volume = Mathf.Lerp(1, 0, time / transitionTime);
                upper2.volume = Mathf.Lerp(1, 0, time / transitionTime);
                time += Time.deltaTime;
                yield return null;
            }

            bass2.Stop();
            upper2.Stop();
        }
    }

    public void SortaFadeTrack(SoundtrackType sound)
    {
        AudioClip bassNewClip = bass.GetTrack(sound);
        AudioClip upperNewClip = upper.GetTrack(sound);
        if (playingMain)
        {

            bass2.clip = bassNewClip;
            upper2.clip = upperNewClip;
            bass2.Play();
            upper2.Play();

            bass2.volume = 1;
            upper2.volume = 1;
        }
        else
        {
            bass1.clip = bassNewClip;
            upper1.clip = upperNewClip;
            bass1.Play();
            upper1.Play();

            bass1.volume = 1;
            upper1.volume = 1;
        }
    }

    public void Update()
    {

        keepTrackTime += Time.deltaTime;
        if (keepTrackTime >= loopTime)
        {
            keepTrackTime = 0;
            if (currentSound == SoundtrackType.Intro)
                currentSound = SoundtrackType.Menu;
            RestartTrack(currentSound);
        }
    }
}
