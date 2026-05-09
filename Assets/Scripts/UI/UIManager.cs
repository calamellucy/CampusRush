using UnityEngine;
using UnityEngine.UI; // Text 사용 시 필요

public class UIManager : MonoBehaviour
{
    // [예린] 점수 UI 업데이트를 위한 변수와 함수 추가
    public Text scoreText;

    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
