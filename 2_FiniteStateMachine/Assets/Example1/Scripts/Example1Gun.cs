using System.Collections.Generic;
using UnityEngine;

public class Example1Gun : MonoBehaviour {
    [Header("Prefabs & References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;

    [Header("Gun Settings")]
    [SerializeField, Min(1.0f)] private float fireRate = 5.0f;
    [SerializeField, Min(1)] private int magazineSize = 12;

    // Private Variables
    private float cooldown = 0.0f;
    private int numBullets = 0;
    private Example1InputActions inputActions;
    [SerializeField] private List<string> fireModes = new List<string>();
    private int currFireMode = 0;

    private void Awake() {
        inputActions = new Example1InputActions();
        Reload();
    }

    private void OnEnable() { inputActions.Enable(); }

    private void OnDisable() { inputActions.Disable(); }

    private void Update() { cooldown -= Time.deltaTime; }

    public Example1InputActions GetInputActions() { return inputActions; }

    public int GetMagazineSize() { return magazineSize; }

    public int GetNumBullets() { return numBullets; }

    public void SetFireMode(string fireMode) {
        for (int i = 0; i < fireModes.Count; ++i) {
            if (fireModes[i] == fireMode) {
                currFireMode = i;
                return;
            }
        }
    }

    public string GetFireMode() { return currFireMode < fireModes.Count ? fireModes[currFireMode] : string.Empty; }

    public string GetNextFireMode() { return currFireMode < fireModes.Count ? fireModes[(currFireMode + 1) % fireModes.Count] : string.Empty; }

    public void AddFireMode(string fireMode) { fireModes.Add(fireMode); }

    public bool Shoot() {
        if (cooldown <= 0.0f && numBullets > 0) {
            Instantiate(bulletPrefab).transform.position = bulletSpawnPoint.transform.position;
            cooldown = 1.0f / fireRate;
            --numBullets;
            return true;
        }

        return false;
    }

    public void Reload() { numBullets = magazineSize; }
}