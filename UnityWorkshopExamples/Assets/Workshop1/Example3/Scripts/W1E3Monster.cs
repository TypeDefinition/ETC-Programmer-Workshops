using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E3Monster : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 checkpointA = new Vector3(8.0f, 1.0f, 0.0f);
    [SerializeField] private Vector3 checkpointB = new Vector3(-8.0f, 1.0f, 0.0f);

    private void Start() {
    }

    private void Update() {
        // Move between the 2 checkpoints.
        float speedMultiplier = 1.0f;
        float lerpValue = 0.5f * (Mathf.Sin(Time.timeSinceLevelLoad * speedMultiplier) + 1.0f);
        transform.position = Vector3.Lerp(checkpointA, checkpointB, lerpValue);
    }
}
