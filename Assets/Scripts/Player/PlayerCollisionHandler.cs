using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // [수아] 충돌한 오브젝트가 Obstacle인지 확인
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle 충돌");

            // [예린] 사망 직전 최종 점수를 PlayerPrefs에 저장
            // FindObjectOfType을 통해 현재 씬의 ScoreManager에서 점수를 가져옴
            ScoreManager scoreMgr = FindObjectOfType<ScoreManager>();
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
}
