using UnityEngine;

public class PotionRule : MonoBehaviour
{
    [Header("검사 대상")]
    [SerializeField] private PotionManager potionManager;
    [SerializeField] private PotionState potionState;

    [Header("검사 결과")]
    [SerializeField] private bool isApproved;
    [SerializeField] private bool isBoiledMatched;
    [SerializeField] private bool isShakedMatched;
    [SerializeField] private bool isCandleMatched;

    public bool CheckPotion()
    {
        PotionData potionData = GetPotionData();
        if (potionData == null || potionState == null)
        {
            Debug.LogWarning("PotionRule: PotionData or PotionState reference is missing.", this);
            isApproved = false;
            return false;
        }

        bool currentBoiled = potionState.IsBoiled();
        bool currentShaked = potionState.IsShaking();
        bool currentCandleChanged = potionState.IsCandleChanged();

        isBoiledMatched = currentBoiled == potionData.OnBoiledGlow;
        isShakedMatched = currentShaked == potionData.OnShakedColorChange;
        isCandleMatched = currentCandleChanged == potionData.OnCandleChange;
        isApproved = isBoiledMatched && isShakedMatched && isCandleMatched;

        return isApproved;
    }

    public bool IsApproved()
    {
        return isApproved;
    }

    public void ResetResult()
    {
        isApproved = false;
        isBoiledMatched = false;
        isShakedMatched = false;
        isCandleMatched = false;
    }

    private PotionData GetPotionData()
    {
        if (potionManager == null)
        {
            potionManager = GetComponent<PotionManager>();
        }

        if (potionState == null)
        {
            potionState = GetComponent<PotionState>();
        }

        return potionManager != null ? potionManager.GetPotionData() : null;
    }
}
