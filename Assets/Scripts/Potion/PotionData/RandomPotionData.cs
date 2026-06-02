using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomPotionData", menuName = "Scriptable Objects/RandomPotionData")]
public class RandomPotionData : ScriptableObject
{
    [Header ("랜덤 정보 속성")]
    public List<String> RandomPotionName = new List<string>();
    public List<String> RandomPotionInfo = new List<string>();
    public List<String> RandomPotionRear = new List<string>();

    [Header ("랜덤 외형 속성")]
    public List<Color> RandomPotinonColor = new List<Color>();
    public List<GameObject> RandomPotinonEffect = new List<GameObject>();

    [Header ("랜덤 특징 속성")]
    public List<float> RandomPotinonAcid = new List<float>();
    public List<float> RandomPotinonPosion = new List<float>();
}
