using UnityEngine;

public class Item : MonoBehaviour
{
    // [수아] Obstacle 코드 복사

    public float moveSpeed = 5f; // 이동 속도
    public float deadZone; // 사라질 왼쪽 X 좌표 경계
    public int scoreValue = 100; // [예린] 아이템 획득 시 증가할 점수 설정

    void Start()
    {
        // 화면 왼쪽 끝(0,0) 좌표를 월드 좌표로 변환 (여유값 -2f 추가)
        deadZone = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2f;
    }

    void Update()
    {
        // 매 프레임마다 왼쪽 방향으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 왼쪽 벽(경계선)을 넘어가면 객체 삭제
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }

    // [수아] Player와 충돌하면 아이템이 사라지고 획득 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // [예린] 아이템 획득 처리 추가
            ScoreManager scoreMgr = FindFirstObjectByType<ScoreManager>();
            if (scoreMgr != null)
            {
                scoreMgr.AddScore(scoreValue);
            }
            else
            {
                Debug.LogWarning("씬에서 ScoreManager를 찾을 수 없습니다!");
            }

            // [수아] 아이템 사라짐
            Destroy(gameObject);
        }
    }
}
