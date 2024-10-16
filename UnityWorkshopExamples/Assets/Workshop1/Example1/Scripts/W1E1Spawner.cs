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

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Vector3 playerForwardDirection = player.transform.forward;
        bool isBehindPlayer = 0.0f < Vector3.Dot(directionToPlayer, playerForwardDirection);

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
}
