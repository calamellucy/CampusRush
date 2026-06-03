using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndScene : MonoBehaviour
{
    // [수아] 엔딩 자막 관련 변수
    [SerializeField] private TextMeshProUGUI endingSubtitleText; // 엔딩 자막 text
    [SerializeField] private TextMeshProUGUI finalScoreText; // 최종 점수 및 학점 text
    [SerializeField] private float typingSpeed = 0.05f; // 엔딩 자막 글자 속도
    [SerializeField] private float lineWaitTime = 1.2f; // 엔딩 자막 넘어가기 전 대기 시간

    // [수아] 재시작 버튼과 메인화면 버튼
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject mainButton;

    // [수아] 2가지 엔딩 배경화면
    [SerializeField] private GameObject GameClearBackground; // 게임클리어 화면
    [SerializeField] private GameObject GameOverBackground; // 게임오버 화면

    private void Start()
    {
        SetEndingBackground();

        finalScoreText.text = "";

        restartButton.SetActive(false);
        mainButton.SetActive(false);


        StartCoroutine(PlayEndingSequence());
    }

    // [수아] 버튼 클릭 함수들

    public void OnClickRestart()
    {
        SceneManager.LoadScene("MainScene");
        // Restart 버튼 클릭 시 MainScene으로 이동
    }

    public void OnClickMain()
    {
        SceneManager.LoadScene("StartScene");
        // Main 버튼 클릭 시 StartScene으로 이동
    }

    private IEnumerator PlayEndingSequence()
    {
        string[] subtitles;

        // [수아] GameManager에 저장된 엔딩 타입에 따라 문구 변경
        if (GameManager.currentEndingType == GameManager.EndingType.Clear)
        {
            subtitles = new string[]
            {
                "학교에 겨우 도착했다...",
                "다행히 지각은 면했어!"
            };
        }
        else if (GameManager.currentEndingType == GameManager.EndingType.Late)
        {
            subtitles = new string[]
            {
                "결국 체력이 모두 소진되었다.",
                "달릴 힘이 없어서 지하철을 놓쳤다.",
                "지각이다..."
            };
        }
        else
        {
            subtitles = new string[]
            {
                "엔딩 정보를 불러오지 못했습니다."
            };
        }

        // [수아] 자막 출력
        for (int i = 0; i < subtitles.Length; i++)
        {
            endingSubtitleText.text = "";

            foreach (char letter in subtitles[i])
            {
                endingSubtitleText.text += letter;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            // [수아] 마지막 줄이 아니면 잠깐 보여준 뒤 지우기
            if (i < subtitles.Length - 1)
            {
                yield return new WaitForSecondsRealtime(lineWaitTime);
                endingSubtitleText.text = "";
            }
        }

        // [수아] 마지막 자막은 그대로 둔 상태로 잠깐 대기
        yield return new WaitForSecondsRealtime(0.8f);

        int score = PlayerPrefs.GetInt("FinalScore", 0);
        string finalGrade = "";

        if (score >= 10000) finalGrade = "A+";
        else if (score >= 8000) finalGrade = "A0";
        else if (score >= 7000) finalGrade = "B+";
        else if (score >= 6000) finalGrade = "B0";
        else if (score >= 5000) finalGrade = "C+";
        else if (score >= 4000) finalGrade = "C0";
        else if (score >= 3000) finalGrade = "D+";
        else if (score >= 2000) finalGrade = "D0";
        else finalGrade = "F";

        // [수아] 최종 점수 타자기 효과 출력
        yield return StartCoroutine(TypeText(finalScoreText, "최종 점수: " + score + "   최종 학점: " + finalGrade));

        yield return new WaitForSecondsRealtime(0.5f);

        // [수아] 점수와 학점까지 모두 나온 뒤 버튼 표시
        restartButton.SetActive(true);
        mainButton.SetActive(true);
    }

    // [수아] 타자기 효과 함수
    private IEnumerator TypeText(TextMeshProUGUI targetText, string content)
    {
        targetText.text = "";

        foreach (char letter in content)
        {
            targetText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    // [수아] 배경 설정 함수
    private void SetEndingBackground()
    {
        if (GameManager.currentEndingType == GameManager.EndingType.Clear)
        {
            GameClearBackground.SetActive(true);
            GameOverBackground.SetActive(false);
        }
        else if (GameManager.currentEndingType == GameManager.EndingType.Late)
        {
            GameClearBackground.SetActive(false);
            GameOverBackground.SetActive(true);
        }
    }
}
