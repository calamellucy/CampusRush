using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // 생성할 아이템 프리팹. 

    // [수아] Inspector에서 값 조정 가능하도록
    [SerializeField] private float spawnX; //생성할 위치의 x값
    [SerializeField] private float spawnY = 0f; //생성할 위치의 y값
    [SerializeField] private float startDelay = 2f; // 첫 생성 대기 시간
    [SerializeField] private float repeatRate = 3f; // 반복 간격 

    void Start()
    {
        // 화면 오른쪽 끝(1,0) 좌표를 월드 좌표로 변환 (여유값 +2f 추가)
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // startDelay초 후에 시작하여 repeatRate마다 SpawnObstacle 함수 실행
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        // 계산된 spawnX와 초기 설정된 spawnY로 위치 저장
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        // 해당 위치에 장애물 오브젝트 생성
        Instantiate(itemPrefab, spawnPos, itemPrefab.transform.rotation);
    }
}
