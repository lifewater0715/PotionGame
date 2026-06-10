using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundWithVolume : MonoBehaviour
{
    public AudioClip clickSound;

    private VolumeSystem volumeSystem;

    void Start()
    {
        volumeSystem = FindFirstObjectByType<VolumeSystem>();

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                if (clickSound != null)
                {
                    float volume = 0.7f;

                    // eVolumeSystem의 볼륨 가져오기
                    if (volumeSystem != null && volumeSystem.sfxAudios != null && volumeSystem.sfxAudios.Length > 0)
                    {
                        if (volumeSystem.sfxAudios[0] != null)
                        {
                            volume = volumeSystem.sfxAudios[0].volume;
                        }
                    }

                    AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
                }
            });
        }
    }
}