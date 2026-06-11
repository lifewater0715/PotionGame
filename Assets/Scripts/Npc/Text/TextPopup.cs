using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextPopup : MonoBehaviour
{
    private enum DialogueType
    {
        Default,
        Approved,
        Rejected
    }

    [Header("텍스트 팝업")]
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("기본 대사")]
    [SerializeField] private List<string> defaultSentences = new List<string>()
    {
        "Hello!",
        "Nice to meet you.",
        "How can I help you?"
    };

    [Header("허가 대사")]
    [SerializeField] private List<string> approvedSentences = new List<string>();

    [Header("비허가 대사")]
    [SerializeField] private List<string> rejectedSentences = new List<string>();

    private string currentSentence;
    private float typeTimer;
    private int typeIndex;
    private bool typing;

    void Awake()
    {
        if (popupObject == null)
        {
            popupObject = gameObject;
        }

        if (textUI == null)
        {
            textUI = popupObject.GetComponentInChildren<TextMeshProUGUI>(true);
        }
    }

    void OnEnable()
    {
        typeTimer = 0f;
        typeIndex = 0;
        typing = false;
    }

    public void Show()
    {
        ShowDefault();
    }

    public void ShowDefault()
    {
        ShowByType(DialogueType.Default);
    }

    public void ShowApproved()
    {
        ShowByType(DialogueType.Approved);
    }

    public void ShowRejected()
    {
        ShowByType(DialogueType.Rejected);
    }

    private void ShowByType(DialogueType dialogueType)
    {
        if (textUI == null)
        {
            Debug.LogWarning("TextPopup.Show failed: TextMeshProUGUI reference is missing.", this);
            return;
        }

        if (!HasAvailableSentenceSource(dialogueType))
        {
            Debug.LogWarning("TextPopup.Show failed: no dialogue data is available.", this);
            return;
        }

        Debug.Log("TextPopup Show called", this);
        popupObject.SetActive(true);

        currentSentence = GetRandomSentence(dialogueType);
        Debug.Log("Selected sentence: " + currentSentence, this);
        textUI.text = "";
        typeTimer = 0f;
        typeIndex = 0;
        typing = true;
    }

    public void Hide()
    {
        typing = false;
        popupObject.SetActive(false);
    }

    void Update()
    {
        if (typing && currentSentence != null && typeIndex < currentSentence.Length)
        {
            if (textUI == null)
            {
                return;
            }

            typeTimer += Time.deltaTime;
            if (typeTimer >= typingSpeed)
            {
                typeTimer = 0f;
                textUI.text += currentSentence[typeIndex];
                typeIndex++;
            }
        }
    }

    private bool HasAvailableSentenceSource(DialogueType dialogueType)
    {
        List<string> selectedSentences = GetSentenceList(dialogueType);
        return selectedSentences != null && selectedSentences.Count > 0;
    }

    private string GetRandomSentence(DialogueType dialogueType)
    {
        List<string> selectedSentences = GetSentenceList(dialogueType);
        return selectedSentences[Random.Range(0, selectedSentences.Count)];
    }

    private List<string> GetSentenceList(DialogueType dialogueType)
    {
        switch (dialogueType)
        {
            case DialogueType.Approved:
                if (approvedSentences != null && approvedSentences.Count > 0)
                {
                    return approvedSentences;
                }
                break;

            case DialogueType.Rejected:
                if (rejectedSentences != null && rejectedSentences.Count > 0)
                {
                    return rejectedSentences;
                }
                break;
        }

        return defaultSentences;
    }
}
