using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NewspaperContent : MonoBehaviour
{
    [Header("텍스트 UI")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private TMP_Text titleTMPText;
    [SerializeField] private TMP_Text contentTMPText;

    [Header("뉴스 리스트")]
    [SerializeField] private List<NewsData> newsList = new List<NewsData>();

    public static NewspaperContent Instance { get; private set; }

    private int currentIndex = 0; // 현재 출력 중인 뉴스의 인덱스

    [System.Serializable]
    public class NewsData
    {
        public string title;
        [TextArea(3, 10)]
        public string content;
    }

    private void Awake()
    {
        Instance = this;

        if (newsList.Count == 0)
        {
            newsList.Add(new NewsData
            {
                title = "기본 뉴스",
                content = "기본 내용입니다."
            });
        }
    }

    // 순서대로 뉴스 출력 (currentIndex 사용)
    public void SetRandomText()
    {
        if (newsList.Count == 0) return;

        if (currentIndex >= newsList.Count) currentIndex = 0;

        NewsData selected = newsList[currentIndex];
        SetTitle(selected.title);
        SetContent(selected.content);
    }

    // 다음 뉴스로 이동
    public void NextNews()
    {
        if (newsList.Count == 0) return;

        currentIndex++;
        if (currentIndex >= newsList.Count)
        {
            currentIndex = 0;
        }
        SetCurrentText();
    }

    // 이전 뉴스로 이동
    public void PreviousNews()
    {
        if (newsList.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = newsList.Count - 1;
        }
        SetCurrentText();
    }

    // 현재 인덱스의 뉴스 출력
    public void SetCurrentText()
    {
        if (newsList.Count == 0) return;
        if (currentIndex >= newsList.Count) currentIndex = 0;

        SetTitle(newsList[currentIndex].title);
        SetContent(newsList[currentIndex].content);
    }

    // 처음 뉴스로 리셋
    public void ResetToFirstNews()
    {
        currentIndex = 0;
        SetCurrentText();
    }

    // 특정 인덱스로 이동
    public void SetTextByIndex(int index)
    {
        if (index < 0 || index >= newsList.Count) return;

        currentIndex = index;
        SetTitle(newsList[index].title);
        SetContent(newsList[index].content);
    }

    public void SetText(string title, string content)
    {
        SetTitle(title);
        SetContent(content);
    }

    private void SetTitle(string title)
    {
        if (titleText != null) titleText.text = title;
        if (titleTMPText != null) titleTMPText.text = title;
    }

    private void SetContent(string content)
    {
        if (contentText != null) contentText.text = content;
        if (contentTMPText != null) contentTMPText.text = content;
    }

    // 현재 인덱스 확인
    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    // 전체 뉴스 개수 확인
    public int GetNewsCount()
    {
        return newsList.Count;
    }
}