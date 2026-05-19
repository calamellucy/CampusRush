using UnityEngine;

//{구가영}
public class BackgroundScroll : MonoBehaviour
{
    // 배경이 흘러가는 속도
    public float scrollSpeed = 0.2f;

    private MeshRenderer meshRenderer;

    void Start()
    {
        // Quad의 MeshRenderer 컴포넌트
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // 시간에 따라 X축 오프셋 값을 증가
        float offset = Time.time * scrollSpeed;

        // 머티리얼의 메인 텍스처 오프셋을 변경하여 배경을 움직임
        meshRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}