using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float defaultVolume = 0.7f;

    private Button button;
    private AudioSource audioSource;
    private VolumeSystem volumeSystem;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        volumeSystem = FindFirstObjectByType<VolumeSystem>();

        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        if (clickSound == null || audioSource == null) return;

        // VolumeSystem의 Public 메서드 사용
        float volume = volumeSystem != null ? volumeSystem.GetSFXVolume() : defaultVolume;

        audioSource.PlayOneShot(clickSound, volume);
    }
}