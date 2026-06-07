using System.Collections;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Player Life Settings")]
    public int maxLives = 3;                // [예린] 최대 라이프 수
    public int lives = 3;                   // 초기 라이프 수

    [Header("Invincible Settings")]
    public float invincibleDuration = 3f;   // 무적 시간
    private bool isInvincible = false;      // 현재 무적 상태인지 여부

    [Header("Flash Effects")]
    [SerializeField] private float flashInterval = 0.1f; // 한 번 깜빡일 때의 속도
    [Range(0f, 1f)]
    [SerializeField] private float minAlpha = 0f;        // 가장 투명해질 때의 알파 값

    private SpriteRenderer playerSprite; // 플레이어 SpriteRenderer 참조
    private Color originalColor;         // 플레이어의 원래 색상 저장

    // [예린] 장애물 충돌 효과음 설정
    [Header("Sound Settings")]
    [SerializeField] private AudioClip hitSound;      // 장애물 충돌 효과음
    [SerializeField] private float hitSoundVolume = 1f; // 장애물 충돌 효과음 볼륨
    private AudioSource audioSource;                  // 효과음 재생용 AudioSource

    private void Awake()
    {
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        
        if (playerSprite != null)
        {
            originalColor = playerSprite.color; // 시작 색상(투명도 포함) 저장
        }
        else
        {
            // 하위에도 없는 경우를 대비한 예외 처리
            Debug.LogError("Player 오브젝트 또는 하위 자식 오브젝트에 SpriteRenderer 컴포넌트가 없습니다!");
        }
        // [예린] Player에 붙어 있는 AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    // [채원] 게임 시작 시 현재 라이프 수만큼 하트 UI 업데이트
    private void Start()
    {
        lives = Mathf.Clamp(lives, 0, maxLives); // [예린] 시작 라이프가 최대값을 넘지 않도록 제한

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(lives);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // [수아] 충돌한 오브젝트가 Obstacle인지 확인
        if (collision.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject);
        }
    }

    // [채원] 장애물 충돌 처리 로직
    private void HandleObstacleCollision(GameObject obstacle) {
        if (isInvincible) return; // 무적 상태에서는 충돌 무시

        // [예린] 실제로 라이프가 감소하는 충돌일 때만 효과음 재생
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound, hitSoundVolume);
        }

        ChangeLife(-1); // 장애물 충돌 시 라이프 감소
        Debug.Log("Life : " + lives);   // 현재 라이프 수 디버그 로그

        if (lives > 0)
        {
            StartCoroutine(BecomeInvincibleCoroutine());
        }
    }

    // [채원] 라이프 변경 공용 함수
    public void ChangeLife(int amount) {
        // [예린] 아이템 및 장애물에 의한 라이프 변화를 0 ~ maxLives 범위로 제한
        lives = Mathf.Clamp(lives + amount, 0, maxLives);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(lives);
        }

        if (lives <= 0) {

            Debug.Log("Obstacle 충돌");

            // 게임 오버
            GameManager.Instance.GameOver();
        }
    }

    // [채원] 설정한 무적 시간만큼 대기 후 무적 상태 해제
    private IEnumerator BecomeInvincibleCoroutine()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작!");

        if (playerSprite != null)
    {
        float elapsedTime = 0f;
        bool isVisble = false; // 켜고 끌 상태 스위치

        while (elapsedTime < invincibleDuration)
        {
            float targetAlpha = isVisble ? originalColor.a : minAlpha;
            
            playerSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
            
            isVisble = !isVisble; // 상태 반전
            
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        playerSprite.color = originalColor; // 원래대로 복구
    }
    else
    {
        yield return new WaitForSeconds(invincibleDuration);
    }

        isInvincible = false;
        Debug.Log("무적 상태 종료!");
    }
}