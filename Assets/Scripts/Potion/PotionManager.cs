using System;
using Unity.VisualScripting;
using UnityEngine;


public class PotionManager : MonoBehaviour
{
    [Header("포션 데이터 가져오기")]
    [SerializeField] private PotionData potionData;
    [Header("렌덤사용")]
    [SerializeField] private bool IsRandomInfo;
    [SerializeField] private bool IsRandomImg;
    [SerializeField] private bool IsRandomType;

    [Header("포션 정보(확인용 수정금지)")]
    [SerializeField] private String potionName;
    [SerializeField] private String potionInfo;
    [SerializeField] private String potionRear;

    [SerializeField] private Color potionColor;
    [SerializeField] private GameObject potionEffect;

    [SerializeField] private float potionAicd;
    [SerializeField] private float potionPosion;

    [Header("포션 외형")]
    [SerializeField] private SpriteRenderer posionRender;

    
    void Awake()
    {
        potionData.InputRndomData(IsRandomInfo,IsRandomImg,IsRandomType);
        GetData();
        
        posionRender.color = potionColor;
    }

    void Update()
    {
        
    }

    private void GetData()
    {
        if (potionData == null)
        {
            Debug.LogWarning("PotionData is not assigned.", this);
            return;
        }

        potionName = potionData.PotionName;
        potionInfo = potionData.PotionInfo;
        potionRear = potionData.PotionRear;

        potionColor = potionData.potinonColor;
        potionEffect = potionData.potinonEffect;

        potionAicd = potionData.Acid;
        potionPosion = potionData.Posion;
    }
    
}
