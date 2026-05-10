using UnityEngine;

/// <summary>
/// 기능
/// : 일정한 시간마다 카메라의 오른쪽 뒤에서 장애물을 소환한다.
/// 
/// 추후 개선 계획
/// : 현재 고정값 0f인 생성위치의 y값(spawnY)은 원하는 위치에 따라 값을 지정.
/// : 현재 오브젝트가 생성되는 시간이 일정한데, 불규칙적이도록 수정해야함.
/// : 다양한 종류의 장애물 오브젝트를 구현해야함.
/// </summary>

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // 생성할 장애물 프리팹. 

    private float spawnX; //생성할 위치의 x값
    private float spawnY = 0f; //생성할 위치의 y값
    private float startDelay = 2f; // 첫 생성 대기 시간
    private float repeatRate = 3f; // 반복 간격 (3초)

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
        Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
    }
}
