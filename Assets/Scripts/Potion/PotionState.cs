using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PotionState : MonoBehaviour
{
    [Header("포션 데이터")]
    [SerializeField] private PotionManager potionManager;

    [Header("포션 상태 정보 가열")]
    [SerializeField] private float potionHeatLv = 0f;
    [SerializeField] private float potionHeatSpeed = 0f;
    [SerializeField] private float maxPotionHeatLv = 10f;

    [Header("포션 상태 정보 섞기")]
    [SerializeField] private float potionMixLv = 0f;
    [SerializeField] private float potionMixSpeed = 0f;
    [SerializeField] private float maxPotionMixLv = 10f;

    [Header("포션 상태 정보 태우기")]
    [SerializeField] private float potionFireLv = 0f;
    [SerializeField] private float potionFireSpeed = 0f;
    [SerializeField] private float maxPotionFierLv = 10f;

    [Header("포션 상태")]
    [SerializeField] private bool IsMixing;
    [SerializeField] private bool IsHeating;
    [SerializeField] private bool IsFireing;
    [SerializeField] private bool IsZooming;
    [SerializeField] private bool IsAciding;

    [Header("끓이기 이팩트")]
    [SerializeField] private Color boilAlpa;
    [SerializeField] private GameObject boilAni;
    [SerializeField] private Animator boilAnimator;
    [SerializeField] private SpriteRenderer boilSpriteRenderer;
    [SerializeField] private SpriteRenderer boilPotionRenderer;
    [SerializeField] private Material boilPotionMaterial;
    [SerializeField] private Color boilTargetMaterialColor = new Color(1f, 0.65f, 0.35f, 1f);
    [SerializeField] private string boilMaterialColorProperty = "_GlowColor";
    [SerializeField] private float boilGlowFadeSpeed = 0.35f;
    [SerializeField] private float minBoilAnimSpeed = 0.5f;
    [SerializeField] private float maxBoilAnimSpeed = 3f;

    [Header("흔들기 이팩트")]
    [SerializeField] private SpriteRenderer potionLiquidRenderer;
    [SerializeField] private Color shakeTargetColor = new Color(0.6f, 1f, 0.75f, 1f);

    [Header("촛불 이팩트")]
    [SerializeField] private GameObject candleObj;
    [SerializeField] private Light2D candleLight;
    [SerializeField] private Material candleMat;
    [SerializeField] private SpriteRenderer candleSpriteRenderer;
    [SerializeField] private Color fireTargetLightColor = new Color(0.35f, 0.7f, 1f, 1f);
    [SerializeField] private Color fireTargetMaterialColor = new Color(0.35f, 0.7f, 1f, 1f);
    [SerializeField] private Color fireTargetSpriteColor = new Color(0.35f, 0.7f, 1f, 1f);
    [SerializeField] private string candleMaterialColorProperty = "_Color";

    private bool wasHeating;
    private bool wasMixing;
    private bool wasFireing;
    private bool isBoilingVisualInitialized;
    private bool isShakeVisualInitialized;
    private bool isBurningVisualInitialized;
    private bool hasLoggedBoilMaterialPropertyWarning;
    private float currentBoilGlowWeight;
    private Color defaultPotionLiquidColor = Color.white;
    private Color defaultBoilMaterialColor = Color.white;
    private Color defaultCandleLightColor = Color.white;
    private Color defaultCandleMaterialColor = Color.white;
    private Color defaultCandleSpriteColor = Color.white;

    // Update is called once per frame
    void Update()
    {
        PotionBoiler();
        PotionMixer();
        PotionBurner();
    }

    private void PotionBoiler()
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
        if (!EnsureBoilingVisualInitialized())
        {
            return;
        }

        float targetAlpha = GetBoilingAlpha();
        bool isBoilingVisible = targetAlpha > 0f;
        float boilAnimSpeed = GetBoilAnimSpeed();
        float boilWeight = UpdateBoilGlowWeight();

        if (boilAni != null && isBoilingVisible && !boilAni.activeSelf)
        {
            boilAni.SetActive(true);
        }

        SetBoilingAlpha(targetAlpha);
        UpdateBoilingMaterialColor(CanApplyBoilingVisual() ? boilWeight : 0f);

        if (boilAni != null && !isBoilingVisible && boilAni.activeSelf)
        {
            boilAni.SetActive(false);
        }

        if (boilAnimator != null)
        {
            boilAnimator.SetBool("IsBoiling", isBoilingVisible);
            boilAnimator.SetFloat("BoilSpeed", boilAnimSpeed);
        }
    }

    private bool EnsureBoilingVisualInitialized()
    {
        if (isBoilingVisualInitialized)
        {
            return true;
        }

        if (boilAni != null && boilSpriteRenderer == null)
        {
            boilSpriteRenderer = boilAni.GetComponent<SpriteRenderer>();
        }

        if (boilSpriteRenderer != null && boilAlpa == default)
        {
            boilAlpa = boilSpriteRenderer.color;
        }

        if (potionManager == null)
        {
            potionManager = GetComponent<PotionManager>();
        }

        if (boilPotionRenderer == null && potionManager != null)
        {
            boilPotionRenderer = potionManager.GetPotionRenderer();
        }

        if (boilPotionRenderer == null)
        {
            boilPotionRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (boilPotionRenderer != null)
        {
            if (boilPotionMaterial != null)
            {
                boilPotionRenderer.material = boilPotionMaterial;
            }

            boilPotionMaterial = boilPotionRenderer.material;
        }

        if (boilPotionMaterial != null && boilPotionMaterial.HasProperty(boilMaterialColorProperty))
        {
            defaultBoilMaterialColor = boilPotionMaterial.GetColor(boilMaterialColorProperty);
        }

        if (boilSpriteRenderer != null)
        {
            SetBoilingAlpha(0f);
        }

        currentBoilGlowWeight = 0f;
        UpdateBoilingMaterialColor(0f);

        if (boilAni != null)
        {
            boilAni.SetActive(false);
        }

        isBoilingVisualInitialized = true;
        return boilSpriteRenderer != null || boilPotionMaterial != null;
    }
    
    private void StartBoiling()
    {
        potionHeatLv = IncreaseWeightedValue(potionHeatLv, ref potionHeatSpeed, maxPotionHeatLv);
    }

    private void StopBoiling()
    {
        potionHeatLv = DecreaseWeightedValue(potionHeatLv, ref potionHeatSpeed);
    }

    private float GetBoilAnimSpeed()
    {
        if (!IsHeating && potionHeatLv <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp(potionHeatLv, minBoilAnimSpeed, maxBoilAnimSpeed);
    }

    private float GetBoilingAlpha()
    {
        if (maxPotionHeatLv <= 0f)
        {
            return 0f;
        }

        float targetAlpha = boilAlpa.a;

        if (targetAlpha <= 0f)
        {
            targetAlpha = 1f;
        }

        return Mathf.Clamp01(potionHeatLv / maxPotionHeatLv) * targetAlpha;
    }

    private float GetBoilingWeight()
    {
        if (maxPotionHeatLv <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp01(potionHeatLv / maxPotionHeatLv);
    }

    private float UpdateBoilGlowWeight()
    {
        float targetWeight = GetBoilingWeight();

        if (targetWeight > currentBoilGlowWeight)
        {
            currentBoilGlowWeight = targetWeight;
            return currentBoilGlowWeight;
        }

        if (boilGlowFadeSpeed <= 0f)
        {
            currentBoilGlowWeight = targetWeight;
            return currentBoilGlowWeight;
        }

        currentBoilGlowWeight = Mathf.MoveTowards(
            currentBoilGlowWeight,
            targetWeight,
            boilGlowFadeSpeed * Time.deltaTime
        );
        return currentBoilGlowWeight;
    }

    private void SetBoilingAlpha(float alpha)
    {
        if (boilSpriteRenderer == null)
        {
            return;
        }

        Color boilColor = boilAlpa;
        boilColor.a = alpha;
        boilSpriteRenderer.color = boilColor;
    }

    private void UpdateBoilingMaterialColor(float boilWeight)
    {
        if (boilPotionMaterial == null)
        {
            return;
        }

        if (!boilPotionMaterial.HasProperty(boilMaterialColorProperty))
        {
            if (!hasLoggedBoilMaterialPropertyWarning)
            {
                Debug.LogWarning(
                    $"PotionState: Material '{boilPotionMaterial.name}' does not have property '{boilMaterialColorProperty}'.",
                    this
                );
                hasLoggedBoilMaterialPropertyWarning = true;
            }

            return;
        }

        boilPotionMaterial.SetColor(
            boilMaterialColorProperty,
            Color.Lerp(defaultBoilMaterialColor, boilTargetMaterialColor, boilWeight)
        );
    }

    private void PotionBurner()
    {
        HandleFireStateChange();
        UpdateBurningState();
        UpdateBurningVisual();
    }

    private void HandleFireStateChange()
    {
        if (wasFireing == IsFireing)
        {
            return;
        }

        potionFireSpeed = 0f;
        wasFireing = IsFireing;
    }

    private void UpdateBurningState()
    {
        if (IsFireing)
        {
            StartBruning();
            return;
        }

        if (potionFireLv > 0f)
        {
            StopBruning();
            return;
        }

        potionFireLv = 0f;
        potionFireSpeed = 0f;
    }

    private void UpdateBurningVisual()
    {
        if (!EnsureBurningVisualInitialized())
        {
            return;
        }

        if (!CanApplyCandleVisual())
        {
            ResetBurningVisual();
            return;
        }

        float fireWeight = GetBurningWeight();

        if (candleLight != null)
        {
            candleLight.color = Color.Lerp(defaultCandleLightColor, fireTargetLightColor, fireWeight);
        }

        if (candleMat != null && candleMat.HasProperty(candleMaterialColorProperty))
        {
            candleMat.SetColor(
                candleMaterialColorProperty,
                Color.Lerp(defaultCandleMaterialColor, fireTargetMaterialColor, fireWeight)
            );
        }

        if (candleSpriteRenderer != null)
        {
            candleSpriteRenderer.color = Color.Lerp(defaultCandleSpriteColor, fireTargetSpriteColor, fireWeight);
        }
    }

    private void PotionMixer()
    {
        HandleMixStateChange();
        UpdateMixingState();
        UpdateMixingVisual();
    }

    private void HandleMixStateChange()
    {
        if (wasMixing == IsMixing)
        {
            return;
        }

        potionMixSpeed = 0f;
        wasMixing = IsMixing;
    }

    private void UpdateMixingState()
    {
        if (IsMixing)
        {
            StartMixing();
            return;
        }

        if (potionMixLv > 0f)
        {
            StopMixing();
            return;
        }

        potionMixLv = 0f;
        potionMixSpeed = 0f;
    }

    private void UpdateMixingVisual()
    {
        if (!EnsureShakeVisualInitialized())
        {
            return;
        }

        if (!CanApplyShakeVisual())
        {
            ResetMixingVisual();
            return;
        }

        float mixWeight = GetMixingWeight();
        potionLiquidRenderer.color = Color.Lerp(defaultPotionLiquidColor, shakeTargetColor, mixWeight);
    }

    private bool EnsureShakeVisualInitialized()
    {
        if (isShakeVisualInitialized)
        {
            return true;
        }

        if (potionLiquidRenderer == null && potionManager != null)
        {
            potionLiquidRenderer = potionManager.GetPotionRenderer();
        }

        if (potionLiquidRenderer == null)
        {
            potionLiquidRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (potionLiquidRenderer == null)
        {
            return false;
        }

        defaultPotionLiquidColor = potionLiquidRenderer.color;
        isShakeVisualInitialized = true;
        return true;
    }

    private bool EnsureBurningVisualInitialized()
    {
        if (isBurningVisualInitialized)
        {
            return true;
        }

        AutoSetCandleReferences();

        if (candleLight != null)
        {
            defaultCandleLightColor = candleLight.color;
        }

        if (candleSpriteRenderer == null && candleObj != null)
        {
            candleSpriteRenderer = candleObj.GetComponent<SpriteRenderer>();
        }

        if (candleSpriteRenderer != null)
        {
            defaultCandleSpriteColor = candleSpriteRenderer.color;
        }

        if (candleMat != null && candleMat.HasProperty(candleMaterialColorProperty))
        {
            defaultCandleMaterialColor = candleMat.GetColor(candleMaterialColorProperty);
        }

        isBurningVisualInitialized = true;
        return true;
    }

    private void AutoSetCandleReferences()
    {
        if (candleObj == null)
        {
            candleObj = GameObject.FindGameObjectWithTag("Heat");
        }

        if (candleObj == null)
        {
            return;
        }

        if (candleLight == null)
        {
            candleLight = candleObj.GetComponentInChildren<Light2D>();
        }

        if (candleSpriteRenderer == null)
        {
            candleSpriteRenderer = candleObj.GetComponentInChildren<SpriteRenderer>();
        }

        if (candleMat == null)
        {
            SpriteRenderer targetRenderer = candleSpriteRenderer;

            if (targetRenderer == null)
            {
                targetRenderer = candleObj.GetComponentInChildren<SpriteRenderer>();
            }

            if (targetRenderer != null)
            {
                candleMat = targetRenderer.material;
            }
        }
    }

    private bool CanApplyBoilingVisual()
    {
        PotionData potionData = GetPotionData();
        return potionData == null || potionData.OnBoiledGlow;
    }

    private bool CanApplyCandleVisual()
    {
        PotionData potionData = GetPotionData();
        return potionData == null || potionData.OnCandleChange;
    }

    private bool CanApplyShakeVisual()
    {
        PotionData potionData = GetPotionData();
        return potionData == null || potionData.OnShakedColorChange;
    }

    private PotionData GetPotionData()
    {
        if (potionManager == null)
        {
            potionManager = GetComponent<PotionManager>();
        }

        return potionManager != null ? potionManager.GetPotionData() : null;
    }

    private void SetBoilingInactive()
    {
        SetBoilingAlpha(0f);
        UpdateBoilingMaterialColor(0f);

        if (boilAni != null && boilAni.activeSelf)
        {
            boilAni.SetActive(false);
        }

        if (boilAnimator != null)
        {
            boilAnimator.SetBool("IsBoiling", false);
            boilAnimator.SetFloat("BoilSpeed", 0f);
        }
    }

    private void ResetBurningVisual()
    {
        if (candleLight != null)
        {
            candleLight.color = defaultCandleLightColor;
        }

        if (candleMat != null && candleMat.HasProperty(candleMaterialColorProperty))
        {
            candleMat.SetColor(candleMaterialColorProperty, defaultCandleMaterialColor);
        }

        if (candleSpriteRenderer != null)
        {
            candleSpriteRenderer.color = defaultCandleSpriteColor;
        }
    }

    private void StartMixing()
    {
        potionMixLv = IncreaseWeightedValue(potionMixLv, ref potionMixSpeed, maxPotionMixLv);
    }

    private void StopMixing()
    {
        potionMixLv = DecreaseWeightedValue(potionMixLv, ref potionMixSpeed);
    }

    private float GetMixingWeight()
    {
        if (maxPotionMixLv <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp01(potionMixLv / maxPotionMixLv);
    }

    private void ResetMixingVisual()
    {
        if (potionLiquidRenderer == null)
        {
            return;
        }

        potionLiquidRenderer.color = defaultPotionLiquidColor;
    }

    private float IncreaseWeightedValue(float currentValue, ref float currentSpeed, float maxValue)
    {
        currentSpeed += Time.deltaTime;
        currentValue += currentSpeed * Time.deltaTime;
        return Mathf.Min(currentValue, maxValue);
    }

    private float DecreaseWeightedValue(float currentValue, ref float currentSpeed)
    {
        currentSpeed += Time.deltaTime;
        currentValue -= currentSpeed * Time.deltaTime;

        if (currentValue <= 0f)
        {
            currentSpeed = 0f;
            return 0f;
        }

        return currentValue;
    }

    //촛불 검사
    private void StartBruning()
    {
        potionFireLv = IncreaseWeightedValue(potionFireLv, ref potionFireSpeed, maxPotionFierLv);
    }

    private void StopBruning()
    {
        potionFireLv = DecreaseWeightedValue(potionFireLv, ref potionFireSpeed);
    }

    private float GetBurningWeight()
    {
        if (maxPotionFierLv <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp01(potionFireLv / maxPotionFierLv);
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

    public bool IsBoiled()
    {
        return potionHeatLv > 0f || IsHeating;
    }

    public bool IsShaking()
    {
        return IsMixing;
    }

    public bool IsCandleChanged()
    {
        return potionFireLv > 0f || IsFireing;
    }

}
