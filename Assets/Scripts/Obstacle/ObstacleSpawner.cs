using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// ���
/// : ������ �ð����� ī�޶��� ������ �ڿ��� ��ֹ��� ��ȯ�Ѵ�.
/// 
/// ���� ���� ��ȹ
/// : ���� ������Ʈ�� �����Ǵ� �ð��� �����ѵ�, �ұ�Ģ���̵��� �����ؾ��Ҽ���.
/// </summary>

/// {������}
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // 3���� ��ֹ� ������ ���

    private float spawnX; //������ ��ġ�� x��
    public float startDelay = 2f; // ù ���� ��� �ð�
    public float repeatRate = 3f; // �ݺ� ���� (3��)

    private bool isSpawning = true; // [채원] 장애물 생성 여부 제어 변수

    void Start()
    {
        // ȭ�� ������ ��(1,0) ��ǥ�� ���� ��ǥ�� ��ȯ (������ +2f �߰�)
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // startDelay�� �Ŀ� �����Ͽ� repeatRate���� SpawnObstacle �Լ� ����
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (!isSpawning) return; // [채원] 장애물 생성이 비활성화된 경우 함수 종료

        // ������ġ: �������� ��ϵ��� �ʾҴٸ� �������� ����
        if (obstaclePrefabs.Length == 0) return;

        // 0���� (��ֹ� ���� - 1) ������ ������ �ε���(��ȣ) ����
        int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomIndex];

        // X�� ������ �� �ڸ�, Y,Z�� �����տ� ����� ������ ���̸� ���.
        Vector3 spawnPos = new Vector3(spawnX, selectedPrefab.transform.position.y, selectedPrefab.transform.position.z);

        // �ش� ��ġ�� ��ֹ� ������Ʈ ����
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
