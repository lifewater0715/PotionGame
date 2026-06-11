using UnityEngine;
using UnityEngine.EventSystems;

public class PotionPaper : MonoBehaviour
{
    [Header("검사지 스프라이트")]
    [SerializeField] private SpriteRenderer paperRenderer;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite hoverSprite;

    [Header("검사지 UI")]
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private Vector2 closedUiPosition;
    [SerializeField] private Vector2 openedUiPosition;
    [SerializeField] private float uiMoveSpeed = 8f;

    private bool isUiOpened;

    void Start()
    {
        if (paperRenderer == null)
        {
            paperRenderer = GetComponent<SpriteRenderer>();
        }

        if (paperRenderer != null && defaultSprite == null)
        {
            defaultSprite = paperRenderer.sprite;
        }

        if (targetUI != null)
        {
            targetUI.anchoredPosition = closedUiPosition;
        }
    }

    private void OnMouseEnter()
    {
        SetPaperSprite(hoverSprite);
    }

    private void OnMouseExit()
    {
        SetPaperSprite(defaultSprite);
    }

    private void SetPaperSprite(Sprite targetSprite)
    {
        if (paperRenderer == null || targetSprite == null)
        {
            return;
        }

        paperRenderer.sprite = targetSprite;
    }

    private void LateUpdate()
    {
        if (targetUI == null)
        {
            return;
        }

        Vector2 targetPosition = isUiOpened ? openedUiPosition : closedUiPosition;
        targetUI.anchoredPosition = Vector2.Lerp(
            targetUI.anchoredPosition,
            targetPosition,
            uiMoveSpeed * Time.deltaTime
        );
    }

    private void OnMouseDown()
    {
        isUiOpened = true;
    }

    private void Update()
    {
        if (!isUiOpened || !Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (IsPointerOverUi() || IsPointerOverPaper())
        {
            return;
        }

        isUiOpened = false;
    }

    private bool IsPointerOverUi()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsPointerOverPaper()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
        return hitCollider != null && hitCollider.gameObject == gameObject;
    }
}
