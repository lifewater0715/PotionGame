using System.Collections;
using UnityEngine;

public class PotionClicker : MonoBehaviour
{
    private GameObject potion;
    private RaycastHit2D rayHitTrg;

    [Header("선택 타겟(수정 금지)")]
    [SerializeField] private GameObject ClickTrg;
    [SerializeField] private GameObject potionTrgPos;
    [Header("포션 이동속도")]
    [SerializeField] private float DragSpeed;
    [Header("포션 이동 기울기 보정값")]
    [SerializeField] private float DragAngle;
    [Header("포션 이동 움직임 정지 관측값")]
    [SerializeField] private float StopDragForce;
    [Header("포션 흔들기 판정값")]
    [SerializeField] private float ShakeDetectForce = 0.6f;
    [Header("포션 시작 위치")]
    [SerializeField] private GameObject potionStartPos;
    [Header("포션 복귀 시간")]
    [SerializeField] private float retrunSpeed;

    private Animator potionAnimator;
    private bool potionAnimatorIsRight;
    private bool potionAnimatorIsLeft;
    private float potionAnimatorStopSpeed;

    private PotionState potionState;

    void Start()
    {
        potionTrgPos = potionStartPos;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rayHitTrg = Physics2D.Raycast(mousPos, Vector2.zero, 1, LayerMask.GetMask("Potion"));

            if (rayHitTrg)
            {
                ClickTrg = rayHitTrg.collider.gameObject;

                if (ClickTrg.tag == "Potion")
                {
                    Cursor.visible = false;
                    potionState = ClickTrg.GetComponent<PotionState>();

                    Debug.Log("감지된 레이어: " + LayerMask.LayerToName(ClickTrg.gameObject.layer));
                    //Debug.Log("포션 선택됨");
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (ClickTrg != null)
            {
                PotionClick();
            }
            //Debug.Log("포션 이동중");
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (ClickTrg != null)
            {
                PotionDrop();
            }
            //Debug.Log("포션 내려놓음");
        }
    }

    private void PotionClick()
    {
        Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lastPosition = ClickTrg.transform.position;

        ClickTrg.transform.position = Vector2.Lerp(ClickTrg.transform.position, mousPos, DragSpeed * Time.deltaTime);

        Vector2 currentPosition = ClickTrg.transform.position;

        float moveVelocity = (currentPosition.x - lastPosition.x) / Time.deltaTime;
        float moveSpeed = GetMoveSpeed(lastPosition, currentPosition);
        //Debug.Log("MoveVlocity = " + moveVelocity);

        potionAnimator = ClickTrg.transform.GetChild(1).GetComponent<Animator>();

        PotionAcceleration(moveVelocity,ClickTrg);
        PotionAnimtion(moveVelocity, ClickTrg);
        UpdatePotionShakeState(moveSpeed);
    }

    private void PotionAcceleration(float moveVelocity, GameObject moveTrg)
    {
        Vector3 potionForceAngle = new Vector3(0, 0, moveTrg.transform.rotation.z + (moveVelocity * -DragAngle));
        moveTrg.transform.rotation = Quaternion.Slerp(moveTrg.transform.rotation, Quaternion.Euler(potionForceAngle), DragSpeed * Time.deltaTime);
    }

    private void PotionDrop()
    {
        Cursor.visible = true;
        UpdatePotionShakeState(0f);

        StartCoroutine(RetrunPotion(retrunSpeed, ClickTrg.transform.position, potionTrgPos.transform.position, ClickTrg));
        ClickTrg = null;

    }

    private void PotionAnimtion(float moveVlocity, GameObject moveTrg)
    {
        if (moveVlocity > 0f)
        {
            potionAnimator.SetBool("IsMoveEnd", false);

            potionAnimator.SetBool("IsRight", true);
            potionAnimator.SetBool("IsLeft", false);
        }

        if (moveVlocity < 0f)
        {
            potionAnimator.SetBool("IsMoveEnd", false);

            potionAnimator.SetBool("IsRight", false);
            potionAnimator.SetBool("IsLeft", true);
        }

        if (moveVlocity < StopDragForce && moveVlocity > -StopDragForce)
        {
            potionAnimator.SetBool("IsMoveEnd", true);
            potionAnimator.SetBool("IsRight", false);
            potionAnimator.SetBool("IsLeft", false);
            ResetPotionAngle(moveTrg);
        }
    }

    private void ResetPotionAngle(GameObject moveTrg)
    {
        moveTrg.transform.rotation = Quaternion.Slerp(
            moveTrg.transform.rotation,
            Quaternion.Euler(Vector3.zero),
            DragSpeed * Time.deltaTime
        );
    }

    IEnumerator RetrunPotion(float retruntime, Vector3 startpos, Vector3 targetpos, GameObject moveTrg)
    {
        float timestack = 0f;
        potionAnimator = moveTrg.transform.GetChild(1).GetComponent<Animator>();

        while (timestack < retruntime+0.2f)
        {
            timestack += Time.deltaTime;

            Vector2 lastPosition = moveTrg.transform.position;

            moveTrg.transform.position = Vector3.Lerp(startpos, targetpos, Mathf.Clamp01(timestack / retruntime));
            ResetPotionAngle(moveTrg);

            Vector2 currentPosition = moveTrg.transform.position;

            float moveVelocity = (currentPosition.x - lastPosition.x) / Time.deltaTime;
            float moveSpeed = GetMoveSpeed(lastPosition, currentPosition);

            PotionAcceleration(moveVelocity,moveTrg);
            PotionAnimtion(moveVelocity, moveTrg);
            UpdatePotionShakeState(moveSpeed);

            yield return null;
        }

        UpdatePotionShakeState(0f);
        transform.position = targetpos;
    }

    private float GetMoveSpeed(Vector2 lastPosition, Vector2 currentPosition)
    {
        return Vector2.Distance(currentPosition, lastPosition) / Time.deltaTime;
    }

    private void UpdatePotionShakeState(float moveSpeed)
    {
        if (potionState == null)
        {
            return;
        }

        potionState.PotionMixing(moveSpeed >= ShakeDetectForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Boil")
        {
            potionTrgPos = other.transform.GetChild(0).gameObject;
            potionState.PotionHeating(true);
            
        }

        else if (other.gameObject.tag == "Heat")
        {
            potionState.PotionFireing(true);
        }

        else if (other.gameObject.tag == null)
        {
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        potionTrgPos = potionStartPos;

        potionState.PotionHeating(false);
        potionState.PotionMixing (false);
        potionState.PotionFireing(false);
        potionState.PotionZooming(false);
        potionState.PotionAciding(false);
    }
}
