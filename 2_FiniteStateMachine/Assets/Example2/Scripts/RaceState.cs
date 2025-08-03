using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RaceState", menuName = "FiniteStateMachine/Example2/RaceState")]
public class RaceState : State {
    // Finite State Machine
    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        // Tell the racers to start running.
        Example2Racer[] racers = target.GetComponent<Example2GameManager>().GetRacers();
        for (int i = 0; i < racers.Length; ++i) {
            racers[i].StartRunning();
        }
    }

    public override void OnUpdate(FiniteStateMachine fsm, GameObject target) {
        bool isFinish = true;
        Example2Racer[] racers = target.GetComponent<Example2GameManager>().GetRacers();
        for (int i = 0; i < racers.Length; ++i) {
            isFinish = isFinish && !racers[i].IsRunning();
        }

        if (isFinish) {
            fsm.ChangeState("Finish");
        }
    }
}