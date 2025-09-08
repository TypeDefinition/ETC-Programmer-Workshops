using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject orderIndicator = null;
    [SerializeField] private GameObject completionIndicator = null;

    // Private Indicator
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
            currentOrder = new Order((Menu.Item)Random.Range(0, (int)Menu.Item.Num), gameObject);
            GameEventSystem.GetInstance().TriggerEvent<Order>(nameof(GameEventName.CreateOrder), currentOrder);

            // Hacky GUI for demo purposes.
            orderIndicator.SetActive(true);
            orderIndicator.GetComponent<Image>().color = Menu.ColorOf(currentOrder.menuItem);
        }
    }

    private void OnCompleteDelivery(Order order) {
        if (currentOrder != null && currentOrder.id == order.id) {
            currentOrder = null;
            cooldown = Random.Range(3.0f, 7.0f);

            // Hacky GUI for demo purposes.
            orderIndicator.SetActive(false);
            completionIndicator.SetActive(true);
        }
    }
}