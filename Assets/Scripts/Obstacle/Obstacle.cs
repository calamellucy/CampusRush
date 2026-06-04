using UnityEngine;

/// <summary>
/// 기능
/// : 일정한 속도로 왼쪽으로 이동하다가, 왼쪽 벽을 넘어가면 객체를 삭제한다.
/// 
/// </summary>

/// {구가영}
public class Obstacle : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float deadZone; // 사라질 왼쪽 X 좌표 경계

    void Start()
    {
        // 화면 왼쪽 끝(0,0) 좌표를 월드 좌표로 변환 (여유값 -2f 추가)
        deadZone = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2f;
    }

    void Update() 
    {
        // [가영] ObstacleSpawner의 static 속도를 참조하여 동일하게 이동
        moveSpeed = ObstacleSpawner.currentSpeed;

        // 매 프레임마다 왼쪽 방향으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 왼쪽 벽(경계선)을 넘어가면 객체 삭제
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
