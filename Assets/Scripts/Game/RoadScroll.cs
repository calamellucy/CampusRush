using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [Header("Road Tilemaps")]
    public Transform road1;
    public Transform road2;

    [Header("Scroll Settings")]
    public float scrollSpeed = 5f;
    public float extraMargin = 1f;

    private Camera mainCamera;
    private Renderer road1Renderer;
    private Renderer road2Renderer;

    void Start()
    {
        mainCamera = Camera.main;

        if (road1 != null)
            road1Renderer = road1.GetComponent<Renderer>();

        if (road2 != null)
            road2Renderer = road2.GetComponent<Renderer>();
    }

    void Update()
    {
        if (road1 == null || road2 == null || road1Renderer == null || road2Renderer == null)
            return;

        // [수아] 도로 2개를 왼쪽으로 이동
        road1.position += Vector3.left * scrollSpeed * Time.deltaTime;
        road2.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // [수아] 카메라 왼쪽 끝 월드 좌표
        float leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - extraMargin;

        // [수아] road1이 화면 왼쪽 밖으로 완전히 나가면 오른쪽으로 이동
        if (road1Renderer.bounds.max.x < leftEdge)
        {
            MoveRoadToRight(road1, road1Renderer);
        }

        // [수아] road2가 화면 왼쪽 밖으로 완전히 나가면 오른쪽으로 이동
        if (road2Renderer.bounds.max.x < leftEdge)
        {
            MoveRoadToRight(road2, road2Renderer);
        }
    }

    void MoveRoadToRight(Transform road, Renderer roadRenderer)
    {
        float roadWidth = roadRenderer.bounds.size.x;

        // [수아] 도로 2개를 이어붙여 쓰는 구조이므로, 자기 너비의 2배만큼 오른쪽으로 이동
        road.position += Vector3.right * roadWidth * 2f;
    }

}
