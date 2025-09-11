using GameEvent;
using UnityEngine;
using System.Collections.Generic;
using FSM;

public class Deliveryman : MonoBehaviour {
    private enum State {
        Meander,
        PickUp,
        Deliver,

        Num,
    }

    [Header("References")]
    [SerializeField] private GameObject package = null;

    [Header("Deliveryman Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float interactionDistance = 2.0f;

    // Private Variables
    private FiniteStateMachine fsm = new FiniteStateMachine((int)State.Num);
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

    private void Awake() {
        // Initialise FSM.
        fsm.SetStateUpdate((int)State.Meander, OnUpdateMeander);
        
        fsm.SetStateUpdate((int)State.PickUp, OnUpdatePickUp);
        
        fsm.SetStateUpdate((int)State.Deliver, OnUpdateDeliver);

        fsm.ChangeState((int)State.Meander);
    }

    private void Update() { fsm.Update(); }

    private void LateUpdate() { fsm.LateUpdate(); }

    // Finite State Machine
    private void OnUpdateMeander() {
        // Choose a random direction and move towards it every 2 seconds.
        meanderTimer -= Time.deltaTime;
        if (meanderTimer <= 0.0f) {
            meanderTimer = 2.0f;
            meanderDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
        }
        transform.Translate(meanderDirection * Time.deltaTime, Space.World);

        // If there is no current delivery, see if we can accept one from the pending deliveries.
        if (0 < pendingDeliveries.Count) {
            // Accept a pending delivery.
            currentDelivery = pendingDeliveries[0].AcceptDelivery(gameObject);
            // Let every deliveryman know that we've already accepted this delivery so they can't accept it too.
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.AcceptDelivery), currentDelivery);
            
            // Let's pick up the delivery.
            fsm.ChangeState((int)State.PickUp);
        }
    }

    private void OnUpdatePickUp() {
        // Move towards the shop to pick up our order.
        Vector3 direction = (currentDelivery.GetShop().transform.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        // Once we are close enough to the shop, pick up the order and inform the shop.
        if (Vector3.Distance(currentDelivery.GetShop().transform.position, transform.position) < interactionDistance) {
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.PickUpDelivery), currentDelivery.PickUp());

            // Just UI stuff.
            package.SetActive(true); 
            package.GetComponent<MeshRenderer>().material.color = Menu.ColorOf(currentDelivery.menuItem);

            // Let's bring it to the customer.
            fsm.ChangeState((int)State.Deliver);
        }
    }

    private void OnUpdateDeliver() {
        // Move towards the customer.
        Vector3 direction = (currentDelivery.GetCustomer().transform.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        // Once we are close enough to the customer, complete the order and inform the customer.
        if (Vector3.Distance(currentDelivery.GetCustomer().transform.position, transform.position) < interactionDistance) {
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CompleteDelivery), currentDelivery.CompleteDelivery());

            // Just UI stuff.
            currentDelivery = null;
            package.SetActive(false);
            
            // Alright, job done, let's go back to meandering.
            fsm.ChangeState((int)State.Meander);
        }
    }

    // Game Event Callbacks
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