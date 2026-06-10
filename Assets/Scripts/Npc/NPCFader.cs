using UnityEngine;

public class NPCFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 2f;

    private bool fadingIn;
    private bool fadingOut;
    private float timer;

    void OnEnable()
    {
        canvasGroup.alpha = 0f;
        fadingIn = false;
        fadingOut = false;
        timer = 0f;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        fadingIn = true;
        fadingOut = false;
        timer = 0f;
    }

    public void Hide()
    {
        fadingOut = true;
        fadingIn = false;
        timer = 0f;
    }

    void Update()
    {
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
    }
}