using UnityEngine;
using System.Collections.Generic;

public class Deliveryman : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject package = null;

    [Header("Deliveryman Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float interactionDistance = 2.0f;

    // Private Variables
    private List<Order> pendingDeliveries = new List<Order>();
    private Order currentDelivery = null;
    private Vector3 meanderDirection = Vector3.zero;
    private float meanderTimer = 0.0f;

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
    }

    private void Update() {
        // If there is no current delivery, see if we can accept one from the pending deliveries.
        if (currentDelivery == null && 0 < pendingDeliveries.Count) {
            // Accept a pending delivery.
            currentDelivery = pendingDeliveries[0].AcceptDelivery(gameObject);
            // Let every deliveryman know that we've already accepted this delivery so they can't accept it too.
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.AcceptDelivery), currentDelivery);
        }

        // If we have a current delivery, work on it.
        if (currentDelivery != null) {
            switch (currentDelivery.GetStatus()) {
                // Pick up the order from the shop.
                case Order.Status.AwaitingPickUp: {
                        // Move towards the shop.
                        Vector3 direction = (currentDelivery.GetShop().transform.position - transform.position).normalized;
                        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
                        // Once we are close enough to the shop, pick up the order and inform the shop.
                        if (Vector3.Distance(currentDelivery.GetShop().transform.position, transform.position) < interactionDistance) {
                            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.PickUpDelivery), currentDelivery.PickUp());
                            package.SetActive(true); // Just UI stuff.
                            package.GetComponent<MeshRenderer>().material.color = Menu.ColorOf(currentDelivery.menuItem);
                        }
                    }
                    break;
                // Bring the order to the customer.
                case Order.Status.InDelivery: {
                        // Move towards the customer.
                        Vector3 direction = (currentDelivery.GetCustomer().transform.position - transform.position).normalized;
                        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
                        // Once we are close enough to the customer, complete the order and inform the customer.
                        if (Vector3.Distance(currentDelivery.GetCustomer().transform.position, transform.position) < interactionDistance) {
                            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CompleteDelivery), currentDelivery.CompleteDelivery());
                            currentDelivery = null;
                            package.SetActive(false); // Just UI stuff.
                        }
                    }
                    break;
                default:
                    throw new System.Exception(gameObject.name + " Unhandled order status!");
            }
        }
        // Otherwise, just meander in a random direction slowly.
        else {
            meanderTimer -= Time.deltaTime;
            if (meanderTimer <= 0.0f) {
                meanderTimer = 2.0f;
                meanderDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
            }
            transform.Translate(meanderDirection * Time.deltaTime, Space.World);
        }
    }

    private void OnCreateDelivery(Order order) {
        pendingDeliveries.Add(order);
    }

    private void OnAcceptDelivery(Order order) {
        // Once a delivery has been accepted by someone, remove it from the pending orders.
        for (int i = 0; i < pendingDeliveries.Count; ++i) {
            if (pendingDeliveries[i].id == order.id) {
                pendingDeliveries.RemoveAt(i);
                break;
            }
        }
    }
}