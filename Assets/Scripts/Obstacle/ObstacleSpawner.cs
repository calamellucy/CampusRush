using System;
using UnityEngine;
using System.Collections;

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

    private float spawnX; // 생성할 위치의 x값
    public float startDelay = 2f; // 첫 생성 대기 시간
    public float repeatRate = 3f; // 반복 간격 (3초)

    private bool isSpawning = true; // [채원] 장애물 생성 여부 제어 변수

    void Start()
    {
        // 화면 오른쪽 끝(1,0) 좌표를 월드 좌표로 변환 (여유값 +2f 추가))
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // startDelay초 후에 시작하여 repeatRate초마다 SpawnObstacle 함수 실행
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (!isSpawning) return; // [채원] 장애물 생성이 비활성화된 경우 함수 종료

        // 안전장치: 프리팹이 등록되지 않았다면 실행하지 않음
        if (obstaclePrefabs.Length == 0) return;

        // 0부터 (장애물 개수 - 1) 사이의 랜덤한 인덱스(번호) 선택
        int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomIndex];

        // X는 오른쪽 벽 뒤를, Y,Z는 프리팹에 저장된 각자의 높이를 사용.
        Vector3 spawnPos = new Vector3(spawnX, selectedPrefab.transform.position.y, selectedPrefab.transform.position.z);

        // 해당 위치에 장애물 오브젝트 생성
        Instantiate(selectedPrefab, spawnPos, selectedPrefab.transform.rotation);
    }
    
    // [채원] 장애물 생성을 중지하는 함수
    public void StopSpawning()
    {
        isSpawning = false;
    }   

    // [채원] 장애물 생성을 재개하는 함수
    public void StartSpawning(float delay)
    { 
        StartCoroutine(ResumeSpawningRoutine(delay));
    }

    private IEnumerator ResumeSpawningRoutine(float delay)
    {
        yield return new WaitForSeconds(delay); // [채원] 지연 시간 대기
        isSpawning = true; // [채원] 장애물 생성 재개
    }
}
