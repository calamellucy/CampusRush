using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public void OnClickRestart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickMain()
    {
        SceneManager.LoadScene("StartScene");
    }
}
