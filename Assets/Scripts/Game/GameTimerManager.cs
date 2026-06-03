using UnityEngine;
using TMPro;

public class GameTimerManager : MonoBehaviour
{
    // [수아] 제한 시간 설정 변수
    [SerializeField] private float timeLimit = 300f;
    [SerializeField] private TextMeshProUGUI timerText;

    // [수아] 실제로 감소하는 현재 남은 시간
    private float currentTime;

    // [수아] 타이머가 끝났는지 확인, 0초 후 GameOver가 여러 번 실행되는 것 방지
    private bool isTimerEnd = false;

    private void Start()
    {
        // [수아] 게임 시작 시 초기화
        currentTime = timeLimit;
        UpdateTimerText();
    }

    private void Update()
    {
        // [수아] 타이머가 이미 끝났으면 더 이상 시간 감소 처리하지 않음
        if (isTimerEnd) return;

        // [수아] 매 프레임마다 지난 시간만큼 현재 시간 감소
        // Time.deltaTime: 이전 프레임부터 현재 프레임까지 걸린 시간
        currentTime -= Time.deltaTime;

        // [수아] 남은 시간이 0초 이하가 되었을 때
        if (currentTime <= 0f)
        {
            currentTime = 0f; // 시간이 음수로 표시되지 않도록 방지
            isTimerEnd = true; // 타이머 종료 상태로 변경
            UpdateTimerText();

            // [수아] 제한 시간이 끝났으므로 게임 종료 처리
            GameManager.Instance.GameOver();
            // TODO : 게임 클리어 처리 추가

            return;
        }

        // [수아] 화면 시간 갱신
        UpdateTimerText();
    }

    // [수아] 남은 시간을 00:00 형식으로 변환해서 화면에 표시하는 함수
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f); // 분 단위 계산
        int seconds = Mathf.FloorToInt(currentTime % 60f); // 초 단위 계산
        timerText.text = $"{minutes:0}:{seconds:00}"; // 화면에 표시
    }
}