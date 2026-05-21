using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // [수아] 싱글톤

    private bool isGameOver = false; // [수아] 게임오버 처리 변수

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
        SceneManager.LoadScene("EndScene"); // [수아] 엔딩 씬으로 이동
    }
}
