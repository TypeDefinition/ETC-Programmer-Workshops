using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "FullAutoState", menuName = "FiniteStateMachine/Example1/FullAutoState")]
public class FullAutoState : State {
    private FiniteStateMachine finiteStateMachine;
    private Example1Gun gun;
    private bool isShooting = false;

    // Finite State Machine
    public override void OnAwake(FiniteStateMachine fsm, GameObject target) {
        finiteStateMachine = fsm;
        gun = target.GetComponent<Example1Gun>();

        gun.AddFireMode(GetStateName());
    }

    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        gun.SetFireMode(GetStateName());

        gun.GetInputActions().Gun.Shoot.started += OnShootStart;
        gun.GetInputActions().Gun.Shoot.canceled += OnShootStop;
        gun.GetInputActions().Gun.Reload.performed += OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed += OnSwitchFireMode;

        isShooting = false;
    }

    public override void OnUpdate(FiniteStateMachine fsm, GameObject target) {
        if (isShooting) {
            if (gun.GetNumBullets() > 0) {
                gun.Shoot();
            } else {
                finiteStateMachine.ChangeState("Reload");
            }
        }
    }

    public override void OnExit(FiniteStateMachine fsm, GameObject target) {
        gun.GetInputActions().Gun.Shoot.started -= OnShootStart;
        gun.GetInputActions().Gun.Shoot.canceled -= OnShootStop;
        gun.GetInputActions().Gun.Reload.performed -= OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed -= OnSwitchFireMode;
    }

    // Input Callbacks
    private void OnShootStart(InputAction.CallbackContext context) {
        isShooting = true;
    }

    private void OnShootStop(InputAction.CallbackContext context) {
        isShooting = false;
    }

    private void OnReload(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState("Reload");
    }

    private void OnSwitchFireMode(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState(gun.GetNextFireMode());
    }
}