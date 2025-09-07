using TMPro;
using UnityEngine;

public class GunGUI : MonoBehaviour {
    [SerializeField] private Gun gun;
    [SerializeField] private TextMeshProUGUI bullets;
    [SerializeField] private TextMeshProUGUI currentState;

    private string ToString(Gun.State state) {
        switch (state) {
            case Gun.State.Reload: return "Reload";
            case Gun.State.SemiAuto: return "SemiAuto";
            case Gun.State.FullAuto: return "FullAuto";
            case Gun.State.BurstFire: return "BurstFire";
            default: return "Unhandled State";
        }
    }

    private void Update() {
        bullets.text = "Bullets: " + gun.GetNumBullets() + "/" + gun.GetMagazineSize();
        currentState.text = "Current State: " + ToString(gun.GetCurrentState());
    }
}