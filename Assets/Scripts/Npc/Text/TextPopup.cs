using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextPopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textUI;
    public float fadeSpeed = 2f;
    public float typingSpeed = 0.05f;

    public List<string> sentences = new List<string>()
    {
        "Hello!",
        "Nice to meet you.",
        "How can I help you?"
    };

    private bool fadingIn;
    private bool fadingOut;
    private float timer;
    private string currentSentence;
    private float typeTimer;
    private int typeIndex;
    private bool typing;

    void OnEnable()
    {
        canvasGroup.alpha = 0f;
        fadingIn = false;
        fadingOut = false;
        timer = 0f;
    }

    public void Show()
    {
        Debug.Log("TextPopup Show 실행됨");
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        fadingIn = true;
        fadingOut = false;
        timer = 0f;

        currentSentence = sentences[Random.Range(0, sentences.Count)];
        Debug.Log("선택된 문장: " + currentSentence);  // 추가
        textUI.text = "";
        typeTimer = 0f;
        typeIndex = 0;
        typing = true;
    }

    public void Hide()
    {
        fadingOut = true;
        fadingIn = false;
        timer = 0f;
        typing = false;
    }

    void Update()
    {
        // 페이드 인
        if (fadingIn)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = timer;

            if (timer >= 1f)
            {
                canvasGroup.alpha = 1f;
                fadingIn = false;
            }
        }

        // 페이드 아웃
        if (fadingOut)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = 1f - timer;

            if (timer >= 1f)
            {
                canvasGroup.alpha = 0f;
                fadingOut = false;
                gameObject.SetActive(false);
            }
        }

        // 타이핑 효과
        if (typing && currentSentence != null && typeIndex < currentSentence.Length)
        {
            typeTimer += Time.deltaTime;
            if (typeTimer >= typingSpeed)
            {
                typeTimer = 0f;
                textUI.text += currentSentence[typeIndex];
                typeIndex++;
            }
        }
    }
}