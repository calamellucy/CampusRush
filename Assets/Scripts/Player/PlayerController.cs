using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 점프 관련 값 설정 - CWS
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxJumpCount = 2;
    private int currentJumpCount = 0;

    [Header("Detection Settings")]
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSizeMultiplier = 0.5f;

    // 내부 컴포넌트 및 상태 변수 - CWS
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private bool isGrounded;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    // 애니메이션 제어를 위한 변수 - CWS
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        
        // Animator 컴포넌트 가져오기 - CWS
        animator = GetComponent<Animator>();

        // 초기 콜라이더 값 저장 (나중에 복구용) - CWS
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        // 콜라이더의 하단 중앙 좌표 계산 - CWS
        float colliderBottom = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
        Vector2 checkPosition = new Vector2(playerCollider.bounds.center.x, colliderBottom);

        // 1. 지면 체크 (OverlapCircle 사용) - CWS
        isGrounded = Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
        
        // 2. 점프 카운트 초기화 - CWS
        // 땅에 닿아 있고, 위로 솟구치는 중이 아닐 때만 초기화 (이단 점프 씹힘 방지) - CWS
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            currentJumpCount = 0;
        }
    }

    public void OnJump(InputValue value)
    {
        // 버튼을 눌렀을 때만 실행 - CWS
        if (value.isPressed)
        {
            // 점프 횟수가 남을 경우만 점프 가능 - CWS
            if (currentJumpCount < maxJumpCount)
            {
                PerformJump();
            }
        }
    }

    private void PerformJump()
    {
        // 기존 낙하 속도를 무시하고 즉시 jumpForce 부여 - CWS
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        currentJumpCount++;
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            if (!isGrounded)
                return; // 공중에서는 숙이지 않도록 방지 - CWS
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        // 숙이기 애니메이션 활성화 (Player_Crouch로 전환) - CWS
        animator.SetBool("IsCrouching", true);

        // 높이는 줄이고, 아래쪽으로 오프셋을 이동시켜 발 위치 고정 - CWS
        float newHeight = originalColliderSize.y * crouchSizeMultiplier;
        playerCollider.size = new Vector2(originalColliderSize.x, newHeight);

        float offsetShift = (originalColliderSize.y - newHeight) / 2f;
        playerCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - offsetShift);
    }

    private void StopCrouch()
    {
        // 숙이기 애니메이션 비활성화 (다시 Player_Run으로 복귀) - CWS
        animator.SetBool("IsCrouching", false);

        // 원래 크기로 콜라이더 복구 - CWS
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }

    // 에디터에서 Ground Check 범위를 시각적으로 확인하기 위해 사용 - CWS
    private void OnDrawGizmosSelected()
    {
        if (playerCollider != null)
        {
            float colliderBottom = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
            Vector2 checkPosition = new Vector2(playerCollider.bounds.center.x, colliderBottom);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checkPosition, checkRadius);
        }
    }
}