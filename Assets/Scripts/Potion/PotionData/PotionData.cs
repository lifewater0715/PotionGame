using System;
using System.Collections.Generic;
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
        }
    }
}
