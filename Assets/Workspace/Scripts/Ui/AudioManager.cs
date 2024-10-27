using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    public Toggle muteToggle;
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

        UpdateButtonAndToggleListeners();        

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.value); });
            volumeSlider.value = 1.0f;
        }
    }

    public void PlayClickSound()
    {
        audioSource.Play();
    }
    
    public void MuteToggle(bool muted)
    {
        if(muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void UpdateButtonAndToggleListeners()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(PlayClickSound);
        }

        Toggle[] toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle tg in toggles)
        {
            tg.onValueChanged.RemoveAllListeners();
            tg.onValueChanged.AddListener(delegate { PlayClickSound(); });
        }
    }

    public void UpdateToggleListenersInPanel(GameObject settingsPanel)
    {
        Toggle[] toggles = settingsPanel.GetComponentsInChildren<Toggle>();
        foreach (Toggle tg in toggles)
        {
            tg.onValueChanged.RemoveAllListeners();
            tg.onValueChanged.AddListener(delegate { PlayClickSound(); });
        }
    }

    public void UpdateButtonListenersInPanel(GameObject settingsPanel)
    {
        Button[] buttons = settingsPanel.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { PlayClickSound(); });
        }
    }
}