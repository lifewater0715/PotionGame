using UnityEngine;

public class PotionState : MonoBehaviour
{
    [Header("포션 상태 정보")]
    [SerializeField] private float potionHeatLv = 0;
    [SerializeField] private float potionHeatSpeed = 0;
    [SerializeField] private float potionMixLv = 0;
    [SerializeField] private float maxPotionHeatLv = 10f;

    [Header("포션 상태")]
    [SerializeField] private bool IsMixing;
    [SerializeField] private bool IsHeating;
    [SerializeField] private bool IsFireing;
    [SerializeField] private bool IsZooming;
    [SerializeField] private bool IsAciding;

    [Header("부글부글")]
    [SerializeField] private GameObject boilAni;
    [SerializeField] private Animator boilAnimator;
    [SerializeField] private float minBoilAnimSpeed = 0.5f;
    [SerializeField] private float maxBoilAnimSpeed = 3f;

    [Header("촛불")]
    [SerializeField] private GameObject candleObj;
    [SerializeField] private GameObject candleLight;
    [SerializeField] private Material candleMat;

    private bool wasHeating;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleHeatStateChange();
        UpdateBoilingState();
        UpdateBoilingVisual();
    }

    private void HandleHeatStateChange()
    {
        if (wasHeating == IsHeating)
        {
            return;
        }

        potionHeatSpeed = 0f;
        wasHeating = IsHeating;
    }

    private void UpdateBoilingState()
    {
        if (IsHeating)
        {
            StartBoiling();
            return;
        }

        if (potionHeatLv > 0f)
        {
            StopBoiling();
            return;
        }

        potionHeatLv = 0f;
        potionHeatSpeed = 0f;
    }

    private void UpdateBoilingVisual()
    {
        bool isBoilingVisible = IsHeating || potionHeatLv > 0f;
        float boilAnimSpeed = GetBoilAnimSpeed();

        boilAni.SetActive(isBoilingVisible);

        if (boilAnimator != null)
        {
            boilAnimator.SetBool("IsBoiling", isBoilingVisible);
            boilAnimator.SetFloat("BoilSpeed", boilAnimSpeed);
        }
    }
    //가열 검사
    private void StartBoiling()
    {
        IncreaseHeatSpeed();
        potionHeatLv += potionHeatSpeed * Time.deltaTime;
        potionHeatLv = Mathf.Min(potionHeatLv, maxPotionHeatLv);
    }

    private void StopBoiling()
    {
        IncreaseHeatSpeed();
        potionHeatLv -= potionHeatSpeed * Time.deltaTime;

        if (potionHeatLv <= 0f)
        {
            potionHeatLv = 0f;
            potionHeatSpeed = 0f;
        }
    }

    private void IncreaseHeatSpeed()
    {
        potionHeatSpeed += Time.deltaTime;
    }

    private float GetBoilAnimSpeed()
    {
        if (!IsHeating && potionHeatLv <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp(potionHeatLv, minBoilAnimSpeed, maxBoilAnimSpeed);
    }

    //촛불 검사
    private void StartBruning()
    {
        
    }

    private void StopBruning()
    {
        
    }

    public void PotionHeating(bool OnHeat)
    {
        IsHeating = OnHeat;
    }

    public void PotionMixing(bool OnMix)
    {
        IsMixing = OnMix;
    }

    public void PotionFireing(bool OnFire)
    {
        IsFireing = OnFire;
    }

    public void PotionZooming(bool OnZoom)
    {
        IsZooming = OnZoom;
    }

    public void PotionAciding(bool OnAcid)
    {
        IsAciding = OnAcid;
    }

}
