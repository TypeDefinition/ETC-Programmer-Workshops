using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E4Laser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer[] lines = new LineRenderer[0];

    [Header("Settings")]
    [SerializeField] private float angularVelocity = 5.0f;

    private void Start() {}

    private void Update() {
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime, Space.World);
        SetLasers();
    }

    private void SetLasers() {
        const int maxLines = 3;
        const float maxDistance = 100.0f;

        foreach (LineRenderer line in lines)
            line.enabled = false;

        Vector3 startPos = transform.position; // Where the line starts.
        Vector3 direction = transform.forward; // Where the line points towards.

        // Let's render each laser segment.
        for (int i = 0; i < maxLines; ++i) {
            LineRenderer line = lines[i];
            line.enabled = true;

            // Check if the laser hits a surface.
            RaycastHit hit;
            if (!Physics.Raycast(startPos, direction, out hit, maxDistance)) {
                // If there is no surface, just render a long straight line in the current direction.
                line.SetPositions(new Vector3[] { startPos, startPos + direction * maxDistance });
                break;
            }
            // If a surface exists, render a line from the current point to the surface.
            line.SetPositions(new Vector3[] { startPos, hit.point });
            
            // Update the start position for the next line segment.
            startPos = hit.point;

            /* Find the projection of the player's forward direction onto the normal of the surface.
             * Note that both transform.forward and hit.normal are unit vectors. */
            Vector3 projection = Vector3.Project(direction, hit.normal);
            /* Calculate the reflection of the ray off the surface.
             * Note that the reflection direction will also be a unit vector. */
            direction -= projection * 2.0f;
        }
    }
}
