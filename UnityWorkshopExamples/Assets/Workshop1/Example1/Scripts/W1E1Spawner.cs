using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class W1E1Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab = null;

    private GameObject player = null;
    private bool canSpawn = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Vector3 playerForwardDirection = player.transform.forward;
        bool isBehindPlayer = 0.0f < Vector3.Dot(directionToPlayer, playerForwardDirection);

        // We need a flag to prevent the spawner from spawning monsters every single frame.
        // We only want to spawn the monster once until the player looks at the spawner again.
        if (!isBehindPlayer)
            canSpawn = true;

        if (isBehindPlayer && canSpawn)
        {
            canSpawn = false;
            GameObject enemyInstance = GameObject.Instantiate(enemyPrefab);
            // This is just a quick and dirty example. Please do not hardcode in your own projects.
            enemyInstance.transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
        }
    }

    // Code Snippet (Simplified for presentation purposes.)
    private void SpawnMonster() {
        // What is the direction from the spawner to the player?
        Vector3 directionToPlayer = player.transform.position - transform.position;
        // Where is the player facing?
        Vector3 playerForwardDirection = player.transform.forward;
        /* If the player is facing the in general direction of the direction from the
         * spawner to it, then the spawner is behind the player. */
        bool isBehindPlayer = Vector3.Dot(directionToPlayer, playerForwardDirection) > 0.0f;

        if (isBehindPlayer) {
            // Spawn monster...
        }
    }
}
