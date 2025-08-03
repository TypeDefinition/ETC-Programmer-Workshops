using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SemiAutoState", menuName = "FiniteStateMachine/Example1/SemiAutoState")]
public class SemiAutoState : State {
    private FiniteStateMachine finiteStateMachine;
    private Example1Gun gun;

    // Finite State Machine
    public override void OnAwake(FiniteStateMachine fsm, GameObject target) {
        finiteStateMachine = fsm;
        gun = target.GetComponent<Example1Gun>();
        
        gun.AddFireMode(GetStateName());
    }

    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        gun.SetFireMode(GetStateName());
        gun.GetInputActions().Gun.Shoot.performed += OnShoot;
        gun.GetInputActions().Gun.Reload.performed += OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed += OnSwitchFireMode;
    }

    public override void OnExit(FiniteStateMachine fsm, GameObject target) {
        gun.GetInputActions().Gun.Shoot.performed -= OnShoot;
        gun.GetInputActions().Gun.Reload.performed -= OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed -= OnSwitchFireMode;
    }

    // Input Callbacks
    private void OnShoot(InputAction.CallbackContext context) {
        if (gun.GetNumBullets() > 0) {
            gun.Shoot();
        } else {
            finiteStateMachine.ChangeState("Reload");
        }
    }

    private void OnReload(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState("Reload");
    }

    private void OnSwitchFireMode(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState(gun.GetNextFireMode());
    }
}