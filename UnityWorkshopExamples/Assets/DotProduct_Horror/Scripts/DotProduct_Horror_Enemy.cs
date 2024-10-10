using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProduct_Horror_Enemy : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 3.0f;
    [SerializeField] private float acceleration = 5.0f;

    private GameObject player = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float mass = rb.mass;
        float force = mass * acceleration;
        
        rb.AddForce(direction * force);
        if (rb.velocity.sqrMagnitude > maxVelocity * maxVelocity)
            rb.velocity = rb.velocity.normalized * maxVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}
