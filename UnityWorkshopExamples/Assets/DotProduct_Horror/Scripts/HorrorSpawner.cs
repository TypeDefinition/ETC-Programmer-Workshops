using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HorrorSpawner : MonoBehaviour
{
    private GameObject player = null;
    private MeshRenderer meshRenderer = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
    }

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Vector3 playerForwardDirection = player.transform.forward;
        meshRenderer.enabled = (0.0f < Vector3.Dot(directionToPlayer, playerForwardDirection));
    }
}
