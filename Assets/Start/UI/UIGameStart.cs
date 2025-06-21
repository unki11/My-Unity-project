using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요

public class UIGameStart : MonoBehaviour
{
    // Inspector에서 할당할 게임 상태 텍스트 UI
    [SerializeField] private TextMeshProUGUI gameStateText;

    void OnEnable()
    {
        // GameManager의 OnGameStateChanged 이벤트에 구독합니다.
        // 이 이벤트가 발생하면 UpdateGameStateUI 함수가 호출됩니다.
        GameManager.OnGameStateChanged += UpdateGameStateUI;
        Debug.Log("UIGameStart GameManager.OnGameStateChanged 이벤트를 구독했습니다.");
    }

    void OnDisable()
    {
        // 이벤트를 구독 해지합니다. (메모리 누수 방지)
        GameManager.OnGameStateChanged -= UpdateGameStateUI;
        Debug.Log("UIGameStart GameManager.OnGameStateChanged 이벤트을 해지했습니다.");
    }

    // 게임 상태가 변경될 때 호출될 함수
    private void UpdateGameStateUI(RoundState newState)
    {
        Debug.Log($"<color=magenta>UIUpdater: OnGameStateChangedHandler 호출됨! 수신된 상태: {newState}</color>");
        Debug.Log($"gameStateText : {gameStateText}");
        if (gameStateText != null)
        {
            // UI 텍스트를 현재 게임 상태에 맞게 업데이트
            gameStateText.text = $"Game state: {newState}";
            Debug.Log("UI 상태 변경." + gameStateText.color);

            // 특정 상태에 따라 UI 색상을 변경하는 예시
            switch (newState)
            {
                case RoundState.Preparation:
                    gameStateText.color = Color.white;
                    break;
                case RoundState.Combat:
                    gameStateText.color = Color.green;
                    break;
                case RoundState.Result:
                    gameStateText.color = Color.yellow;
                    break;
                case RoundState.Intermission:
                    gameStateText.color = Color.red;
                    break;
            }
        }
    }
}
