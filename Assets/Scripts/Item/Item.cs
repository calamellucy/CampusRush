using UnityEngine;

public class Item : MonoBehaviour
{
    // [가영]
    [Header("Item Settings")]
    public string itemName = "Default Item";     // 아이템 이름
    public float scoreValue = 100f;              // [예린] 아이템 획득 시 증가할 점수
    public int hpChangeValue = 0;                // HP 변화량 
    public bool isEventMultiplierApplied = false; // 이벤트 배율(ex: 2배 이벤트) 적용 여부

    [Header("Movement")]
    // [수아] Obstacle 코드 복사
    public float moveSpeed = 5f; // 이동 속도
    public float deadZone; // 사라질 왼쪽 X 좌표 경계

    [Header("UI Floating Text Settings")]
    public GameObject floatingText;
    private Transform canvasTransform;

    // [예린] 아이템 획득 효과음 설정
    [Header("Sound Settings")]
    [SerializeField] private AudioClip itemGetSound;
    [SerializeField] private float itemGetSoundVolume = 1.0f; // 아이템 획득 효과음 볼륨

    void Start()
    {
        // 화면 왼쪽 끝(0,0) 좌표를 월드 좌표로 변환 (여유값 -2f 추가)
        deadZone = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2f;
    
        GameObject FXCanvasObj = GameObject.Find("FloatingTextCanvas");

        if (FXCanvasObj != null)
        {
            canvasTransform = FXCanvasObj.transform;
        }
        else
        {
            Debug.LogError("하이어라키에 'FloatingTextCanvas' 라는 이름의 캔버스가 없습니다! 이름을 확인해 주세요.");
        }
    }

    void Update()
    {
        // [가영] ObstacleSpawner의 static 속도를 참조하여 동일하게 이동
        float speed = moveSpeed + ObstacleSpawner.currentSpeedDiff;

        // 매 프레임마다 왼쪽 방향으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime); //fixedDeltaTime사용

        // 왼쪽 벽(경계선)을 넘어가면 객체 삭제
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }

    // [수아] Player와 충돌하면 아이템이 사라지고 획득 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 점수 처리
        if (other.CompareTag("Player"))
        {
            // [예린] 점수 변화가 있는 아이템일 경우 ScoreManager와 연동
            if (scoreValue != 0)
            {
                ScoreManager scoreMgr = FindFirstObjectByType<ScoreManager>();

                float finalCalculatedScore = scoreValue;

                if (scoreMgr != null)
                {
                    finalCalculatedScore = scoreMgr.AddScore(scoreValue);
                }
                else
                {
                    Debug.LogWarning("씬에서 ScoreManager를 찾을 수 없습니다!");
                }

                SpawnFloatingText(finalCalculatedScore);
            }

            // 2. [가영] 라이프(HP) 처리 (PlayerCollisionHandler 연동)
            // [예린] 체력 변화가 있는 아이템일 경우 PlayerCollisionHandler와 연동
            if (hpChangeValue != 0)
            {
                // 충돌한 플레이어 오브젝트나 그 자식/부모에게서 해당 스크립트를 찾음
                PlayerCollisionHandler playerHealth = other.GetComponent<PlayerCollisionHandler>();

                if (playerHealth != null)
                {
                    //예시) 만약 커피라면 하트(체력)가 -1됨. 만약 체력키우는 아이템(ex.하트) 먹으면 하트가 +1된다.
                    playerHealth.ChangeLife(hpChangeValue);
                }
                else
                {
                    Debug.LogWarning("Player에게서 PlayerCollisionHandler 컴포넌트를 찾을 수 없습니다!");
                }
            }

            // [예린] 아이템 획득 효과음 재생
            if (itemGetSound != null)
            {
                Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(itemGetSound, soundPosition, itemGetSoundVolume);
            }

            // 3. [수아] 아이템 오브젝트 파괴
            Destroy(gameObject);
        }
    }

    // [채원] 아이템의 현재 월드 위치를 기반으로 UI 텍스트를 생성하는 함수
    void SpawnFloatingText(float finalScore)
    {
        if (floatingText != null && canvasTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            screenPos.x += 50f; 

            GameObject textObj = Instantiate(floatingText, screenPos, Quaternion.identity, canvasTransform);
            
            FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();
            if (floatingTextScript != null)
            {
                floatingTextScript.SetScoreText((int)finalScore);
            }
        }
    }
}
