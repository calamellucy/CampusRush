using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject professorBuffUI; // [채원] 교수님 버프 UI
    public GameObject romanceBuffUI;   // [채원] 연애 버프 UI
    public GameObject cultBuffUI;      // [채원] 사이비 버프 UI

    public ScoreManager scoreManager; // [수아] 스코어 매니저 연결

    // [예린] NPC 이벤트 효과음 설정
    [Header("Sound Settings")]
    [SerializeField] private AudioClip professorSound; // 교수 이벤트 효과음
    [SerializeField] private AudioClip romanceSound;   // 이성 이벤트 효과음
    [SerializeField] private AudioClip cultSound;      // 사이비 이벤트 효과음
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
        // [채원] 시작할 때는 모든 버프 UI 숨기기
        ClearAllBuffUI();
    }

    public void ApplyBuff(string buffType)
    {
        // [채원] 기존에 돌고 있던 버프 코루틴이 있다면 취소 (새 버프로 갱신 혹은 중첩 처리)
        if (activeBuffCoroutine != null)
        {
            StopCoroutine(activeBuffCoroutine);
            ClearAllBuffUI();
        }

        activeBuffCoroutine = StartCoroutine(BuffDurationRoutine(buffType));
    }

    IEnumerator BuffDurationRoutine(string buffType)
    {
        // [채원] 1. 해당 버프 UI 켜기 및 효과 발동 위치
        switch (buffType)
        {
            case "Professor":
                professorBuffUI.SetActive(true);
                scoreManager.ApplyProfessorEffect(); // [수아] 교수님 효과 적용 함수 호출
                PlayEventSound(professorSound); // [예린] 교수 이벤트 효과음 재생
                Debug.Log("교수님 조우! 학점 압박 버프 발동");
                break;
            case "Romance":
                romanceBuffUI.SetActive(true);
                scoreManager.ApplyRomanceEffect(); // [수아] 연애 효과 적용 함수 호출
                PlayEventSound(romanceSound); // [예린] 연애 이벤트 효과음 재생
                Debug.Log("연애 버프 발동! 발걸음이 가볍습니다.");
                break;
            case "Cult":
                cultBuffUI.SetActive(true);
                scoreManager.ApplyCultEffect(); // [수아] 사이비 효과 적용 함수 호출
                PlayEventSound(cultSound); // [예린] 사이비 이벤트 효과음 재생
                Debug.Log("도를 아십니까? 시간 지체 버프 발동");
                break;
        }

        // [채원] 2. 10초 지속
        yield return new WaitForSeconds(10f);

        // [채원] 3. 버프 종료 및 UI 끄기
        ClearAllBuffUI();
        // TODO: 버프 효과를 원래대로 되돌리는 함수 호출
        scoreManager.ResetScoreEffect(); // [수아] 버프 효과 리셋
        Debug.Log("버프 지속시간이 끝났습니다.");
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
        if(cultBuffUI != null) cultBuffUI.SetActive(false);
    }
}