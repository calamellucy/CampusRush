using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // [채원] 점프 관련 값 설정
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float fallMultiplier = 4.0f;     // 최고점을 찍고 내려올 때 적용할 추가 중력 배율
    private int currentJumpCount = 0;

    [Header("Detection Settings")]
    [SerializeField] private Transform groundCheckPoint; // 발밑 중심점이 될 오브젝트 위치
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f); // 체크할 사각형의 크기 (가로, 세로)
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSizeMultiplier = 0.5f;

    // [예린] 효과음 관련 값 설정
    [Header("Sound Settings")]
    [SerializeField] private AudioClip jumpSound; // 점프 효과음
    [SerializeField] private AudioClip crouchSound; // 숙이기 효과음
    [SerializeField] private float jumpSoundVolume = 1f;
    [SerializeField] private float crouchSoundVolume = 0.5f; // 숙이기 효과음 볼륨
    private AudioSource audioSource;              // 효과음 재생용 AudioSource
    

    // [채원] 내부 컴포넌트 및 상태 변수
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private bool isGrounded;
    private bool isCrouchPressed = false;

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

        // [예린] AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // [채원] 초기 콜라이더 값 저장 (나중에 복구용)
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        // [채원] 콜라이더의 하단 중앙 좌표 계산
        float colliderBottom = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
        Vector2 checkPosition = new Vector2(playerCollider.bounds.center.x, colliderBottom);

        // [채원] 지면 체크 (OverlapBox 사용)
        if (groundCheckPoint != null)
        {
            isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, boxSize, 0f, groundLayer);
        }
        
        // [채원] 점프 카운트 초기화
        // 땅에 닿아 있고, 위로 솟구치는 중이 아닐 때만 초기화 (이단 점프 씹힘 방지)
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            currentJumpCount = 0;
        }

        // [채원] 숙이기 입력 처리
        if (isGrounded && isCrouchPressed)
        {
            if (animator != null && !animator.GetBool("IsCrouching"))
            {
                StartCrouch();
            }
        }
        else if (!isGrounded || !isCrouchPressed)
        {
            // 공중에 뜨거나 키를 떼면 숙이기 해제
            if (animator != null && animator.GetBool("IsCrouching"))
            {
                StopCrouch();
            }
        }

        // [채원] 최고점에서 낙하할 때 추가 중력 적용
        ApplyAdditionalGravity();

        // [예린] 점프 애니메이션 제어를 위해 현재 지면 상태와 y축 속도를 Animator에 전달
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("YVelocity", rb.linearVelocity.y);
        }
    }

    // [채원] 추가 중력 적용 메서드
    private void ApplyAdditionalGravity()
    {
        if (isGrounded) return;

        Vector2 velocity = rb.linearVelocity;

        // 최고점을 찍고 아래로 낙하하기 시작할 때
        if (rb.linearVelocity.y < 0) 
        {
            // fallMultiplier를 적용해 중력 가속도를 붙임
            velocity.y += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 

        rb.linearVelocity = velocity;
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

        // [예린] 점프 효과음 재생
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound, jumpSoundVolume);
        }
    }

    // [채원] 숙이기 입력 처리
    public void OnCrouch(InputValue value)
    {
        isCrouchPressed = value.isPressed;
    }

    // [채원] 숙이기 시작
    private void StartCrouch()
    {
        // [채원] 숙이기 애니메이션 활성화 (Player_Crouch로 전환)
        animator.SetBool("IsCrouching", true);

        // [예린] 숙이기 버튼을 처음 눌렀을 때만 효과음 재생
        if (audioSource != null && crouchSound != null)
        {
            audioSource.PlayOneShot(crouchSound, crouchSoundVolume);
        }

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
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red; // 평소에는 빨간색 박스
            if (isGrounded) Gizmos.color = Color.green; // 땅에 닿으면 초록색 박스로 변경

            // 사각형 범위 그리기
            Gizmos.DrawWireCube(groundCheckPoint.position, boxSize);
        }
    }
}