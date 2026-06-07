using UnityEngine;

// [예린] UI 버튼 클릭 효과음 재생 스크립트
public class ButtonSound : MonoBehaviour
{
    [Header("Button Sound Settings")]
    [SerializeField] private AudioClip buttonClickSound; // 버튼 클릭 효과음
    private AudioSource audioSource;                     // 효과음 재생용 AudioSource

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // [예린] 버튼 클릭 시 효과음 재생
    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}

