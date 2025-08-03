using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "FinishState", menuName = "FiniteStateMachine/Example2/FinishState")]
public class FinishState : State {
    // Finite State Machine
    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        target.GetComponent<Example2GameManager>().finishText.gameObject.SetActive(true);
    }
}