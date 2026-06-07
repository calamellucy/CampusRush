using UnityEngine;

public class SpawnTimingManager : MonoBehaviour
{
    // [수아] 장애물과 아이템 사이에 확보할 최소 시간 간격
    // 예: 0.5초면 장애물 생성 전후 0.5초 안에는 아이템이 생성되지 않도록 함
    [SerializeField] private float minGapBetweenObstacleAndItem = 0.3f;

    // [수아] 마지막으로 장애물이 생성된 시간
    private float lastObstacleSpawnTime = -999f;

    // [수아] 마지막으로 아이템이 생성된 시간
    private float lastItemSpawnTime = -999f;

    // [수아] 장애물을 지금 생성하기 전에 얼마나 기다려야 하는지 계산
    public float GetObstacleWaitTime()
    {
        // [수아] 직전 아이템 스폰 시간에서 얼마나 지났는지 계산하고
        // 장애물과 아이템 최소 간격에서 그 시간을 뺀 값을 반환함
        // 장애물 생성 시 이 값이 양수면 그 값이 장애물 최소 간격 시간까지 남은 시간이므로
        // 그 시간만큼 기다린 후 장애물 생성
        float timeAfterItem = Time.time - lastItemSpawnTime;
        float waitTime = minGapBetweenObstacleAndItem - timeAfterItem;

        return Mathf.Max(0f, waitTime);
    }

    // [수아] 아이템을 지금 생성하기 전에 얼마나 기다려야 하는지 계산
    public float GetItemWaitTime()
    {
        // [수아] 위와 같은 원리
        float timeAfterObstacle = Time.time - lastObstacleSpawnTime;
        float waitTime = minGapBetweenObstacleAndItem - timeAfterObstacle;

        return Mathf.Max(0f, waitTime);
    }

    // [수아] 장애물이 생성되었을 때 호출
    public void RegisterObstacleSpawn()
    {
        lastObstacleSpawnTime = Time.time;
    }

    // [수아] 아이템이 생성되었을 때 호출
    public void RegisterItemSpawn()
    {
        lastItemSpawnTime = Time.time;
    }
}
