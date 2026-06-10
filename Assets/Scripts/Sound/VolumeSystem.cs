using UnityEngine;
using UnityEngine.UI;

// ==================================================
// CompleteVolumeSystem 클래스
// 역할: BGM과 SFX 볼륨을 각각 독립적으로 제어
// ==================================================
public class VolumeSystem : MonoBehaviour
{
    [Header("UI 요소")]
    public Slider bgmVolumeSlider;      // BGM 전용 슬라이더
    public Slider sfxVolumeSlider;      // SFX 전용 슬라이더
    public Text bgmVolumeText;          // BGM 볼륨 퍼센트 표시
    public Text sfxVolumeText;          // SFX 볼륨 퍼센트 표시
    public Button muteButton;           // 전체 음소거 버튼

    [Header("오디오")]
    public AudioSource bgmAudio;        // 배경음악 AudioSource
    public AudioSource[] sfxAudios;     // 효과음 AudioSource들

    // 각각의 볼륨을 따로 저장
    private float lastBGMVolume = 0.7f;  // 마지막 BGM 볼륨
    private float lastSFXVolume = 0.7f;  // 마지막 SFX 볼륨
    private bool isMuted = false;        // 음소거 상태

    // ==================================================
    // Awake() - 게임 시작 시 처음 실행
    // 역할: 저장된 볼륨 불러오기 및 초기 설정
    // ==================================================
    void Awake()
    {

        DontDestroyOnLoad(gameObject);
        // PlayerPrefs에서 저장된 볼륨 불러오기
        // 저장된 값이 없으면 기본값 0.7(70%) 사용 1퍼로 수정
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // 마지막 볼륨 값 저장 (음소거 해제 시 복원용)
        lastBGMVolume = savedBGMVolume;
        lastSFXVolume = savedSFXVolume;

        // ===== BGM 슬라이더 설정 =====
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.minValue = 0f;           // 최소값 0
            bgmVolumeSlider.maxValue = 1f;           // 최대값 1
            bgmVolumeSlider.value = savedBGMVolume;  // 현재값 설정
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);  // 이벤트 연결
        }

        // ===== SFX 슬라이더 설정 =====
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.minValue = 0f;
            sfxVolumeSlider.maxValue = 1f;
            sfxVolumeSlider.value = savedSFXVolume;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        // ===== 음소거 버튼 설정 =====
        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);

        // 초기 볼륨 적용
        ApplyBGMVolume(savedBGMVolume);
        ApplySFXVolume(savedSFXVolume);

        // 볼륨 텍스트 업데이트
        UpdateBGMVolumeText(savedBGMVolume);
        UpdateSFXVolumeText(savedSFXVolume);
    }

    // ==================================================
    // OnBGMVolumeChanged() 함수
    // 역할: BGM 슬라이더를 움직일 때 호출됨
    // 매개변수: volume - 새로운 BGM 볼륨값 (0~1)
    // ==================================================
    void OnBGMVolumeChanged(float volume)
    {
        // 음소거 상태가 아닐 때만 볼륨 변경
        if (!isMuted)
        {
            ApplyBGMVolume(volume);
            lastBGMVolume = volume;  // 마지막 볼륨 저장
            PlayerPrefs.SetFloat("BGMVolume", volume);  // 저장
            UpdateBGMVolumeText(volume);  // 텍스트 업데이트
        }
    }

    // ==================================================
    // OnSFXVolumeChanged() 함수
    // 역할: SFX 슬라이더를 움직일 때 호출됨
    // 매개변수: volume - 새로운 SFX 볼륨값 (0~1)
    // ==================================================
    void OnSFXVolumeChanged(float volume)
    {
        if (!isMuted)
        {
            ApplySFXVolume(volume);
            lastSFXVolume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
            UpdateSFXVolumeText(volume);
        }
    }

    // ==================================================
    // ApplyBGMVolume() 함수
    // 역할: BGM 볼륨을 실제로 적용
    // ==================================================
    void ApplyBGMVolume(float volume)
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = volume;
            Debug.Log("BGM 볼륨 설정: " + volume);
        }
    }

    // ==================================================
    // ApplySFXVolume() 함수
    // 역할: 모든 SFX 볼륨을 실제로 적용
    // ==================================================
    void ApplySFXVolume(float volume)
    {
        foreach (AudioSource sfx in sfxAudios)
        {
            if (sfx != null)
            {
                sfx.volume = volume;
            }
        }
        Debug.Log("SFX 볼륨 설정: " + volume);
    }

    // ==================================================
    // UpdateBGMVolumeText() 함수
    // 역할: BGM 볼륨 퍼센트를 화면에 표시
    // ==================================================
    void UpdateBGMVolumeText(float volume)
    {
        if (bgmVolumeText != null)
        {
            int percent = Mathf.RoundToInt(volume * 100); 
            bgmVolumeText.text = "BGM: " + percent + "%";
        }
    }

    // ==================================================
    // UpdateSFXVolumeText() 함수
    // 역할: SFX 볼륨 퍼센트를 화면에 표시
    // ==================================================
    void UpdateSFXVolumeText(float volume)
    {
        if (sfxVolumeText != null)
        {
            int percent = Mathf.RoundToInt(volume * 100);
            sfxVolumeText.text = "SFX: " + percent + "%";
        }
    }

    // ==================================================
    // ToggleMute() 함수
    // 역할: 음소거 버튼 클릭 시 전체 음소거 ON/OFF
    // ==================================================
    void ToggleMute()
    {
        isMuted = !isMuted;  // 상태 반전

        if (isMuted)
        {
            // ===== 음소거 ON: 모든 볼륨 0으로 =====
            ApplyBGMVolume(0f);
            ApplySFXVolume(0f);

            // 버튼 텍스트 변경
            if (muteButton != null)
            {
                Text buttonText = muteButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                    buttonText.text = "음소거 해제";
            }
            Debug.Log("전체 음소거 ON");
        }
        else
        {
            // ===== 음소거 OFF: 마지막 볼륨으로 복원 =====
            ApplyBGMVolume(lastBGMVolume);
            ApplySFXVolume(lastSFXVolume);

            // 버튼 텍스트 변경
            if (muteButton != null)
            {
                Text buttonText = muteButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                    buttonText.text = "음소거";
            }

            // 텍스트 업데이트
            UpdateBGMVolumeText(lastBGMVolume);
            UpdateSFXVolumeText(lastSFXVolume);
            Debug.Log("전체 음소거 OFF");
        }
    }

    // ==================================================
    // 외부에서 호출할 수 있는 Public 메서드들
    // ==================================================

    // BGM 볼륨 가져오기
    public float GetBGMVolume()
    {
        return bgmAudio != null ? bgmAudio.volume : 0f;
    }

    // SFX 볼륨 가져오기
    public float GetSFXVolume()
    {
        return sfxAudios.Length > 0 && sfxAudios[0] != null ? sfxAudios[0].volume : 0f;
    }

    // BGM 볼륨 설정하기 (외부에서 직접 설정)
    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);  // 0~1 범위로 제한
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.value = volume;
        else
            OnBGMVolumeChanged(volume);
    }

    // SFX 볼륨 설정하기 (외부에서 직접 설정)
    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = volume;
        else
            OnSFXVolumeChanged(volume);
    }

    // 음소거 상태 확인
    public bool IsMuted()
    {
        return isMuted;
    }
}