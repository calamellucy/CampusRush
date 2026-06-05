using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float destroyTime = 1.0f;
    
    private TextMeshProUGUI textMesh;
    private Color textColor;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        textColor = textMesh.color;
        timer = 0f;
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        textColor.a = Mathf.Lerp(1f, 0f, timer / destroyTime);
        textMesh.color = textColor;
    }

    // [채원] 점수에 따라 색상과 텍스트를 다르게 설정하는 함수
    public void SetScoreText(int amount)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();

        if (amount >= 0)
        {
            textMesh.text = "+" + amount.ToString();
            textMesh.color = Color.yellow;
        }
        else
        {
            textMesh.text = amount.ToString(); 
            textMesh.color = Color.red;
        }
        
        textColor = textMesh.color; 
    }
}