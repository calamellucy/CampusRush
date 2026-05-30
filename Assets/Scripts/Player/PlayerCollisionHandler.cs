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

            // [예린] 사망 직전 최종 점수를 PlayerPrefs에 저장
            // FindObjectOfType을 통해 현재 씬의 ScoreManager에서 점수를 가져옴
            ScoreManager scoreMgr = FindFirstObjectByType<ScoreManager>();
            if (scoreMgr != null)
            {
                // ScoreManager에서 계산 중인 점수를 정수로 변환하여 'FinalScore'라는 키로 저장
                int finalScore = Mathf.FloorToInt(scoreMgr.GetCurrentScore());
                PlayerPrefs.SetInt("FinalScore", finalScore);
                PlayerPrefs.Save(); // 즉시 저장 확인
            }

            // 게임 오버
            GameManager.Instance.GameOver();
        }
    }

    // [채원] 설정한 무적 시간만큼 대기 후 무적 상태 해제
    private IEnumerator BecomeInvincibleCoroutine()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작!");

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        Debug.Log("무적 상태 종료!");
    }
}