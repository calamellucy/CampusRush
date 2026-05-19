using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // [채원] 점프 관련 값 설정
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxJumpCount = 2;
    private int currentJumpCount = 0;

    [Header("Detection Settings")]
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSizeMultiplier = 0.5f;

    // [채원] 내부 컴포넌트 및 상태 변수
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private bool isGrounded;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    // [채원] 애니메이션 제어를 위한 변수
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        
        // [채원] Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // [채원] 초기 콜라이더 값 저장 (나중에 복구용)
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        // [채원] 콜라이더의 하단 중앙 좌표 계산
        float colliderBottom = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
        Vector2 checkPosition = new Vector2(playerCollider.bounds.center.x, colliderBottom);

        // [채원] 1. 지면 체크 (OverlapCircle 사용)
        isGrounded = Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
        
        // [채원] 2. 점프 카운트 초기화
        // 땅에 닿아 있고, 위로 솟구치는 중이 아닐 때만 초기화 (이단 점프 씹힘 방지)
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            currentJumpCount = 0;
        }
    }

    // [채원] 점프 입력 처리
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            // [채원] 점프 횟수가 남을 경우만 점프 가능
            if (currentJumpCount < maxJumpCount)
            {
                PerformJump();
            }
        }
    }

    // [채원] 실제 점프 수행 메서드
    private void PerformJump()
    {
        // [채원] 기존 낙하 속도를 무시하고 즉시 jumpForce 부여
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        currentJumpCount++;
    }

    // [채원] 숙이기 입력 처리
    public void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            if (!isGrounded)
                return; // [채원] 공중에서는 숙이지 않도록 방지
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }
    }

    // [채원] 숙이기 시작
    private void StartCrouch()
    {
        // [채원] 숙이기 애니메이션 활성화 (Player_Crouch로 전환)
        animator.SetBool("IsCrouching", true);

        // [채원] 높이는 줄이고, 아래쪽으로 오프셋을 이동시켜 발 위치 고정
        float newHeight = originalColliderSize.y * crouchSizeMultiplier;
        playerCollider.size = new Vector2(originalColliderSize.x, newHeight);

        float offsetShift = (originalColliderSize.y - newHeight) / 2f;
        playerCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - offsetShift);
    }

    // [채원] 숙이기 종료
    private void StopCrouch()
    {
        // [채원] 숙이기 애니메이션 비활성화 (다시 Player_Run으로 복귀)
        animator.SetBool("IsCrouching", false);

        // [채원] 원래 크기로 콜라이더 복구
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }

    // [채원] 에디터에서 Ground Check 범위를 시각적으로 확인하기 위해 사용
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