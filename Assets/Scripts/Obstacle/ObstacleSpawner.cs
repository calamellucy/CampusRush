using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 기능
/// : 일정한 시간마다 카메라의 오른쪽 뒤에서 장애물을 소환한다.
/// 
/// 추후 개선 계획
/// : 현재 오브젝트가 생성되는 시간이 일정한데, 불규칙적이도록 수정해야할수도.
/// </summary>

/// {구가영}
public class ObstacleSpawner : MonoBehaviour 
{
    public GameObject[] obstaclePrefabs; // 3가지 장애물 프리팹 담기

    //{가영} 장애물 등 모든 오브젝트와 배경의 속도를 점직적으로 올리기 위한 변수
    public static float currentSpeed = 5f;        // 현재 공용 속도
    public float initialSpeed = 5f; //장애물 초기 속도 설정값 (게임 재시작시 처음속도로 돌아가기위함)
    public float speedIncreaseRate = 0.2f; // 점진적 가속도
    public float spawnInterval = 3f;       // 현재 생성 간격

    private float spawnX; // 생성할 위치의 x값
    public float startDelay = 2f; // 첫 생성 대기 시간
    public float repeatRate = 3f; // 반복 간격 (3초)

    private bool isSpawning = true; // [채원] 장애물 생성 여부 제어 변수

    void Start()
    {
        //게임이 시작될 때마다 공용 속도를 초기속도로 리셋
        currentSpeed = initialSpeed;

        // 화면 오른쪽 끝(1,0) 좌표를 월드 좌표로 변환 (여유값 +2f 추가))
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // [가영] 코루틴을 시작하는 것으로 변경
        StartCoroutine(SpawnRoutine());
    }

    //[가영] 장애물을 일정한 주기마다 생성하며, 시간이 흐를수록 난이도(공용 속도)를 높이는 코루틴
    IEnumerator SpawnRoutine()
    {
        //장애물 생성 플래그(isSpawning)가 true인 동안 반복 실행
        while (isSpawning)
        {
            //장애물 생성 함수 호출
            SpawnObstacle();

            // 가속 로직: 장애물 생성할 때마다 공용 속도값 증가 
            currentSpeed += speedIncreaseRate;

            // 디버그 로그 (콘솔창에 상태 변화 출력)
            Debug.Log($"<color=yellow>[System]</color> 현재 난이도 - 속도: {currentSpeed:F2}");

            // 속도에 비례하여 생성 간격을 조정
            float currentInterval = spawnInterval + (currentSpeed * 0.1f);
            //대기: 지정된 생성 간격만큼 다음 장애물 생성을 대기
            yield return new WaitForSeconds(currentInterval);
        }

    }

    void SpawnObstacle()
    {
        if (!isSpawning) return; // [채원] 장애물 생성이 비활성화된 경우 함수 종료

        // 안전장치: 프리팹이 등록되지 않았다면 실행하지 않음
        if (obstaclePrefabs.Length == 0) return;

        // 0부터 (장애물 개수 - 1) 사이의 랜덤한 인덱스(번호) 선택
        int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        
        //장애물의 프리팹과 위치 담은 오브젝트 생성
        GameObject obj = Instantiate(obstaclePrefabs[randomIndex],              // 선택된 프리팹을 화면 오른쪽 끝(spawnX)에 복제하여 생성
                         new Vector3(spawnX, obstaclePrefabs[randomIndex].transform.position.y, 0), // Y축 위치는 프리팹 설정값을 따르며, Z축은 0으로 고정하고
                         obstaclePrefabs[randomIndex].transform.rotation);      // 프리팹의 회전값을 유지

        // 생성된 장애물에 현재 속도 주입
        Obstacle obstacleScript = obj.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            obstacleScript.SetSpeed(currentSpeed);
        }
    }

    // [채원] 장애물 생성을 중지하는 함수
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines(); // 현재 돌고 있는 모든 생성 코루틴 즉시 정지
    }

    // [채원] 장애물 생성을 재개하는 함수
    public void StartSpawning(float delay)
    {
        isSpawning = true;
        StartCoroutine(ResumeSpawningRoutine(delay));
        StartCoroutine(SpawnRoutine()); // 코루틴을 다시 시작
    }

    private IEnumerator ResumeSpawningRoutine(float delay)
    {
        yield return new WaitForSeconds(delay); // [채원] 지연 시간 대기
        isSpawning = true; // [채원] 장애물 생성 재개
    }
}
