    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E3Turret : MonoBehaviour
{
    private GameObject monster = null;

    void Start() {
        monster = GameObject.FindWithTag("Monster");
    }

    void Update() {
        transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), GetRotationAngle(), Space.World);
    }

    // Code Snippet
    float GetRotationAngle() {
        // Find our forward direction.
        Vector3 turretForward = new Vector3(transform.forward.x, 0.0f, transform.forward.z).normalized;

        // Find the direction from the turret to the monster.
        Vector3 directionToMonster = monster.transform.position - transform.position;
        directionToMonster.y = 0.0f; // We only want the horizontal direction. Ignore any verticality.
        directionToMonster.Normalize(); // Normalize the direction since we do not need the magnitude.

        // Calculate the angle we need to rotate the turret to face the monster.
        float dotProduct = Vector3.Dot(turretForward, directionToMonster);
        /* Note: Due to floating point precision error, it is possible for the dot product to be greater
         *       than 1 even though the vectors are normalised.
         *       Since the valid range of inputs for acos is [-1, 1], limit dotProduct to be a maximum of 1. */
        float angleToMonster = Mathf.Acos(Mathf.Min(dotProduct, 1.0f)) * Mathf.Rad2Deg;

        // Use the cross product to determine if monster is clockwise or anti-clockwise from where the turret is facing.
        Vector3 crossProduct = Vector3.Cross(turretForward, directionToMonster);

        /* Note that Unity's coordinate system is left-handed.
         * Thus, if the cross product is pointing up, then we need to rotate clockwise. Otherwise, rotate anti-clockwise.
         * In a left-handed coordinate system, a positive rotation on the y-axis rotates clockwise. */
        return (crossProduct.y > 0.0f) ? angleToMonster : -angleToMonster;
    }
}