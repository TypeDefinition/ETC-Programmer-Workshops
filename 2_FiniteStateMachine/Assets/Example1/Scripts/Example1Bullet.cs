using UnityEngine;

public class Example1Bullet : MonoBehaviour {
    [SerializeField] private float velocity = 10.0f;
    [SerializeField] private float lifeTime = 3.0f;

    void Update() {
        // Move bullet forward.
        transform.Translate(transform.forward * velocity * Time.deltaTime);

        // Delete bullet after a few seconds.
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            Destroy(gameObject);
    }
}