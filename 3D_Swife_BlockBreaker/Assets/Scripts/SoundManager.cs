using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton

    private AudioSource audioSource; // 효과음을 재생할 Audio source
    private AudioSource BGM; // Camera에서 재생되는 Main BGM
    public AudioClip[] clip = new AudioClip[3]; // shoot, break, click 3가지 효과음

    private void Awake()
    {
        if (instance == null) instance = this; // Singleton
        BGM = Camera.main.GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        // 받아온 이름과 같은 이름의 clip이 있다면 재생
        for (int i = 0; i < clip.Length; i++) if (clip[i].name == name) audioSource.PlayOneShot(clip[i]);
    }

    public void Mute(bool isMuted)
    {
        // 받아온 bool 값을 기반으로 음소거 ON/OFF 설정하기
        audioSource.mute = isMuted;
        BGM.mute = isMuted;
    }
}
