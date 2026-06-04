using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // [예린] 게임 방법 화면 패널
    [SerializeField] private GameObject howToPlayPanel;

    // [예린] 게임 방법 버튼 클릭 시 패널 표시
    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    // [예린] 닫기 버튼 클릭 시 패널 숨김
    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
