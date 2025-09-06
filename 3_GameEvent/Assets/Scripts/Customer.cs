using UnityEngine;

public class Customer : MonoBehaviour {
    private Order currentOrder = null;
    private float cooldown = 0.0f;

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void Start() {}

    private void Update() {
        // If there is no current order, create an order with a random menu item.
        cooldown -= Time.deltaTime;
        if (currentOrder == null && cooldown < 0.0f) {
            currentOrder = new Order((MenuItem)Random.Range(0, (int)MenuItem.Num), gameObject);
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CreateOrder), currentOrder);
        }
    }

    private void OnCompleteDelivery(Order order) {
        if (currentOrder != null && currentOrder.id == order.id) {
            currentOrder = null;
            cooldown = Random.Range(3.0f, 7.0f);
        }
    }
}