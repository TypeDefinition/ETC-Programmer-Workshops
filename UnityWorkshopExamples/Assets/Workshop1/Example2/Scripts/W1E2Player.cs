using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E2Player : MonoBehaviour
{
    private float patrolRadius = 4.0f;
    private float angularVelocity = 120.0f;
    private float currentAngle = 0.0f;

    private void Start()
    {
    }

    private void Update()
    {
        // Make the player walk in a circle around the origin of the world.
        currentAngle += angularVelocity * Time.deltaTime;
        float xPosition = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float yPosition = transform.position.y;
        float zPosition = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        transform.position = new Vector3(xPosition * patrolRadius, yPosition, zPosition * patrolRadius);
        transform.localRotation = Quaternion.Euler(0, -currentAngle, 0);
    }
}