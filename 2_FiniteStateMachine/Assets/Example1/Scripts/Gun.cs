using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour {
    [Header("Prefabs & References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;

    [Header("Gun Settings")]
    [SerializeField, Min(1.0f)] private float roundsPerMinute = 450.0f;
    [SerializeField, Min(1)] private int magazineSize = 12;

    // Finite State Machine
    public enum State { Reload, SemiAuto, FullAuto, BurstFire, Num }
    private FSM.FiniteStateMachine fsm = new FSM.FiniteStateMachine((int)State.Num);

    // Private Variables
    private GunInputActions inputActions;
    private int numBullets = 0;
    private float fireCooldown = 0.0f;
    private State fireMode = State.SemiAuto;

    // Reload Variables
    private float reloadDuration = 2.0f;
    private float reloadTimer = 0.0f;

    // Full Auto Variables
    bool isTriggerHeld = false;

    // Burst Fire Variables
    int burstCounter = 0;

    // For GUI
    public int GetMagazineSize() { return magazineSize; }
    public int GetNumBullets() { return numBullets; }
    public State GetCurrentState() { return (Gun.State)fsm.GetCurrentState(); }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    private void Awake() {
        inputActions = new GunInputActions();
        numBullets = magazineSize;

        // Initialise FSM.
        fsm.SetStateEntry((int)State.Reload, OnEnterReload);
        fsm.SetStateUpdate((int)State.Reload, OnUpdateReload);
        fsm.SetStateExit((int)State.Reload, OnExitReload);

        fsm.SetStateEntry((int)State.SemiAuto, OnEnterSemiAuto);
        fsm.SetStateExit((int)State.SemiAuto, OnExitSemiAuto);

        fsm.SetStateEntry((int)State.FullAuto, OnEnterFullAuto);
        fsm.SetStateUpdate((int)State.FullAuto, OnUpdateFullAuto);
        fsm.SetStateExit((int)State.FullAuto, OnExitFullAuto);

        fsm.SetStateEntry((int)State.BurstFire, OnEnterBurstFire);
        fsm.SetStateUpdate((int)State.BurstFire, OnUpdateBurstFire);
        fsm.SetStateExit((int)State.BurstFire, OnExitBurstFire);

        fsm.ChangeState((int)fireMode);
    }

    private void Update() {
        fireCooldown = Mathf.Max(0.0f, fireCooldown - Time.deltaTime);
        fsm.Update();
    }

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    public void Shoot() {
        Instantiate(bulletPrefab).transform.position = bulletSpawnPoint.transform.position;
        fireCooldown = 60.0f / roundsPerMinute;
        --numBullets;
        GetComponent<AudioSource>().Play();
    }

    private void OnPerformedSwitchFireMode(InputAction.CallbackContext context) {
        switch (fireMode) {
            case State.SemiAuto:
                fireMode = State.FullAuto; break;
            case State.FullAuto:
                fireMode = State.BurstFire; break;
            case State.BurstFire:
                fireMode = State.SemiAuto; break;
            default:
                throw new System.Exception("Unhandled fire mode!");
        }
        fsm.ChangeState((int)fireMode);
    }

    // Reload State
    private void OnEnterReload() {
        reloadTimer = reloadDuration;
    }

    private void OnUpdateReload() {
        reloadTimer = Mathf.Max(0.0f, reloadTimer - Time.deltaTime);
        if (reloadTimer == 0.0f) fsm.ChangeState((int)fireMode);
    }

    private void OnExitReload() {
        numBullets = magazineSize;
    }

    private void OnPerformedReload(InputAction.CallbackContext context) { fsm.ChangeState((int)State.Reload); }

    // Semi Auto State
    private void OnEnterSemiAuto() {
        inputActions.Gun.Reload.performed += OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed += OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.performed += OnPerformedShootSemiAuto;
    }

    private void OnExitSemiAuto() {
        inputActions.Gun.Reload.performed -= OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed -= OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.performed -= OnPerformedShootSemiAuto;
    }

    private void OnPerformedShootSemiAuto(InputAction.CallbackContext context) {
        if (0.0f < fireCooldown) { return; }
        if (numBullets == 0) {
            fsm.ChangeState((int)State.Reload);
        } else {
            Shoot();
        }
    }

    // Full Auto State
    private void OnEnterFullAuto() {
        inputActions.Gun.Reload.performed += OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed += OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.started += OnStartedShootFullAuto;
        inputActions.Gun.Shoot.canceled += OnCancelledShootFullAuto;

        isTriggerHeld = false;
    }

    private void OnUpdateFullAuto() {
        if (!isTriggerHeld) { return; }
        if (0.0f < fireCooldown) { return; }
        if (numBullets == 0) {
            fsm.ChangeState((int)State.Reload);
        } else {
            Shoot();
        }
    }

    private void OnExitFullAuto() {
        inputActions.Gun.Reload.performed -= OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed -= OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.started -= OnStartedShootFullAuto;
        inputActions.Gun.Shoot.canceled -= OnCancelledShootFullAuto;
    }

    private void OnStartedShootFullAuto(InputAction.CallbackContext context) { isTriggerHeld = true; }

    private void OnCancelledShootFullAuto(InputAction.CallbackContext context) { isTriggerHeld = false; }

    // Burst Fire State
    private void OnEnterBurstFire() {
        inputActions.Gun.Reload.performed += OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed += OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.performed += OnPerformedShootBurstFire;
    }

    private void OnUpdateBurstFire() {
        if (burstCounter == 0) { return; }
        if (0.0f < fireCooldown) { return; }
        if (numBullets == 0) {
            fsm.ChangeState((int)State.Reload);
        } else {
            Shoot();
            --burstCounter;
        }
    }

    private void OnExitBurstFire() {
        inputActions.Gun.Reload.performed -= OnPerformedReload;
        inputActions.Gun.SwitchFireMode.performed -= OnPerformedSwitchFireMode;
        inputActions.Gun.Shoot.performed -= OnPerformedShootBurstFire;

        burstCounter = 0;
    }

    private void OnPerformedShootBurstFire(InputAction.CallbackContext context) { burstCounter = 3; }
}