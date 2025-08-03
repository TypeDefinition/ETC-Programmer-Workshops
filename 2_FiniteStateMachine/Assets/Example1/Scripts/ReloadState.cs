using FSM;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadState", menuName = "FiniteStateMachine/Example1/ReloadState")]
public class ReloadState : State {
    [SerializeField] private float reloadDuration = 2.0f;
    [SerializeField] private float timer = 0.0f;

    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        timer = reloadDuration;
    }

    public override void OnUpdate(FiniteStateMachine fsm, GameObject target) {
        timer -= Time.deltaTime;
        if (timer <= 0.0f) {
            fsm.ChangeState(target.GetComponent<Example1Gun>().GetFireMode());
        }
    }

    public override void OnExit(FiniteStateMachine fsm, GameObject target) {
        target.GetComponent<Example1Gun>().Reload();
    }
}
