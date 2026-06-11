using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "Scriptable Objects/PotionData")]
public class PotionData : ScriptableObject
{
    [Header ("렌덤 데이터")]
    public RandomPotionData RandData;
    [Header ("포션 정보")]
    public string PotionName;
    public string PotionInfo;
    public string PotionRear; //nomal,epic,legund

    [Header ("포션 외형")]
    public Color potinonColor;
    public GameObject potinonEffect;

    [Header ("포션 속성")]
    public float Acid;
    public float Posion;

    [Header ("포션 작업 결과")]
    public bool OnBoiledGlow;
    public bool OnShakedColorChange;
    public bool OnCandleChange;

    [Header ("랜덤 속성")]
    public bool OnrandomInfo;

    [Header ("랜덤 외형")]
    public bool OnrandomLiquid;

    [Header ("랜덤 속성")]
    public bool OnrandomType;

    public void InputRndomData(bool OnRandomLiquid, bool OnrandomInfo, bool OnRandomType)
    {
        if(OnrandomInfo)
        {
            PotionName = RandData.RandomPotionName[UnityEngine.Random.Range(0,RandData.RandomPotionName.Count)];
            PotionInfo = RandData.RandomPotionInfo[UnityEngine.Random.Range(0,RandData.RandomPotionInfo.Count)];
            PotionRear = RandData.RandomPotionRear[UnityEngine.Random.Range(0,RandData.RandomPotionRear.Count)];
        }

        if(OnRandomLiquid)
        {
            potinonColor = RandData.RandomPotinonColor[UnityEngine.Random.Range(0,RandData.RandomPotinonColor.Count)];
            potinonEffect = RandData.RandomPotinonEffect[UnityEngine.Random.Range(0,RandData.RandomPotinonEffect.Count)];
        }

        if(OnRandomType)
        {
            Acid = RandData.RandomPotinonAcid[UnityEngine.Random.Range(0,RandData.RandomPotinonAcid.Count)];
            Posion = RandData.RandomPotinonPosion[UnityEngine.Random.Range(0,RandData.RandomPotinonPosion.Count)];
            OnBoiledGlow = GetRandomBool(RandData.RandomPotionBoiledGlow, OnBoiledGlow);
            OnShakedColorChange = GetRandomBool(RandData.RandomPotionShakedColorChange, OnShakedColorChange);
            OnCandleChange = GetRandomBool(RandData.RandomPotionCandleChange, OnCandleChange);
        }
    }

    private bool GetRandomBool(bool[] randomValues, bool fallbackValue)
    {
        if (randomValues == null || randomValues.Length == 0)
        {
            return fallbackValue;
        }

        return randomValues[UnityEngine.Random.Range(0, randomValues.Length)];
    }

    private bool GetRandomBool(System.Collections.Generic.List<bool> randomValues, bool fallbackValue)
    {
        if (randomValues == null || randomValues.Count == 0)
        {
            return fallbackValue;
        }

        return randomValues[UnityEngine.Random.Range(0, randomValues.Count)];
    }
}
