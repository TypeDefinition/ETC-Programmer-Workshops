using UnityEngine;
using System.Collections.Generic;

namespace FSM {
    // We want to make State a ScriptableObject so that we can drag an drop them in the editor.
    // New states should inherit from this class.
    public abstract class State : ScriptableObject {
        [SerializeField] private string stateName = "Unnamed State";

        public string GetStateName() { return stateName; }

        // Virtual functions that child states may choose to override.
        public virtual void OnAwake(FiniteStateMachine fsm, GameObject target) { }
        public virtual void OnEnter(FiniteStateMachine fsm, GameObject target) { }
        public virtual void OnUpdate(FiniteStateMachine fsm, GameObject target) { }
        public virtual void OnLateUpdate(FiniteStateMachine fsm, GameObject target) { }
        public virtual void OnExit(FiniteStateMachine fsm, GameObject target) { }
    }

    public class FiniteStateMachine : MonoBehaviour {
        // All the possible states in the FSM. It's finite, after all.
        [SerializeField] private State[] states = new State[0];
        // The initial starting state.
        [SerializeField] private string initialState = "";

        // The current state that is active.
        private string currentState = "";
        // The next state we will transit into.
        private string nextState = "";
        // Since State is a ScriptableObject, we need to make a copy during runtime so that we do not affect every other FSM using the same state.
        private Dictionary<string, State> runtimeStates = new Dictionary<string, State>();

        public string GetInitialState() { return initialState; }
        public string GetCurrentState() { return currentState; }
        public bool HasState(string stateName) { return runtimeStates.ContainsKey(stateName); }
        public void ChangeState(string nextState) { this.nextState = nextState; }

        public void Awake() {
            // Make a copy of each state so that each instance of FiniteStateMachine's data is independent.
            nextState = initialState;
            for (int i = 0; i < states.Length; ++i) {
                State state = Instantiate(states[i]);
                runtimeStates.Add(state.GetStateName(), Instantiate(state));
            }

            // Invoke the OnAwake function of every state.
            foreach (var state in runtimeStates.Values) {
                state.OnAwake(this, gameObject);
            }
        }

        private void Update() {
            // Transit state.
            if (nextState != currentState) {
                // Exit current state.
                if (runtimeStates.ContainsKey(currentState)) {
                    runtimeStates[currentState].OnExit(this, gameObject);
                }

                // Enter next state.
                currentState = nextState;
                if (runtimeStates.ContainsKey(currentState)) {
                    runtimeStates[currentState].OnEnter(this, gameObject);
                } else {
                    Debug.LogWarning("FiniteStateMachine::Update - State " + currentState + " not found! Ensure that the state is correctly named in its ScriptableObject, and that it has been added to the FiniteStateMachine in the Unity Inspector.");
                }
            }

            // Update current state.
            if (runtimeStates.ContainsKey(currentState)) {
                runtimeStates[currentState].OnUpdate(this, gameObject);
            }
        }

        private void LateUpdate() {
            // LateUpdate current state.
            if (runtimeStates.ContainsKey(currentState)) {
                runtimeStates[currentState].OnLateUpdate(this, gameObject);
            }
        }
    }
}