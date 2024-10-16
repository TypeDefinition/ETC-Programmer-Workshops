using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1E2Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color detectedColor = Color.red;
    [SerializeField] private Color undetectedColor = Color.white;
    private float detectionRange = 7.5f;
    private float detectionAngle = 45.0f; // Hardcoded to fit the detection radius texture.

    [Header("References")]
    [SerializeField] private GameObject detectionRadiusObject = null;
    private GameObject player = null;

    // Start is called before the first frame update
    private void Start()
    {
        detectionRadiusObject.transform.localScale = new Vector3(detectionRange * 2.0f, detectionRange * 2.0f, 1.0f);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the direction and distance to the player.
        Vector3 directionToPlayer = (player.transform.position - transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;

        // If the player is beyond our detection range, it is not detected.
        if (detectionRange < distanceToPlayer)
        {
            detectionRadiusObject.GetComponent<MeshRenderer>().material.color = undetectedColor;
            return;
        }

        // Get the angle to the player.
        float angleToPlayer = Mathf.Acos(Vector3.Dot(transform.forward, directionToPlayer.normalized)) * Mathf.Rad2Deg;

        // If the player is out of our detection angle, it is not detected.
        if (angleToPlayer < detectionAngle)
            detectionRadiusObject.GetComponent<MeshRenderer>().material.color = detectedColor;
        else
            detectionRadiusObject.GetComponent<MeshRenderer>().material.color = undetectedColor;
    }
}