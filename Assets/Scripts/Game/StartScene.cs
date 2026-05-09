using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
