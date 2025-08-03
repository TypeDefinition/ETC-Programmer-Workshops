using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example1Player : MonoBehaviour {
    [SerializeField] private float angularVelocity = 60.0f;
    
    private void Update() {
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime);
    }
}