using UnityEngine;
using TMPro; // TextMeshPro ��� �� �ʿ�
using System.Collections.Generic; // 동적 라이프 관리를 위한 List 네임스페이스 추가

public class UIManager : MonoBehaviour
{
    // [채원] 플레이어가 접근할 수 있도록 싱글톤 인스턴스 추가
    public static UIManager Instance { get; private set; }

    [Header("Score UI Settings")]
    // [����] ���� UI ������Ʈ�� ���� ������ �Լ� �߰�
    public TextMeshProUGUI scoreText;

    [Header("Dynamic Life UI Settings")]
    public GameObject heartPrefab;    // 생성할 하트 프리팹 원본
    public Transform lifePanel;       // 하트가 들어갈 부모 위치

    // [채원] 생성된 하트 오브젝트들을 추적할 리스트
    private List<GameObject> activeHearts = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // [채원] 플레이어의 현재 생명 수에 따라 하트 UI를 동적으로 업데이트하는 메서드
    public void UpdateLifeUI(int currentLives)
    {
        if (heartPrefab == null || lifePanel == null) return;

        // [채원] 1. 현재 화면의 하트 개수가 부족하면 -> 부족한 만큼 생성
        while (activeHearts.Count < currentLives)
        {
            GameObject newHeart = Instantiate(heartPrefab, lifePanel, false);

            newHeart.transform.localPosition = Vector3.zero;
            newHeart.transform.localRotation = Quaternion.identity;
            newHeart.transform.localScale = Vector3.one; // 크기 비율을 (1, 1, 1)로 고정!
            
            activeHearts.Add(newHeart);
        }

        // [채원] 2. 현재 화면의 하트 개수가 넘치면 -> 넘치는 만큼 삭제
        while (activeHearts.Count > currentLives)
        {
            GameObject heartToDestroy = activeHearts[activeHearts.Count - 1];
            activeHearts.RemoveAt(activeHearts.Count - 1);
            Destroy(heartToDestroy);
        }
    }
}
