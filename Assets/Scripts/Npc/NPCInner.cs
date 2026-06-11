using UnityEngine;

public class NPCInner : MonoBehaviour
{
    [Header("NPC 대상")]
    [SerializeField] private Transform npcTarget;

    [Header("이동 위치")]
    [SerializeField] private Transform enterPoint;
    [SerializeField] private Transform stayPoint;
    [SerializeField] private Transform exitPoint;

    [Header("이동 설정")]
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float stopDistance = 0.05f;
    [SerializeField] private bool playEnterOnStart = true;

    private Transform currentTarget;
    private bool isMoving;
    private bool isInside;
    private Vector3 moveStartPosition;
    private float moveProgress;

    void Start()
    {
        if (npcTarget == null)
        {
            npcTarget = transform;
        }

        if (playEnterOnStart)
        {
            StartEnter();
        }
    }

    void Update()
    {
        MoveNpc();
    }

    public void StartEnter()
    {
        if (npcTarget == null)
        {
            return;
        }

        if (enterPoint != null)
        {
            npcTarget.position = enterPoint.position;
        }

        BeginMove(stayPoint);
        isInside = true;
    }

    public void StartExit()
    {
        BeginMove(exitPoint);
        isInside = false;
    }

    public bool IsInside()
    {
        return isInside;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void MoveNpc()
    {
        if (!isMoving || currentTarget == null || npcTarget == null)
        {
            return;
        }

        if (moveDuration <= 0f)
        {
            npcTarget.position = currentTarget.position;
            isMoving = false;
            return;
        }

        moveProgress += Time.deltaTime / moveDuration;
        float smoothProgress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(moveProgress));
        npcTarget.position = Vector3.Lerp(
            moveStartPosition,
            currentTarget.position,
            smoothProgress
        );

        if (moveProgress >= 1f || Vector3.Distance(npcTarget.position, currentTarget.position) <= stopDistance)
        {
            npcTarget.position = currentTarget.position;
            isMoving = false;
        }
    }

    private void BeginMove(Transform targetPoint)
    {
        currentTarget = targetPoint;
        isMoving = currentTarget != null && npcTarget != null;
        moveStartPosition = npcTarget != null ? npcTarget.position : Vector3.zero;
        moveProgress = 0f;
    }
}
