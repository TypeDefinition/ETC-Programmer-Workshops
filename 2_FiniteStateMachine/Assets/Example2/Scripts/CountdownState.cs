using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CountdownState", menuName = "FiniteStateMachine/Example2/CountdownState")]
public class CountdownState : State {
    private float timer = 2.0f;

    // Finite State Machine
    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        timer = 2.0f;
    }

    public override void OnUpdate(FiniteStateMachine fsm, GameObject target) {
        Example2GameManager gameManager = target.GetComponent<Example2GameManager>();

        timer -= Time.deltaTime;
        if (timer > 1.0f) {
            gameManager.readyText.gameObject.SetActive(true);
            gameManager.goText.gameObject.SetActive(false);
        } else if (timer > 0.0f) {
            gameManager.readyText.gameObject.SetActive(false);
            gameManager.goText.gameObject.SetActive(true);
        } else {
            gameManager.readyText.gameObject.SetActive(false);
            gameManager.goText.gameObject.SetActive(false);
            fsm.ChangeState("Race");
        }
    }
}