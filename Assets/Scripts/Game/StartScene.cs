using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    // [수아]
    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
        // Start 버튼 클릭 시 MainScene으로 이동
    }

    public void OnClickExit()
    {
        Application.Quit();
        // Exit 버튼 클릭 시 게임 종료
    }
}
