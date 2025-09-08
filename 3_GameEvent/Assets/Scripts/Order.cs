using UnityEngine;

public class Order {
    public enum Status {
        Created,
        Accepted,
        AwaitingPickUp,
        InDelivery,
        Completed,

        Num,
    }

    private static int orderCounter = 0;

    public readonly int id;
    private Status status;
    public readonly Menu.Item menuItem;
    
    private GameObject customer;
    private GameObject shop;
    private GameObject deliveryman;

    public Order(Menu.Item menuItem, GameObject customer) {
        id = ++orderCounter;
        status = Status.Created;
        this.menuItem = menuItem;
        this.customer = customer;
    }

    public Status GetStatus() { return status; }

    public GameObject GetCustomer() { return customer; }
    public GameObject GetShop() { return shop; }
    public GameObject GetDeliveryman() { return deliveryman; }

    public Order AcceptOrder(GameObject shop) {
        if (status == Status.Created) {
            this.shop = shop;
            status = Status.Accepted;
            return this;
        }
        throw new System.Exception("An order can only be accepted once!");
    }

    public Order AwaitPickUp() {
        if (status == Status.Accepted) {
            status = Status.AwaitingPickUp;
            return this;
        }
        throw new System.Exception("An order can only await pick up once!");
    }

    public Order AcceptDelivery(GameObject deliveryman) {
        if (status == Status.AwaitingPickUp) {
            this.deliveryman = deliveryman;
            return this;
        }
        throw new System.Exception("A delivery can only be accepted once!");
    }

    public Order PickUp() {
        if (status == Status.AwaitingPickUp) {
            status = Status.InDelivery;
            return this;
        }
        throw new System.Exception("A delivery can only be picked up once!");
    }

    public Order CompleteDelivery() {
        if (status == Status.InDelivery) {
            status = Status.Completed;
            return this;
        }
        throw new System.Exception("A delivery can only be completed once!");
    }
}