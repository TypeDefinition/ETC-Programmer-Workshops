using TMPro;
using UnityEngine;

public class Nameplate : MonoBehaviour {
    [SerializeField] private GameObject target = null;

    private void Start() {
        TextMeshPro text = GetComponent<TextMeshPro>();
        if (text != null && target != null) {
            text.text = target.name;
        }
    }
}