using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // [가영] 인스펙터에서 아이템 프리팹과 등장 비율(가중치)을 세트로 묶어 관리하기 위한 구조체
    [System.Serializable]
    public struct ItemRate
    {
        public GameObject itemPrefab; // 아이템 프리팹
        public int spawnRatio;        // 등장 비율 가중치 (ex: 50, 30, 20 등 상대적 비율)
    }

    [Header("Item Pool")]
    public ItemRate[] itemPool; // 위 구조체의 배열 (여러 아이템 프리팹과 등장 비율을 담음.)

    // [수아] Inspector에서 값 조정 가능하도록
    [Header("Spawn Settings")]
    [SerializeField] private float spawnX; //생성할 위치의 x값
    [SerializeField] private float spawnY = 0f; //생성할 위치의 y값
    [SerializeField] private float startDelay = 2f; // 첫 생성 대기 시간
    [SerializeField] private float repeatRate = 3f; // 반복 간격 

    void Start()
    {
        // 화면 오른쪽 끝(1,0) 좌표를 월드 좌표로 변환 (여유값 +2f 추가)
        spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;

        // startDelay초 후에 시작하여 repeatRate마다 SpawnItem 함수 실행
        InvokeRepeating("SpawnItem", startDelay, repeatRate);
    }

    /// <summary>
    /// 각 아이템 등장 비율에 따라 랜덤하게 아이템을 호출한다
    /// </summary>
    void SpawnItem()
    {
        if (itemPool == null || itemPool.Length == 0) return;
        // [가영]
        // 1. 전체 가중치(비율)의 총합을 구함
        int totalRatio = 0;
        foreach (ItemRate item in itemPool)
        {
            totalRatio += item.spawnRatio;
        }

        // 2. 0부터 총합 사이의 랜덤 숫자를 하나 뽑음
        int randomValue = UnityEngine.Random.Range(0, totalRatio);

        // 3. 가중치 기반으로 어떤 아이템 프리팹을 뽑을지 결정함
        GameObject selectedItemPrefab = null;
        int currentSum = 0;

        foreach (ItemRate item in itemPool)
        {
            currentSum += item.spawnRatio;
            /*
             * <해당 로직 간단한 설명>
             *  만약 randomValue가 49이고,
             *  item의 spawnRatio가 50,30,10 순이라고 할때,
             *  첫번째 턴에서 currentSum이 50이 되어버리니까
             *  바로 첫 item을 selectedItemPrefab에 넣음!
             */
            if (randomValue < currentSum) 
            {
                selectedItemPrefab = item.itemPrefab;
                break;
            }
        }
        
        // 4. 계산된 spawnX와 고정된 spawnY로 위치에 아이템 오브젝트 생성함
        if (selectedItemPrefab != null)
        {
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
            Instantiate(itemPrefab, spawnPos, selectedItemPrefab.transform.rotation);
        }
    }
}
