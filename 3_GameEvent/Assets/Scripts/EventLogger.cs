using TMPro;
using UnityEngine;

public class EventLogger : MonoBehaviour {
    private TextMeshProUGUI textUI;

    private string ToString(MenuItem menuItem) {
        switch (menuItem) {
            case MenuItem.Invalid: return "Invalid";
            case MenuItem.Pizza: return "Pizza";
            case MenuItem.Taco: return "Taco";
            case MenuItem.Laksa: return "Laksa";
            case MenuItem.Num: return "Num";
            default: throw new System.Exception("Unhandled MenuItem");
        }
    }

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.PickUpDelivery), OnPickUpDelivery);
        GameEventSystem.GetInstance().SubscribeToEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateOrder), OnCreateOrder);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptOrder), OnAcceptOrder);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CreateDelivery), OnCreateDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.AcceptDelivery), OnAcceptDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.PickUpDelivery), OnPickUpDelivery);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Order>(nameof(GameEventName.CompleteDelivery), OnCompleteDelivery);
    }

    private void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {}

    private void OnCreateOrder(Order order) {
        textUI.text += "[" + order.id + ", Create Order] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + "\n";
    }
    private void OnAcceptOrder(Order order) {
        textUI.text += "[" + order.id + ", Accept Order] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + "\n";
    }
    private void OnCreateDelivery(Order order) {
        textUI.text += "[" + order.id + ", Create Delivery] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + "\n";
    }
    
    private void OnAcceptDelivery(Order order) {
        textUI.text += "[" + order.id + ", Accept Delivery] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
    }

    private void OnPickUpDelivery(Order order) {
        textUI.text += "[" + order.id + ", Pick Up Delivery] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
    }

    private void OnCompleteDelivery(Order order) {
        textUI.text += "[" + order.id + ", Complete Delivery] " + "Item: " + ToString(order.menuItem) + ", Customer: " + order.GetCustomer().name + ", Shop: " + order.GetShop().name + ", Deliveryman: " + order.GetDeliveryman().name + "\n";
    }
}