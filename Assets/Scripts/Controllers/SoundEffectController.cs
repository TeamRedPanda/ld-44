using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioClip ItemSold;
    public AudioClip ClientLeave;
    public AudioClip ClientEnter;
    public AudioClip ClientDies;

    public GameObject AudioClipPlayerPrefab;

    public static SoundEffectController Instance {
        get {
            if (m_Instance == null) {
                m_Instance = FindObjectOfType<SoundEffectController>();
            }

            return m_Instance;
        }
    }

    private static SoundEffectController m_Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Instance != null)
            Debug.LogError("Cannot have more than 1 SoundEffectControllers in scene.");

        m_Instance = this;
    }

    public static void PlayEffect(EffectType type)
    {
        AudioClip clip = m_Instance.GetAudioClipForType(type);

        if (clip == null)
            return;

        AudioClipPlayer player = Instantiate(m_Instance.AudioClipPlayerPrefab, m_Instance.transform).GetComponent<AudioClipPlayer>();
        player.Play(clip);
    }

    public static void PlayEffect(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioClipPlayer player = Instantiate(m_Instance.AudioClipPlayerPrefab, m_Instance.transform).GetComponent<AudioClipPlayer>();
        player.Play(clip);
    }

    private AudioClip GetAudioClipForType(EffectType type)
    {
        switch (type) {
            case EffectType.ClientDies:
                return ClientDies;
            case EffectType.ClientEnter:
                return ClientEnter;
            case EffectType.ClientLeave:
                return ClientLeave;
            case EffectType.ItemSold:
                return ItemSold;
        }

        throw new ArgumentException("Effect not found.");
    }
}

public enum EffectType
{
    ItemSold,
    ClientLeave,
    ClientEnter,
    ClientDies
}
