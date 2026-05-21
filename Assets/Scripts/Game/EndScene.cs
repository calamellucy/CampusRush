using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // [수아]

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
}
