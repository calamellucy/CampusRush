using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // [수아] 충돌한 오브젝트가 Obstacle인지 확인
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle 충돌");

            // 게임 오버
            GameManager.Instance.GameOver();
        }
    }
}
