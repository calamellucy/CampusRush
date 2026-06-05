using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // [예린] 현재 점수 연동 및 UIManager 연결
    public UIManager uiManager;
    public float baseScorePerSecond = 10f;    // [예린] 기본 초당 점수 증가량
    public float currentScorePerSecond = 10f; // [예린] 현재 초당 점수 증가량
    private float currentScore = 0f;

    private float ItemScoreMultiplier = 1f;   // [수아] 이벤트로 인한 점수 배율
    private bool isProfessorEffect = false;   // [수아] 교수님 효과 체크 변수
    private bool isRomanceEffect = false;     // [수아] 로맨스 효과 체크 변수
    private bool isCultEffect = false;        // [수아] 사이비 효과 체크 변수

    void Update()
    {
        // 시간이 흐름에 따라 지속적으로 점수 증가
        currentScore += Time.deltaTime * currentScorePerSecond;

        // UIManager를 통해 화면에 표시
        if (uiManager != null)
        {
            uiManager.UpdateScoreDisplay(Mathf.FloorToInt(currentScore));
        }
    }

    public float GetCurrentScore()  // 플레이어 충돌 시 등 이 함수를 호출하여 현재 점수를 가져옴
    {
        return currentScore;    // 소수점이 포함된 실시간 점수를 반환
    }

    public float AddScore(float amount)
    {
        // [예린] 아이템을 획득했을 때 외부(Item.cs)에서 호출하여 점수를 반영하는 함수 추가
        // 원래 획득해야 하는 점수(amount)에 배율 계산하기

        // [수아] 마이너스 점수 처리 if문
        if (amount < 0)
        {
            if (isProfessorEffect) // [수아] 교수님 효과는 마이너스 점수가 0이 됨
            {
                amount = 0;
            }
            else if (isRomanceEffect) // [수아] 로맨스 효과는 마이너스도 2배가 됨
            {
                amount *= ItemScoreMultiplier;
            }
            else if (isCultEffect)
            {
                // [수아] 사이비 효과는 마이너스가 그대로 들어감
            }
        }
        else // [수아] 마이너스 점수가 아닌 정상 점수는 이벤트 효과(배율)를 곱해줌
        {
            amount *= ItemScoreMultiplier;
        }

        currentScore += amount;

        // 화면 UI 새로고침
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreDisplay(Mathf.FloorToInt(currentScore));
        }

        return amount;
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
        Debug.Log("교수님 효과 발동");
        // [수아] 아이템 효과 2배 (마이너스 제외), 초당 점수 2배
    }

    public void ApplyRomanceEffect() // 로맨스 효과
    {
        ResetScoreEffect();
        isRomanceEffect = true;
        SetItemScoreMultiplier(2f);
        Debug.Log("로맨스 효과 발동");
        // [수아] 아이템 효과 2배, 초당 점수는 기본 유지
    }

    public void ApplyCultEffect() // 사이비 효과
    {
        ResetScoreEffect();
        isCultEffect = true;
        SetItemScoreMultiplier(0.5f);
        SetScorePerSecond(0.5f);
        Debug.Log("사이비 효과 발동");
        // [수아] 아이템 효과 0.5배 (마이너스 포함), 초당 점수 0.5배
    }

    // [수아] 이벤트 점수 배율 설정 함수
    private void SetItemScoreMultiplier(float value)
    {
        ItemScoreMultiplier = value;
    }

    // [수아] 초당 증가하는 점수 배율 설정 함수
    private void SetScorePerSecond(float value)
    {
        currentScorePerSecond = value * baseScorePerSecond;
    }
}