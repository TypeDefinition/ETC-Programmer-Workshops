namespace FSM {
    public class FiniteStateMachine {
        public const int INVALID_STATE = -1; // Use negative value to denote an invalid state.

        public readonly int NumStates = 0;
        private int currentState = INVALID_STATE;
        private int nextState = INVALID_STATE;

        public delegate void FuncPtr();
        private FuncPtr[] stateEntries = null;
        private FuncPtr[] stateUpdates = null;
        private FuncPtr[] stateLateUpdates = null;
        private FuncPtr[] stateExits = null;

        public FiniteStateMachine(int numStates) {
            NumStates = numStates;
            stateEntries = new FuncPtr[numStates];
            stateUpdates = new FuncPtr[numStates];
            stateLateUpdates = new FuncPtr[numStates];
            stateExits = new FuncPtr[numStates];
        }

        public int GetCurrentState() { return currentState; }

        public void SetStateEntry(int state, FuncPtr funcPtr = null) { stateEntries[state] = funcPtr; }
        public void SetStateUpdate(int state, FuncPtr funcPtr = null) { stateUpdates[state] = funcPtr; }
        public void SetStateLateUpdate(int state, FuncPtr funcPtr = null) { stateLateUpdates[state] = funcPtr; }
        public void SetStateExit(int state, FuncPtr funcPtr = null) { stateExits[state] = funcPtr; }

        public void ChangeState(int nextState) { this.nextState = nextState; }

        public void Update() {
            // Transit state.
            if (nextState != currentState) {
                // Exit current state.
                if (0 <= currentState) { stateExits[currentState]?.Invoke(); }

                // Enter next state.
                currentState = nextState;
                if (0 <= currentState) { stateEntries[currentState]?.Invoke(); }
            }

            // Update current state.
            if (0 <= currentState) { stateUpdates[currentState]?.Invoke(); }
        }

        public void LateUpdate() {
            if (0 <= currentState) { stateLateUpdates[currentState]?.Invoke(); }
        }
    }
}