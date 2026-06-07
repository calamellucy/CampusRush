using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // [수아] 싱글톤

    // [수아] 엔딩 종류를 구분하기 위한 enum
    public enum EndingType
    {
        None,
        Late,
        Clear
    }

    // [수아] 현재 엔딩 종류를 저장하는 static 변수
    public static EndingType currentEndingType = EndingType.None;

    private bool isGameOver = false; // [수아] 게임오버 처리 변수
    private bool isGameClear = false; // [수아] 게임클리어 처리 변수

    private void Awake()
    {
        // [수아] GameManager Instance가 없으면 현재 오브젝트를 Instance로 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // [수아] GameManager 중복 생성 방지
            Destroy(gameObject);
        }
    }

    // [수아] 게임 오버 처리 함수
    public void GameOver()
    {
        if (isGameOver) return; // [수아] 게임오버 중복 실행 방지
        isGameOver = true;

        SaveFinalScore(); // [수아] 점수 저장
        currentEndingType = EndingType.Late; // [수아] 지각 엔딩 저장
        SceneManager.LoadScene("EndScene"); // [수아] 엔딩 씬으로 이동
    }

    // [수아] 게임 클리어 처리 함수
    public void GameClear()
    {
        if (isGameClear) return; // [수아] 중복 실행 방지
        isGameClear = true;

        SaveFinalScore(); // [수아] 점수 저장
        currentEndingType = EndingType.Clear; // [수아] 학교 도착 엔딩 저장
        SceneManager.LoadScene("EndScene"); // [수아] 엔딩 씬으로 이동
    }

    // [수아] 현재 점수를 최종 점수로 저장하는 함수
    private void SaveFinalScore()
    {
        ScoreManager scoreMgr = FindFirstObjectByType<ScoreManager>();
        if (scoreMgr != null)
        {
            // ScoreManager에서 계산 중인 점수를 정수로 변환하여 'FinalScore'라는 키로 저장
            int finalScore = Mathf.FloorToInt(scoreMgr.GetCurrentScore());
            PlayerPrefs.SetInt("FinalScore", finalScore);
            PlayerPrefs.Save(); // 즉시 저장 확인
        }
    }
}
