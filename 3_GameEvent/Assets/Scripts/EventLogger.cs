using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using GameEvent;

public class EventLogger : MonoBehaviour {
    [Header("References (Do not change!)")]
    [SerializeField] private GameObject scrollView = null;
    [SerializeField] private TextMeshProUGUI eventLogsText = null;
    [SerializeField] private TextMeshProUGUI scoreCounterText = null;

    // Private Variable(s)
    private FoodDeliveryInputActions inputActions;
    private int scoreCounter = 0;

    private void Awake() {
        inputActions = new FoodDeliveryInputActions();
    }

    private void OnEnable() {
        // Input Actions
        inputActions.Enable();
        inputActions.FoodDelivery.ToggleEventLogs.performed += OnToggleEventLogs;

        // Game Events
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.PickUpDelivery), OnPickUpDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void OnDisable() {
        // Input Actions
        inputActions.Disable();
        inputActions.FoodDelivery.ToggleEventLogs.performed -= OnToggleEventLogs;

        // Game Events
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.PickUpDelivery), OnPickUpDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    // Input Actions
    private void OnToggleEventLogs(InputAction.CallbackContext context) {
        scrollView.SetActive(!scrollView.activeSelf);
    }

    // Game Events
    private void OnCreateOrder(Order order) {
        eventLogsText.text += "[" + order.id + ", Create Order] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + "\n";
    }
    private void OnAcceptOrder(Order order) {
        eventLogsText.text += "[" + order.id + ", Accept Order] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + "\n";
    }
    private void OnCreateDelivery(Order order) {
        eventLogsText.text += "[" + order.id + ", Create Delivery] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + "\n";
    }
    
    private void OnAcceptDelivery(Order order) {
        eventLogsText.text += "[" + order.id + ", Accept Delivery] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
    }

    private void OnPickUpDelivery(Order order) {
        eventLogsText.text += "[" + order.id + ", Pick Up Delivery] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
    }

    private void OnCompleteDelivery(Order order) {
        eventLogsText.text += "[" + order.id + ", Complete Delivery] " + "Item: " + Menu.StringOf(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
        scoreCounterText.text = "Completed Deliveries: " + (++scoreCounter).ToString();
    }
}