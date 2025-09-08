using UnityEngine;

public class DisableTimer : MonoBehaviour {
    private float timer = 0.0f;

    private void OnEnable() {
        // Just hard code this. It's a quick and dirty demo so who cares?
        timer = 2.0f;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0.0f) {
            gameObject.SetActive(false);
        }
    }
}