using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC")]
    [SerializeField] private NPCFader npcFader;
    [SerializeField] private TextPopup textPopup;
    [SerializeField] private NPCInner npcInner;

    [Header("포션 소환")]
    [SerializeField] private GameObject potionObject;
    [SerializeField] private PotionManager potionManager;
    [SerializeField] private Transform potionSpawnPoint;
    [SerializeField] private Transform potionStartPos;
    [SerializeField] private float potionMoveDuration = 1.2f;
    [SerializeField] private float resultDialogueDuration = 2f;
    [SerializeField] private float reEnterDelay = 0.5f;

    private bool canSpawnPotion;
    private bool hasSpawnedPotion;
    private Vector3 potionMoveStart;
    private float potionMoveProgress;
    private bool isPotionMoving;
    private bool hasShownDialogue;
    private Coroutine resultRoutine;

    public void ShowNPC()
    {
        RefreshPotionForNewNpc();

        if (npcFader != null)
        {
            npcFader.Show();
        }

        if (npcInner != null)
        {
            npcInner.StartEnter();
        }

        if (potionObject != null)
        {
            potionObject.SetActive(false);
        }

        canSpawnPotion = false;
        hasSpawnedPotion = false;
        hasShownDialogue = false;
    }

    public void HideNPC()
    {
        if (npcFader != null)
        {
            npcFader.Hide();
        }

        if (textPopup != null)
        {
            textPopup.Hide();
        }

        if (npcInner != null)
        {
            npcInner.StartExit();
        }
    }

    public void ApprovePotion()
    {
        StartResultSequence(true);
    }

    public void RejectPotion()
    {
        StartResultSequence(false);
    }

    private void Update()
    {
        UpdatePotionSpawnState();
        UpdateDialogueState();
        HandlePotionSpawnInput();
        MoveSpawnedPotion();
    }

    private void UpdatePotionSpawnState()
    {
        if (npcInner == null)
        {
            canSpawnPotion = true;
            return;
        }

        canSpawnPotion = npcInner.IsInside() && !npcInner.IsMoving();
    }

    private void UpdateDialogueState()
    {
        if (hasShownDialogue || textPopup == null)
        {
            return;
        }

        if (npcInner != null && (!npcInner.IsInside() || npcInner.IsMoving()))
        {
            return;
        }

        textPopup.Show();
        hasShownDialogue = true;
    }

    private void HandlePotionSpawnInput()
    {
        if (!canSpawnPotion || hasSpawnedPotion || !Input.GetMouseButtonDown(0))
        {
            return;
        }

        SpawnPotion();
    }

    private void SpawnPotion()
    {
        if (potionObject == null || potionStartPos == null)
        {
            return;
        }

        Vector3 spawnPosition = potionSpawnPoint != null ? potionSpawnPoint.position : transform.position;
        potionObject.transform.position = spawnPosition;
        potionObject.SetActive(true);
        potionMoveStart = potionObject.transform.position;
        potionMoveProgress = 0f;
        isPotionMoving = true;
        hasSpawnedPotion = true;
    }

    private void MoveSpawnedPotion()
    {
        if (!isPotionMoving || potionObject == null || potionStartPos == null)
        {
            return;
        }

        if (potionMoveDuration <= 0f)
        {
            potionObject.transform.position = potionStartPos.position;
            isPotionMoving = false;
            return;
        }

        potionMoveProgress += Time.deltaTime / potionMoveDuration;
        float moveT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(potionMoveProgress));
        potionObject.transform.position = Vector3.Lerp(
            potionMoveStart,
            potionStartPos.position,
            moveT
        );

        if (potionMoveProgress < 1f)
        {
            return;
        }

        potionObject.transform.position = potionStartPos.position;
        isPotionMoving = false;
    }

    private void ShowResultDialogue(bool isApproved)
    {
        if (textPopup == null)
        {
            return;
        }

        if (isApproved)
        {
            textPopup.ShowApproved();
            return;
        }

        textPopup.ShowRejected();
    }

    private void StartNpcExit()
    {
        canSpawnPotion = false;
        hasShownDialogue = true;

        if (npcInner != null)
        {
            npcInner.StartExit();
        }
    }

    private void StartResultSequence(bool isApproved)
    {
        if (resultRoutine != null)
        {
            StopCoroutine(resultRoutine);
        }

        resultRoutine = StartCoroutine(ProcessResultSequence(isApproved));
    }

    private IEnumerator ProcessResultSequence(bool isApproved)
    {
        canSpawnPotion = false;
        ShowResultDialogue(isApproved);
        yield return new WaitForSeconds(resultDialogueDuration);

        StartNpcExit();

        if (npcInner != null)
        {
            while (npcInner.IsMoving())
            {
                yield return null;
            }
        }

        if (textPopup != null)
        {
            textPopup.Hide();
        }

        yield return new WaitForSeconds(reEnterDelay);
        ShowNPC();
        resultRoutine = null;
    }

    private void RefreshPotionForNewNpc()
    {
        if (potionManager == null && potionObject != null)
        {
            potionManager = potionObject.GetComponent<PotionManager>();
        }

        if (potionManager != null)
        {
            potionManager.RefreshPotionData();
        }
    }
}
