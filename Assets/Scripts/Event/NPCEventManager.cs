using System.Collections;
using UnityEngine;

public class NPCEventManager : MonoBehaviour
{
    public float waitTime = 60f;            // [채원] NPC 이벤트가 발생하는 간격 (초 단위)
    public ObstacleSpawner obstacleSpawner; // [채원] 기존 장애물 스포너 연결
    public GameObject npcPrefab;            // [채원] 등장할 인물 프리팹
    public BuffManager buffManager;         // [채원] 버프 매니저 연결

    private float spawnX;

    private void Start()
    {
        // [채원] 인물 등장 위치 X 좌표 (화면 오른쪽 끝 기준)
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // [채원] 게임 시작 후 waitTime마다 반복 실행
        StartCoroutine(NPCEventTriggerRoutine());
    }

    IEnumerator NPCEventTriggerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            TriggerNPCEvent();
        }
    }

    private void TriggerNPCEvent()
    {
        // [채원] 1. 장애물 생성 중지
        obstacleSpawner.StopSpawning();

        // [채원] 2. 3초 대기 후 NPC 등장 (장애물이 안 나오는 상태로 3초 유지)
        StartCoroutine(SpawnNPCRoutine());
    }

    IEnumerator SpawnNPCRoutine()
    {
        yield return new WaitForSeconds(3f);

        Vector3 spawnPos = new Vector3(spawnX, npcPrefab.transform.position.y, npcPrefab.transform.position.z);

        // [채원] NPC 생성
        GameObject npc = Instantiate(npcPrefab, spawnPos, Quaternion.identity);
        NPCController npcScript = npc.GetComponent<NPCController>();
        
        // [채원] NPC 스크립트에 매니저 참조 전달
        if (npcScript != null)
        {
            npcScript.Init(this);
        }
    }

    // [채원] NPC가 화면 밖으로 완전히 사라졌을 때 호출될 함수 (플레이어가 피했을 때)
    public void OnNPCEscaped()
    {
        // [채원] 화면을 벗어난 지 1초 후에 장애물 다시 생성 시작
        obstacleSpawner.StartSpawning(1f);
    }

    // [채원] 플레이어와 충돌해서 버프가 발동되었을 때 호출될 함수
    public void OnNPCHit()
    {
        // [채원] 확률에 따라 버프 부여
        DetermineBuff();

        // [채원] 부딪힌 경우에도 인물은 즉시 사라지므로, 1초 뒤 장애물 재생성
        obstacleSpawner.StartSpawning(1f);
    }

    private void DetermineBuff()
    {
        int rand = Random.Range(0, 100);

        if (rand < 20) // [채원] 20% 확률 (0 ~ 19)
        {
            buffManager.ApplyBuff("Professor");
        }
        else if (rand < 80) // [채원] 60% 확률 (20 ~ 79)
        {
            buffManager.ApplyBuff("Romance");
        }
        else // [채원] 20% 확률 (80 ~ 99)
        {
            buffManager.ApplyBuff("Cult");
        }
    }
}