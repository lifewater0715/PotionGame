using UnityEngine;
using UnityEngine.Events;

public class NewspaperController : MonoBehaviour
{
    [Header("신문 패널")]
    [SerializeField] private GameObject newspaperPanel;

    [Header("애니메이션")]
    [SerializeField] private bool useAnimation = true;
    [SerializeField] private float animationSpeed = 0.3f;

    [Header("이벤트")]
    public UnityEvent onOpened;
    public UnityEvent onClosed;

    public static NewspaperController Instance { get; private set; }
    public bool IsOpen { get; private set; }

    private CanvasGroup canvasGroup;
    private Coroutine currentAnimation;

    private void Awake()
    {
        Instance = this;

        canvasGroup = newspaperPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = newspaperPanel.AddComponent<CanvasGroup>();

        newspaperPanel.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        if (IsOpen) return;

        IsOpen = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (useAnimation)
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            currentAnimation = StartCoroutine(FadeIn());
        }
        else
        {
            canvasGroup.alpha = 1f;
        }

        onOpened?.Invoke();
    }

    public void Close()
    {
        if (!IsOpen) return;

        IsOpen = false;

        if (useAnimation)
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            currentAnimation = StartCoroutine(FadeOut());
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        onClosed?.Invoke();
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < animationSpeed)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / animationSpeed);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < animationSpeed)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / animationSpeed);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}