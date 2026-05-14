using UnityEngine;
using TMPro; // 점수 가독성을 위해 TMPro 사용

public class EndScoreUI : MonoBehaviour
{
    // [예린] EndScene 점수 표시 기능 구현
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // PlayerPrefs에서 저장된 점수 불러오기
        int score = PlayerPrefs.GetInt("FinalScore", 0);

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score.ToString();
        }
        else
        {
            Debug.LogWarning("FinalScoreText가 연결되지 않았습니다!");
        }
    }
}
