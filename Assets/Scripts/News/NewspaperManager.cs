using UnityEngine;
using UnityEngine.UI;

public class NewspaperManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private NewspaperController newspaperController;
    [SerializeField] private NewspaperContent newspaperContent;

    [Header("신문 열기 버튼")]
    [SerializeField] private Button openButton;

    public static NewspaperManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 신문 열기 버튼
        if (openButton != null)
        {
            openButton.onClick.AddListener(OpenNewspaper);
        }

        // 신문 패널 클릭 시 닫기
        if (newspaperController != null)
        {
            Button panelButton = newspaperController.GetComponent<Button>();
            if (panelButton == null)
            {
                panelButton = newspaperController.gameObject.AddComponent<Button>();
            }
            panelButton.onClick.AddListener(CloseNewspaper);
        }
    }

    private void OpenNewspaper()
    {
        // 열기 버튼 숨김
        if (openButton != null)
            openButton.gameObject.SetActive(false);

        // 다음 뉴스로 이동한 후 출력 (열 때마다 다음 뉴스)
        newspaperContent.NextNews();  // 여기가 핵심!

        // 신문 열기
        newspaperController.Open();
    }

    private void CloseNewspaper()
    {
        // 신문 닫기
        newspaperController.Close();

        // 열기 버튼 다시 표시
        if (openButton != null)
            openButton.gameObject.SetActive(true);
    }
}