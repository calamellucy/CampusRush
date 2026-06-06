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

    public ScoreManager scoreManager; // [수아] 스코어 매니저 연결

    private Coroutine activeBuffCoroutine;

    void Start()
    {
        if (buffVisualObject != null)
        {
            buffAnimator = buffVisualObject.GetComponent<Animator>();
            
            if (buffAnimator == null)
            {
                Debug.LogError($"{buffVisualObject.name} 오브젝트에 Animator 컴포넌트가 없습니다! 확인해 주세요.");
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
        if (buffVisualObject != null) buffVisualObject.SetActive(true);

        // [채원] 1. 해당 버프 UI 켜기 및 효과 발동 위치
        switch (buffType)
        {
            case "Professor":
                professorBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_professor"); // [채원] 교수님 버프 애니메이션 재생
                scoreManager.ApplyProfessorEffect(); // [수아] 교수님 효과 적용 함수 호출
                Debug.Log("교수님 조우! 학점 압박 버프 발동");
                break;
            case "Romance":
                romanceBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_romance"); // [채원] 연애 버프 애니메이션 재생
                scoreManager.ApplyRomanceEffect(); // [수아] 연애 효과 적용 함수 호출
                Debug.Log("연애 버프 발동! 발걸음이 가볍습니다.");
                break;
            case "Gang":
                gangBuffUI.SetActive(true);
                if (buffAnimator != null) buffAnimator.Play("Buff_gang"); // [채원] 깡패 버프 애니메이션 재생
                scoreManager.ApplyGangEffect(); // [수아] 깡패 효과 적용 함수 호출
                Debug.Log("도를 아십니까? 시간 지체 버프 발동");
                break;
        }

        // [채원] 2. 10초 지속
        yield return new WaitForSeconds(10f);

        // [채원] 3. 버프 종료 및 UI 끄기
        ClearAllBuffUI();
        StopBuffVisual();
        scoreManager.ResetScoreEffect(); // [수아] 버프 효과 리셋
        Debug.Log("버프 지속시간이 끝났습니다.");
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
}