using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // [예린] 점수 계산 로직 구현 및 UIManager 연결
    public UIManager uiManager;
    public float scoreMultiplier = 10f; // 초당 증가 점수
    private float currentScore = 0f;

    void Update()
    {
        // 시간이 지남에 따라 점수 증가
        currentScore += Time.deltaTime * scoreMultiplier;

        // UIManager를 통해 화면에 표시
        if (uiManager != null)
        {
            uiManager.UpdateScoreDisplay(Mathf.FloorToInt(currentScore));
        }
    }
    public float GetCurrentScore()  // 플레이어 충돌 시 이 함수를 호출하여 점수를 저장
    {
        return currentScore;    // 현재까지 계산된 실시간 점수를 반환
    }
}
