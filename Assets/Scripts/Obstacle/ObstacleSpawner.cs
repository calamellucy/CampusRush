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

    void Start()
    {
        //게임이 시작될 때마다 공용 속도를 초기속도로 리셋
        currentSpeed = initialSpeed;

        // 화면 오른쪽 끝(1,0) 좌표를 월드 좌표로 변환 (여유값 +2f 추가))
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // [가영] 코루틴을 시작하는 것으로 변경
        StartCoroutine(SpawnRoutine(startDelay));
    }

    //[수아] 장애물을 일정한 주기마다 생성, 시간이 흐를 수록 장애물 속도 증가 코루틴
    IEnumerator SpawnRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true) // [수아] 시작 대기 후 장애물 생성 반복 실행
        {
            SpawnObstacle(); // 장애물 생성

            // 가속 로직: 장애물 생성할 때마다 공용 속도값 증가 
            currentSpeed += speedIncreaseRate;

            Debug.Log($"<color=yellow>[System]</color> 현재 난이도 - 속도: {currentSpeed:F2}");

            // TODO: 장애물 생성 간격 조정 추가

            yield return new WaitForSeconds(spawnInterval); // 다음 장애물 생성 대기
        }

    }

    void SpawnObstacle()
    {
        // 안전장치: 프리팹이 등록되지 않았다면 실행하지 않음
        if (obstaclePrefabs.Length == 0) return;

        // 0부터 (장애물 개수 - 1) 사이의 랜덤한 인덱스(번호) 선택
        int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        
        //장애물의 프리팹과 위치 담은 오브젝트 생성
        GameObject obj = Instantiate(obstaclePrefabs[randomIndex],              // 선택된 프리팹을 화면 오른쪽 끝(spawnX)에 복제하여 생성
                         new Vector3(spawnX, obstaclePrefabs[randomIndex].transform.position.y, 0), // Y축 위치는 프리팹 설정값을 따르며, Z축은 0으로 고정하고
                         obstaclePrefabs[randomIndex].transform.rotation);      // 프리팹의 회전값을 유지

    }

    // [채원] 장애물 생성을 중지하는 함수
    public void StopSpawning()
    {
        StopAllCoroutines(); // 현재 돌고 있는 모든 생성 코루틴 즉시 정지
    }

    // [채원] 장애물 생성을 재개하는 함수
    public void StartSpawning(float delay)
    {
        StartCoroutine(SpawnRoutine(delay)); // 코루틴을 다시 시작
    }
}
