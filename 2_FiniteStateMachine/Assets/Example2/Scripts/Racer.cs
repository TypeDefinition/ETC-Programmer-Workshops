using UnityEngine;

public class Racer : MonoBehaviour {
    private float minSpeed = 4.0f;
    private float maxSpeed = 8.0f;
    private float speed = 0.0f;
    private bool isRunning = false;

    private void Awake() {
        speed = Random.Range(minSpeed, maxSpeed);
    }

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