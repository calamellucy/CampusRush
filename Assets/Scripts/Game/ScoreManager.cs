using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // [예린] 점수 계산 로직 구현 및 UIManager 연결
    public UIManager uiManager;
    public float baseScorePerSecond = 10f; // [수아] 기본 초당 증가 점수
    public float currentScorePerSecond = 10f; // [수아] 현재 초당 증가 점수
    private float currentScore = 0f;

    private float ItemScoreMultiplier = 1f; // [수아] 이벤트에 의한 점수 배율
    private bool isProfessorEffect = false; // [수아] 교수님 효과 체크 변수
    private bool isRomanceEffect = false; // [수아] 연애 효과 체크 변수
    private bool isCultEffect = false; // [수아] 사이비 효과 체크 변수

    void Update()
    {
        // 시간이 지남에 따라 점수 증가
        currentScore += Time.deltaTime * currentScorePerSecond;

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
    public void AddScore(float amount)
    {
        // [예린] 아이템을 획득했을 때 외부(Item.cs)에서 점수를 즉시 더해줄 공용 함수 추가
        // 현재 점수에 아이템 점수(amount)를 더하기

        // [수아] 마이너스 점수 계산 if문
        if (amount < 0)
        {
            if (isProfessorEffect) // [수아] 교수님 효과일 때는 마이너스가 0점
            {
                amount = 0;
            }
            else if (isRomanceEffect) // [수아] 연애 효과일 때는 마이너스가 2배
            {
                amount *= ItemScoreMultiplier;
            }
            else if (isCultEffect)
            {
                // [수아] 사이비 효과일 때는 마이너스가 그대로 들어감
            }
        }
        else // [수아] 마이너스 점수 아닐 때는 각각의 이벤트 효과 배율을 적용시킴
        {
            amount *= ItemScoreMultiplier;
        }
     
        currentScore += amount;

        // 화면 UI 새로고침
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreDisplay(Mathf.FloorToInt(currentScore));
        }
    }

    // [수아] 이벤트 효과 리셋 함수
    public void ResetScoreEffect()
    {
        SetItemScoreMultiplier(1f);
        SetScorePerSecond(1f);
        isProfessorEffect = false;
        isRomanceEffect = false;
        isCultEffect = false;
    }

    // [수아] 이벤트 효과 적용 함수들
    public void ApplyProfessorEffect() // 교수님 효과
    {
        ResetScoreEffect();
        isProfessorEffect = true;
        SetItemScoreMultiplier(2f);
        SetScorePerSecond(2f);
        Debug.Log("교수님 효과 적용");
        // [수아] 아이템 효과 2배 (마이너스 제외), 초당 점수 2배
    }

    public void ApplyRomanceEffect() // 연애 효과
    {
        ResetScoreEffect();
        isRomanceEffect = true;
        SetItemScoreMultiplier(2f);
        Debug.Log("연애 효과 적용");
        // [수아] 아이템 효과 2배, 초당 점수 변경 없음
    }

    public void ApplyCultEffect() // 사이비 효과
    {
        ResetScoreEffect();
        isCultEffect = true;
        SetItemScoreMultiplier(0.5f);
        SetScorePerSecond(0.5f);
        Debug.Log("사이비 효과 적용");
        // [수아] 아이템 효과 0.5배 (마이너스 제외), 초당 점수 0.5배
    }

    // [수아] 이벤트 점수 배율 변경 함수
    private void SetItemScoreMultiplier(float value)
    {
        ItemScoreMultiplier = value;
    }

    // [수아] 초당 증가하는 점수 값 변경 함수
    private void SetScorePerSecond(float value)
    {
        currentScorePerSecond = value * baseScorePerSecond;
    }

    
}
