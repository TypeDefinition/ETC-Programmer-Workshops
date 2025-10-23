using UnityEngine;
using UnityEngine.UI;
using GameEvent;
using FSM;

public class Customer : MonoBehaviour {
    private enum State {
        Order,
        Cooldown,

        Num,
    }

    [Header("References")]
    [SerializeField] private GameObject orderIndicator = null;
    [SerializeField] private GameObject completionIndicator = null;

    // Private Variable(s)
    private FiniteStateMachine fsm = new FiniteStateMachine((int)State.Num);
    private Order currentOrder = null;
    private float cooldown = 0.0f;

    private void OnEnable() {
        // Subscribe to events that we care about.
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void OnDisable() {
        // Unsubscribe from events.
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void Awake() {
        // Initialise FSM.
        fsm.SetStateEntry((int)State.Order, OnEnterOrder);

        fsm.SetStateEntry((int)State.Cooldown, OnEnterCooldown);
        fsm.SetStateUpdate((int)State.Cooldown, OnUpdateCooldown);

        fsm.ChangeState((int)State.Order);
    }

    private void Update() { fsm.Update(); }

    private void LateUpdate() { fsm.LateUpdate(); }

    // Finite State Machine
    private void OnEnterOrder() {
        // Create an order with a random menu item.
        currentOrder = new Order((Menu.Item)Random.Range(0, (int)Menu.Item.Num), gameObject);
        GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CreateOrder), currentOrder);

        // Hacky GUI for demo purposes.
        orderIndicator.SetActive(true);
        orderIndicator.GetComponent<Image>().color = Menu.ColorOf(currentOrder.menuItem);
    }

    private void OnEnterCooldown() {
        // Hacky GUI for demo purposes.
        orderIndicator.SetActive(false);
        completionIndicator.SetActive(true);

        // Random cooldown duration.
        cooldown = Random.Range(3.0f, 7.0f);
    }

    private void OnUpdateCooldown() {
        // After some time, we get hungry again.
        cooldown = Mathf.Max(0.0f, cooldown - Time.deltaTime);
        if (cooldown == 0.0f) {
            fsm.ChangeState((int)State.Order);
        }
    }

    // Event Callbacks
    private void OnCompleteDelivery(Order order) {
        // Once we receive our food, it'll be a while before we're hungry again.
        if (currentOrder != null && currentOrder.id == order.id) {
            currentOrder = null;
            fsm.ChangeState((int)State.Cooldown);
        }
    }
}