using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "BurstFireState", menuName = "FiniteStateMachine/Example1/BurstFireState")]
public class BurstFireState : State {
    private FiniteStateMachine finiteStateMachine;
    private Example1Gun gun;
    private int numShots = 0;

    public override void OnAwake(FiniteStateMachine fsm, GameObject target) {
        finiteStateMachine = fsm;
        gun = target.GetComponent<Example1Gun>();

        gun.AddFireMode(GetStateName());
    }

    // Finite State Machine
    public override void OnEnter(FiniteStateMachine fsm, GameObject target) {
        gun.SetFireMode(GetStateName());

        gun.GetInputActions().Gun.Shoot.performed += OnShoot;
        gun.GetInputActions().Gun.Reload.performed += OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed += OnSwitchFireMode;
        
        numShots = 0;
    }

    public override void OnUpdate(FiniteStateMachine fsm, GameObject target) {
        if (numShots > 0) {
            if (gun.GetNumBullets() == 0) {
                finiteStateMachine.ChangeState("Reload");
            } else if (gun.Shoot()) {
                --numShots;
            }
        }
    }

    public override void OnExit(FiniteStateMachine fsm, GameObject target) {
        gun.GetInputActions().Gun.Shoot.performed -= OnShoot;
        gun.GetInputActions().Gun.Reload.performed -= OnReload;
        gun.GetInputActions().Gun.SwitchFireMode.performed -= OnSwitchFireMode;
    }

    // Input Callbacks
    private void OnShoot(InputAction.CallbackContext context) {
        if (numShots == 0)
            numShots = 3; // Burst size of 3.
    }

    private void OnReload(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState("Reload");
    }

    private void OnSwitchFireMode(InputAction.CallbackContext context) {
        finiteStateMachine.ChangeState(gun.GetNextFireMode());
    }
}