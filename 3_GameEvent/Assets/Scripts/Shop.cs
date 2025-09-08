using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Slider timerSlider = null;
    [SerializeField] private GameObject readyIndicator = null;
    
    [Header("Shop Settings")]
    [SerializeField] private Menu.Item menuItem = Menu.Item.Invalid;

    private List<Order> pendingOrders = new List<Order>();
    private Order currentOrder = null;
    private float orderDuration = 0.0f;
    private float orderTimer = 0.0f;

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
    }

    private void Start() {
        timerSlider.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().material.color = Menu.ColorOf(menuItem);
    }

    private void Update() {
        // If there is no current order, see if we can accept one from the pending orders.
        if (currentOrder == null && 0 < pendingOrders.Count) {
            // Accept a pending order.
            currentOrder = pendingOrders[0].AcceptOrder(gameObject);
            // Let every shops know that we've already accepted this order so they can't accept it too.
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.AcceptOrder), currentOrder);
            // Generate a random duration to work on the order.
            orderTimer = orderDuration = Random.Range(3.0f, 7.0f);

            /*** Honestly the UI stuff is super jank and just quickly thrown together for this demo. ***/
            // Enable the timer to show how much time is left.
            timerSlider.gameObject.SetActive(true);
        }

        // If we have a current order, work on it.
        if (currentOrder != null) {
            // Work on the order.
            orderTimer -= Time.deltaTime;
            timerSlider.value = orderTimer / orderDuration;

            // If we finished working on the current order, create a delivery.
            if (orderTimer < 0.0f) {
                GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CreateDelivery), currentOrder.AwaitPickUp());
                currentOrder = null;

                /*** Honestly the UI stuff is super jank and just quickly thrown together for this demo. ***/
                // Disable the timer.
                timerSlider.gameObject.SetActive(false);
                // Enable the ready indicator.
                readyIndicator.SetActive(true);
            }
        }
    }

    private void OnCreateOrder(Order order) {
        // If this shops sells what the order requests, add the order to our pending orders list.
        if (order.menuItem == menuItem) {
            pendingOrders.Add(order);
        }
    }

    private void OnAcceptOrder(Order order) {
        // Once an order has been accepted by someone, remove it from the pending orders.
        for (int i = 0; i < pendingOrders.Count; ++i) {
            if (pendingOrders[i].id == order.id) {
                pendingOrders.RemoveAt(i);
                break;
            }
        }
    }
}
