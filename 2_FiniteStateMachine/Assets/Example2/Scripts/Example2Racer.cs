using UnityEngine;

public class Example2Racer : MonoBehaviour {
    [SerializeField] private float speed = 5.0f;
    private bool isRunning = false;

    private void Update() {
        if (isRunning) {
            transform.Translate(transform.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        isRunning = false;
    }

    public void StartRunning() { isRunning = true; }
    public bool IsRunning() { return isRunning; }
}