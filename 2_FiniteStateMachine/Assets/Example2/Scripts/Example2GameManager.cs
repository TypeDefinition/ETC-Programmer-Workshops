using TMPro;
using UnityEngine;

public class Example2GameManager : MonoBehaviour {
    public TextMeshProUGUI readyText = null;
    public TextMeshProUGUI goText = null;
    public TextMeshProUGUI finishText = null;
    [SerializeField] private Example2Racer[] racers = new Example2Racer[0];

    private void Start() {

    }

    private void Update() {

    }

    public Example2Racer[] GetRacers() { return racers; }
}