using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Player Life Settings")]
    public int lives = 3;

    // 게임 시작 시 현재 라이프 수만큼 하트 UI 업데이트 - CWS
    private void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(lives);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // [����] �浹�� ������Ʈ�� Obstacle���� Ȯ��
        if (collision.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject);
        }
    }

    // 장애물 충돌 처리 로직 - CWS
    private void HandleObstacleCollision(GameObject obstacle) {
        ChangeLife(-1); // 장애물 충돌 시 라이프 감소
        Debug.Log("Life : " + lives);   // 현재 라이프 수 디버그 로그
    }

    // 라이프 변경 공용 함수 - CWS
    public void ChangeLife(int amount) {
        lives += amount;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(lives);
        }

        if (lives <= 0) {
            lives = 0;
            Debug.Log("Obstacle �浹");

            // [����] ��� ���� ���� ������ PlayerPrefs�� ����
            // FindObjectOfType�� ���� ���� ���� ScoreManager���� ������ ������
            ScoreManager scoreMgr = FindObjectOfType<ScoreManager>();
            if (scoreMgr != null)
            {
                // ScoreManager���� ��� ���� ������ ������ ��ȯ�Ͽ� 'FinalScore'��� Ű�� ����
                int finalScore = Mathf.FloorToInt(scoreMgr.GetCurrentScore());
                PlayerPrefs.SetInt("FinalScore", finalScore);
                PlayerPrefs.Save(); // ��� ���� Ȯ��
            }

            // ���� ����
            GameManager.Instance.GameOver();
        }
    }

    /* 이외 아이템 충돌 로직은 필요에 따라 함수 추가해 사용할 것 - CWS*/
    
}
