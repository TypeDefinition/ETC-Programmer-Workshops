using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RaceManager : MonoBehaviour {
    [Header("References")]
    public TextMeshProUGUI readyText = null;
    public TextMeshProUGUI goText = null;
    public TextMeshProUGUI finishText = null;
    [SerializeField] private Racer[] racers = new Racer[0];

    public enum State { Countdown, Race, Finish, Num }
    private FSM.FiniteStateMachine fsm = new FSM.FiniteStateMachine((int)State.Num);

    // Countdown Variables
    float countdownTimer = 0.0f;

    private void Awake() {
        // Initialise Finite State Machine
        fsm.SetStateEntry((int)State.Countdown, OnEnterCountdown);
        fsm.SetStateUpdate((int)State.Countdown, OnUpdateCountdown);

        fsm.SetStateEntry((int)State.Race, OnEnterRace);
        fsm.SetStateUpdate((int)State.Race, OnUpdateRace);

        fsm.SetStateEntry((int)State.Finish, OnEnterFinish);

        fsm.ChangeState((int)State.Countdown);
    }

    private void Update() {
        fsm.Update();
    }

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    // Countdown State
    private void OnEnterCountdown() {
        countdownTimer = 2.0f;
    }

    private void OnUpdateCountdown() {
        countdownTimer = Mathf.Max(0.0f, countdownTimer - Time.deltaTime);
        if (countdownTimer > 1.0f) {
            readyText.gameObject.SetActive(true);
            goText.gameObject.SetActive(false);
        } else if (countdownTimer > 0.0f) {
            readyText.gameObject.SetActive(false);
            goText.gameObject.SetActive(true);
        } else {
            readyText.gameObject.SetActive(false);
            goText.gameObject.SetActive(false);
            fsm.ChangeState((int)State.Race);
        }
    }

    // Race State
    private void OnEnterRace() {
        for (int i = 0; i < racers.Length; ++i) {
            racers[i].StartRunning();
        }
    }

    private void OnUpdateRace() {
        bool isFinish = true;
        for (int i = 0; i < racers.Length; ++i) {
            isFinish = isFinish && !racers[i].IsRunning();
        }

        if (isFinish) {
            fsm.ChangeState((int)State.Finish);
        }
    }

    // Finish State
    private void OnEnterFinish() {
        finishText.gameObject.SetActive(true);
    }
}