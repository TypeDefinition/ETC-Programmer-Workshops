using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E4Laser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer lineForward;
    [SerializeField] private LineRenderer lineRicochet1;
    [SerializeField] private LineRenderer lineRicochet2;

    [Header("Rotation")]
    [SerializeField] private float angularVelocity = 60.0f;

    private void Start() {}

    private void Update() {
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime, Space.World);
        SetRicochetLines();
    }

    private void SetRicochetLines() {
        const float maxDistance = 100.0f;
        lineRicochet1.enabled = false;
        lineRicochet2.enabled = false;

        // **** Let's render a line in the forward direction of the player to the nearest surface. **** //
        lineForward.enabled = true;
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;

        // First, check for a surface in front of the player.
        RaycastHit hit;
        if (!Physics.Raycast(startPos, direction, out hit, maxDistance)) {
            // If there is no surface, just render a really long straight line in the player's forward direction.
            lineForward.SetPositions(new Vector3[] { startPos, startPos + direction * maxDistance });
            return;
        }
        // If a surface exists, render a line from the player to where the raycast hit.
        lineForward.SetPositions(new Vector3[] { startPos, hit.point });
        // ******************************************************************************************** //

        // **** Next, let's render a line showing the direction the bullet will ricochet. **** //
        lineRicochet1.enabled = true;
        startPos = hit.point;

        /* Find the projection of the player's forward direction onto the normal of the surface.
         * Note that both transform.forward and hit.normal are unit vectors. */
        Vector3 projection = Vector3.Project(transform.forward, hit.normal);
        /* Calculate the reflection of the ray off the surface.
         * Note that the reflection direction will also be a unit vector. */
        direction -= projection * 2.0f;

        // Check for another surface in the direction of the ricochet.
        if (!Physics.Raycast(startPos, direction, out hit, maxDistance)) {
            lineRicochet1.SetPositions(new Vector3[] { startPos, startPos + direction * maxDistance });
            lineRicochet2.enabled = false;
            return;
        }
        lineRicochet1.SetPositions(new Vector3[] { startPos, hit.point });
        // ************************************************************************************ //

        // **** Finally, let's render a line showing the direction the bullet will ricochet the second time. **** //
        lineRicochet2.enabled = true;
        startPos = hit.point;

        projection = Vector3.Project(direction, hit.normal);
        direction -= projection * 2.0f;
        
        // Check for another surface in the direction of the ricochet.
        if (!Physics.Raycast(startPos, direction, out hit, maxDistance)) {
            lineRicochet2.SetPositions(new Vector3[] { startPos, startPos + direction * maxDistance });
            return;
        }
        lineRicochet2.SetPositions(new Vector3[] { startPos, hit.point });
        // ****************************************************************************************************** //
    }
}
