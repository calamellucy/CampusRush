using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject professorBuffUI; // [채원] 교수님 버프 UI
    public GameObject romanceBuffUI;   // [채원] 연애 버프 UI
    public GameObject gangBuffUI;      // [채원] 깡패 버프 UI

    [Header("Player Visuals")]
    public GameObject buffVisualObject; // [채원] 머리 위 아이콘 오브젝트 자체를 연결
    private Animator buffAnimator;       // [채원] 플레이어 머리 위 아이콘 애니메이터
    private SpriteRenderer buffSpriteRenderer; // [채원] 알파값 조절을 위해 스프라이트 렌더러 추가

    public ScoreManager scoreManager; // [수아] 스코어 매니저 연결

    // [예린] NPC 이벤트 효과음 설정
    [Header("Sound Settings")]
    [SerializeField] private AudioClip professorSound; // 교수 이벤트 효과음
    [SerializeField] private AudioClip romanceSound;   // 이성 이벤트 효과음
    [SerializeField] private AudioClip gangSound;      // 사이비 이벤트 효과음
    [SerializeField] private float eventSoundVolume = 1f;

    private AudioSource audioSource; // [예린] 효과음 재생용 AudioSource
    private Coroutine activeBuffCoroutine;

    private void Awake()
    {
        // [예린] BuffManager 오브젝트에 붙어 있는 AudioSource 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (buffVisualObject != null)
        {
            buffAnimator = buffVisualObject.GetComponent<Animator>();
            buffSpriteRenderer = buffVisualObject.GetComponent<SpriteRenderer>();

            if (buffAnimator == null)
            {
                Debug.LogError($"Animator 컴포넌트가 없음");
            }
        }
        // [채원] 시작할 때는 모든 버프 UI 숨기기
        ClearAllBuffUI();
        StopBuffVisual();
    }

    public void ApplyBuff(string buffType)
    {
        // [채원] 기존에 돌고 있던 버프 코루틴이 있다면 취소 (새 버프로 갱신 혹은 중첩 처리)
        if (activeBuffCoroutine != null)
        {
            StopCoroutine(activeBuffCoroutine);
            ClearAllBuffUI();
            StopBuffVisual();
        }

        activeBuffCoroutine = StartCoroutine(BuffDurationRoutine(buffType));
    }

    IEnumerator BuffDurationRoutine(string buffType)
    {
        if (buffVisualObject != null) {
            SetSpriteAlpha(1f);
            buffVisualObject.SetActive(true);
        }

        // [채원] 1. 해당 버프 UI 켜기 및 효과 발동 위치
        switch (buffType)
        {
            case "Professor":
                professorBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_professor"); // [채원] 교수님 버프 애니메이션 재생
                scoreManager.ApplyProfessorEffect(); // [수아] 교수님 효과 적용 함수 호출
                PlayEventSound(professorSound); // [예린] 교수 이벤트 효과음 재생
                break;
            case "Romance":
                romanceBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_romance"); // [채원] 연애 버프 애니메이션 재생
                scoreManager.ApplyRomanceEffect(); // [수아] 연애 효과 적용 함수 호출
                PlayEventSound(romanceSound); // [예린] 연애 이벤트 효과음 재생
                break;
            case "Gang":
                gangBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_gang"); // [채원] 깡패 버프 애니메이션 재생
                scoreManager.ApplyGangEffect(); // [수아] 깡패 효과 적용 함수 호출
                PlayEventSound(gangSound); // [예린] 깡패 이벤트 효과음 재생
                break;
        }
        // [채원] 2. 처음 7초 동안은 정상 유지
        yield return new WaitForSeconds(7f);

        // [채원] 3. 마지막 3초 동안 알파값을 활용한 깜빡임 처리
        float blinkDuration = 3f;
        float elapsed = 0f;
        float blinkSpeed = 10f; // 깜빡임 속도 (숫자가 클수록 더 빠르게 번쩍임)

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;

            float lerpedAlpha = (Mathf.Sin(elapsed * blinkSpeed) + 1f) * 0.5f;

            SetSpriteAlpha(lerpedAlpha);

            yield return null;
        }

        // [채원] 4. 버프 종료 및 UI 끄기
        ClearAllBuffUI();
        StopBuffVisual();
        scoreManager.ResetScoreEffect(); // [수아] 버프 효과 리셋
    }

    // [예린] NPC 이벤트 종류에 맞는 효과음 재생
    private void PlayEventSound(AudioClip soundClip)
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip, eventSoundVolume);
        }
    }

    private void ClearAllBuffUI()
    {
        if(professorBuffUI != null) professorBuffUI.SetActive(false);
        if(romanceBuffUI != null) romanceBuffUI.SetActive(false);
        if(gangBuffUI != null) gangBuffUI.SetActive(false);
    }

    // [채원] 버프 시각 효과 끄는 함수
    private void StopBuffVisual()
    {
        if (buffVisualObject != null) 
        {
            buffVisualObject.SetActive(false);
        }
    }

    // [채원] 스프라이트의 알파값만 안전하게 변경해 주는 헬퍼 함수
    private void SetSpriteAlpha(float alpha)
    {
        if (buffSpriteRenderer != null)
        {
            Color color = buffSpriteRenderer.color;
            color.a = alpha;
            buffSpriteRenderer.color = color;
        }
    }
}