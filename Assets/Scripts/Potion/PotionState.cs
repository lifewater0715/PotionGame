using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PotionState : MonoBehaviour
{
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
    [SerializeField] private float minBoilAnimSpeed = 0.5f;
    [SerializeField] private float maxBoilAnimSpeed = 3f;

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
    private bool wasFireing;
    private bool isBoilingVisualInitialized;
    private bool isBurningVisualInitialized;
    private Color defaultCandleLightColor = Color.white;
    private Color defaultCandleMaterialColor = Color.white;
    private Color defaultCandleSpriteColor = Color.white;

    // Update is called once per frame
    void Update()
    {
        PotionBoiler();
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

        if (boilAni != null && isBoilingVisible && !boilAni.activeSelf)
        {
            boilAni.SetActive(true);
        }

        SetBoilingAlpha(targetAlpha);

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

        if (boilAni == null)
        {
            return false;
        }

        if (boilSpriteRenderer == null)
        {
            boilSpriteRenderer = boilAni.GetComponent<SpriteRenderer>();
        }

        if (boilSpriteRenderer == null)
        {
            return false;
        }

        if (boilAlpa == default)
        {
            boilAlpa = boilSpriteRenderer.color;
        }

        SetBoilingAlpha(0f);
        boilAni.SetActive(false);
        isBoilingVisualInitialized = true;
        return true;
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

}
