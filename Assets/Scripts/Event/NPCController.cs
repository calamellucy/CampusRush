using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed = 4f; // [채원] NPC가 다가오는 속도
    private NPCEventManager eventManager;
    private bool isTriggered = false;

    private float escapeX;

    public void Init(NPCEventManager manager)
    {
        eventManager = manager;
    }

    void Start()
    {
        escapeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2f;
    }

    void Update()
    {
        // [채원] 왼쪽 방향으로 천천히 이동
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // [채원] 화면 왼쪽 끝을 벗어났는지 체크 (화면 좌표 기준)
        if (transform.position.x < escapeX && !isTriggered)
        {
            isTriggered = true;
            eventManager.OnNPCEscaped();
            Destroy(gameObject);
        }
    }

    // [채원] NPC가 플레이어와 충돌했을 때 호출되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered) return;

        // [채원] 플레이어 레이어나 태그 확인
        if (collision.CompareTag("Player"))
        {
            isTriggered = true;
            eventManager.OnNPCHit();
            Destroy(gameObject); // [채원] 부딪히면 사라짐
        }
    }
}