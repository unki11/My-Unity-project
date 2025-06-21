using UnityEngine;
using System.Collections; // Coroutine 사용을 위해 추가
using System; // Action을 사용하기 위해 필요


public enum RoundState
    {
        Preparation, // 준비 페이즈 (유닛 구매/배치)
        Combat,      // 전투 페이즈 (유닛 자동 전투)
        Result,      // 결과 페이즈 (보상, 체력 감소 등)
        Intermission // 다음 라운드 준비 (선택 사항)
    }


public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    // 게임 상태 변경 시 외부에 알리는 이벤트 (현재 상태를 함께 전달)
    public static event Action<RoundState> OnGameStateChanged;

    public RoundState currentRoundState;
    public int currentRoundNumber = 0;

    // 각 페이즈의 지속 시간 (초)
    public float preparationTime = 5f;
    public float combatTime = 10f; // 최대 전투 시간
    public float resultTime = 10f; // 대기 시간

    // 코루틴 참조 (페이즈 중단 시 필요)
    private Coroutine currentPhaseCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {   
            Instance = this;
            // 씬이 바뀌어도 파괴되지 않도록 설정 (선택 사항)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 현재 게임 오브젝트 파괴
            Destroy(gameObject);
        }
    }

    public RoundState CurrentGameState
    {
        get { return currentRoundState; }
        private set
        {
            if (currentRoundState != value) // 상태가 실제로 변경되었을 때만 처리
            {

                Debug.Log($"<color=cyan>GameManager: 상태 변경 시도됨 -> 새: {currentRoundState}</color>");

                if (OnGameStateChanged == null)
                {
                    Debug.LogWarning("<color=red>GameManager: OnGameStateChanged 이벤트에 구독자가 없습니다!</color>");
                }else
                {
                    currentRoundState = value;
                    Debug.Log($"<color=green>GameManager: OnGameStateChanged 이벤트에 {OnGameStateChanged.GetInvocationList().Length}개의 구독된 메서드가 있습니다. Invoke 호출!</color>");
                    OnGameStateChanged.Invoke(currentRoundState); // ?. 제거하고 Invoke로 직접 호출 (확인용)
                }
                Debug.Log($"Game State Changed to: {currentRoundState}");
            }
        }
    }

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        // Debug용: N 키를 누르면 다음 페이즈로 강제 전환 (개발용)
        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToNextPhase();
        }

        // 여기에 현재 상태에 따른 Update 로직을 추가할 수 있습니다.
        // 예를 들어, Preparation 페이즈에서 타이머를 표시하는 등
    }

    public void SetGameState(RoundState newState)
    {
        CurrentGameState = newState;
    }

    // 새 라운드를 시작하는 함수
    public void StartNewRound()
    {
        currentRoundNumber++;
        Debug.Log($"--- 라운드 {currentRoundNumber} 시작 ---");
        ChangeState(RoundState.Preparation);
    }

    // 상태를 변경하는 핵심 함수
    public void ChangeState(RoundState newState)
    {
        // 현재 실행 중인 페이즈 코루틴이 있다면 중지
        if (currentPhaseCoroutine != null)
        {
            StopCoroutine(currentPhaseCoroutine);
        }

        // 이전 상태에서 나갈 때의 로직 (필요하다면)
        // OnExitState(currentRoundState); 

        Debug.Log($"상태 변경 전: {currentRoundState}");
        SetGameState(newState);
        Debug.Log($"상태 변경: {currentRoundState}");

        // 새 상태에 진입할 때의 로직
        // OnEnterState(newState);

        // 각 상태에 맞는 코루틴 시작
        switch (currentRoundState)
        {
            case RoundState.Preparation:
                currentPhaseCoroutine = StartCoroutine(PreparationPhase());
                break;
            case RoundState.Combat:
                currentPhaseCoroutine = StartCoroutine(CombatPhase());
                break;
            case RoundState.Result:
                currentPhaseCoroutine = StartCoroutine(ResultPhase());
                break;
            case RoundState.Intermission:
                currentPhaseCoroutine = StartCoroutine(IntermissionPhase());
                break;
        }
    }

    // 각 페이즈별 코루틴
    IEnumerator PreparationPhase()
    {
        // 준비 페이즈 시작 로직: 상점 활성화, 유닛 배치 UI 등
        Debug.Log("준비 페이즈: 유닛 구매 및 배치");
        
        float timer = preparationTime;
        while (timer > 0)
        {
            // UI에 타이머 표시 (필요하다면)
            // UIManager.Instance.UpdateTimer(timer);
            timer -= Time.deltaTime;
            yield return null; // 1프레임 대기
        }

        // 준비 페이즈 종료 로직: 상점 비활성화, 배치 완료 확인 등
        Debug.Log("준비 페이즈 종료");
        ChangeState(RoundState.Combat); // 다음 페이즈로 전환
    }

    IEnumerator CombatPhase()
    {
        // 전투 페이즈 시작 로직: 유닛 전투 시작, AI 활성화 등
        Debug.Log("전투 페이즈: 유닛 전투");

        // 유닛 전투 시작 알림 (각 유닛들의 AI 활성화)
        // CombatManager.Instance.StartCombat(); 

        float combatTimer = combatTime; // 최대 전투 시간
        bool combatEnded = false;

        while (combatTimer > 0 && !combatEnded)
        {
            // 전투 진행 상황 확인: 한쪽 팀이 전멸했는지 여부
            // combatEnded = CombatManager.Instance.CheckCombatEnd();
            
            combatTimer -= Time.deltaTime;
            yield return null;
        }

        // 전투 종료 로직: 전투 결과 집계, 남은 유닛 정리 등
        Debug.Log("전투 페이즈 종료");
        // CombatManager.Instance.EndCombat(); // 전투 매니저에게 전투 종료 알림

        ChangeState(RoundState.Result);
    }

    IEnumerator ResultPhase()
    {
        // 결과 페이즈 시작 로직: 전투 결과 표시, 골드, 경험치, 체력 반영
        Debug.Log("결과 페이즈: 보상 및 페널티 적용");

        // PlayerManager.Instance.ApplyCombatResults(); // 플레이어 체력, 골드 등 업데이트

        yield return new WaitForSeconds(resultTime); // 일정 시간 대기

        // 결과 페이즈 종료 로직
        Debug.Log("결과 페이즈 종료");
        ChangeState(RoundState.Intermission); // 다음 페이즈 또는 바로 다음 라운드로
    }

    IEnumerator IntermissionPhase()
    {
        Debug.Log("인터미션 페이즈: 잠시 대기 후 다음 라운드 시작");
        yield return new WaitForSeconds(5f); // 5초 대기

        // 게임 종료 조건 확인 (모든 플레이어가 탈락했는지 등)
        // if (GameIsOver())
        // {
        //     Debug.Log("게임 종료!");
        //     // 게임 종료 화면 등으로 전환
        // }
        // else
        // {
            StartNewRound(); // 다음 라운드 시작
        // }
    }

    // 게임 종료 여부를 판단하는 함수 (예시)
    // private bool GameIsOver()
    // {
    //     // 모든 플레이어가 탈락했는지 확인하는 로직
    //     return PlayerManager.Instance.GetActivePlayers().Count <= 1;
    // }

    // 다음 페이즈로 강제 전환 (개발용)
    public void GoToNextPhase()
    {
        switch (currentRoundState)
        {
            case RoundState.Preparation:
                ChangeState(RoundState.Combat);
                break;
            case RoundState.Combat:
                ChangeState(RoundState.Result);
                break;
            case RoundState.Result:
                ChangeState(RoundState.Intermission);
                break;
            case RoundState.Intermission:
                StartNewRound();
                break;
        }
    }
}