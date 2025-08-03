using FSM;
using TMPro;
using UnityEngine;

public class Example1GUI : MonoBehaviour {
    [SerializeField] private Example1Gun gun;
    [SerializeField] private FiniteStateMachine fsm;
    [SerializeField] private TextMeshProUGUI bullets;
    [SerializeField] private TextMeshProUGUI currentState;

    void Update() {
        bullets.text = "Bullets: " + gun.GetNumBullets() + "/" + gun.GetMagazineSize();
        currentState.text = "Current State: " + fsm.GetCurrentState();
    }
}