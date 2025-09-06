using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class GameEventSystem {
    // Singleton Instance
    private static GameEventSystem instance = null;

    // Events
    private Dictionary<string, UnityEventBase> events = new Dictionary<string, UnityEventBase>();

    // Debug Variable(s)
    public bool enableDebug = false;

    // Constructor(s)
    private GameEventSystem() { }

    // Singleton Getter
    public static GameEventSystem GetInstance() {
        if (instance == null) {
            instance = new GameEventSystem();
        }

        return instance;
    }

    private void DebugLog(string str) {
        if (enableDebug) { Debug.Log(str); }
    }

    private UnityEventBase GetEvent(string eventName) {
        UnityEventBase result = null;
        if (events.TryGetValue(eventName, out result)) {
            return result;
        }

        return null;
    }

    private void AddEvent(string eventName) {
        Assert.IsFalse(events.ContainsKey(eventName), MethodBase.GetCurrentMethod().Name + " - An event with this name already exist!");

        UnityEvent unityEvent = new UnityEvent();
        events.Add(eventName, unityEvent);
    }

    private void AddEvent<T>(string eventName) {
        Assert.IsFalse(events.ContainsKey(eventName), MethodBase.GetCurrentMethod().Name + " - An event with this name already exist!");

        UnityEvent<T> unityEvent = new UnityEvent<T>();
        events.Add(eventName, unityEvent);
    }

    private void AddEvent<T1, T2>(string eventName) {
        Assert.IsFalse(events.ContainsKey(eventName), MethodBase.GetCurrentMethod().Name + " - An event with this name already exist!");

        UnityEvent<T1, T2> unityEvent = new UnityEvent<T1, T2>();
        events.Add(eventName, unityEvent);
    }

    private void AddEvent<T1, T2, T3>(string eventName) {
        Assert.IsFalse(events.ContainsKey(eventName), MethodBase.GetCurrentMethod().Name + " - An event with this name already exist!");

        UnityEvent<T1, T2, T3> unityEvent = new UnityEvent<T1, T2, T3>();
        events.Add(eventName, unityEvent);
    }

    private void AddEvent<T1, T2, T3, T4>(string eventName) {
        Assert.IsFalse(events.ContainsKey(eventName), MethodBase.GetCurrentMethod().Name + " - An event with this name already exist!");

        UnityEvent<T1, T2, T3, T4> unityEvent = new UnityEvent<T1, T2, T3, T4>();
        events.Add(eventName, unityEvent);
    }

    // **************** Public Interface ****************
    public bool HasEvent(string eventName) {
        return events.ContainsKey(eventName);
    }

    public void SubscribeToEvent(string eventName, UnityAction unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent unityEvent = (UnityEvent)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.AddListener(unityAction);
    }

    public void SubscribeToEvent<T>(string eventName, UnityAction<T> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T> unityEvent = (UnityEvent<T>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.AddListener(unityAction);
    }

    public void SubscribeToEvent<T1, T2>(string eventName, UnityAction<T1, T2> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2> unityEvent = (UnityEvent<T1, T2>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.AddListener(unityAction);
    }

    public void SubscribeToEvent<T1, T2, T3>(string eventName, UnityAction<T1, T2, T3> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2, T3>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2, T3> unityEvent = (UnityEvent<T1, T2, T3>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.AddListener(unityAction);
    }

    public void SubscribeToEvent<T1, T2, T3, T4>(string eventName, UnityAction<T1, T2, T3, T4> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2, T3, T4>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2, T3, T4> unityEvent = (UnityEvent<T1, T2, T3, T4>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.AddListener(unityAction);
    }

    public void UnsubscribeFromEvent(string eventName, UnityAction unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        Assert.IsFalse(unityEventBase == null, MethodBase.GetCurrentMethod().Name + " - Event(" + eventName + ") not found!");

        UnityEvent unityEvent = (UnityEvent)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.RemoveListener(unityAction);
    }

    public void UnsubscribeFromEvent<T>(string eventName, UnityAction<T> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        Assert.IsFalse(unityEventBase == null, MethodBase.GetCurrentMethod().Name + " - Event(" + eventName + ") not found!");

        UnityEvent<T> unityEvent = (UnityEvent<T>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.RemoveListener(unityAction);
    }

    public void UnsubscribeFromEvent<T1, T2>(string eventName, UnityAction<T1, T2> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        Assert.IsFalse(unityEventBase == null, MethodBase.GetCurrentMethod().Name + " - Event(" + eventName + ") not found!");

        UnityEvent<T1, T2> unityEvent = (UnityEvent<T1, T2>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.RemoveListener(unityAction);
    }

    public void UnsubscribeFromEvent<T1, T2, T3>(string eventName, UnityAction<T1, T2, T3> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        Assert.IsFalse(unityEventBase == null, MethodBase.GetCurrentMethod().Name + " - Event(" + eventName + ") not found!");

        UnityEvent<T1, T2, T3> unityEvent = (UnityEvent<T1, T2, T3>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.RemoveListener(unityAction);
    }

    public void UnsubscribeFromEvent<T1, T2, T3, T4>(string eventName, UnityAction<T1, T2, T3, T4> unityAction) {
        UnityEventBase unityEventBase = GetEvent(eventName);
        Assert.IsFalse(unityEventBase == null, MethodBase.GetCurrentMethod().Name + " - Event(" + eventName + ") not found!");

        UnityEvent<T1, T2, T3, T4> unityEvent = (UnityEvent<T1, T2, T3, T4>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.RemoveListener(unityAction);
    }

    public void TriggerEvent(string eventName) {
        DebugLog("Triggered Event: " + eventName);

        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent unityEvent = (UnityEvent)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.Invoke();
    }

    public void TriggerEvent<T>(string eventName, T parameter) {
        DebugLog("Triggered Event: " + eventName);

        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T> unityEvent = (UnityEvent<T>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.Invoke(parameter);
    }

    public void TriggerEvent<T1, T2>(string eventName, T1 parameter1, T2 parameter2) {
        DebugLog("Triggered Event: " + eventName);

        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2> unityEvent = (UnityEvent<T1, T2>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.Invoke(parameter1, parameter2);
    }

    public void TriggerEvent<T1, T2, T3>(string eventName, T1 parameter1, T2 parameter2, T3 parameter3) {
        DebugLog("Triggered Event: " + eventName);

        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2, T3>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2, T3> unityEvent = (UnityEvent<T1, T2, T3>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.Invoke(parameter1, parameter2, parameter3);
    }

    public void TriggerEvent<T1, T2, T3, T4>(string eventName, T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4) {
        DebugLog("Triggered Event: " + eventName);

        UnityEventBase unityEventBase = GetEvent(eventName);
        if (unityEventBase == null) {
            AddEvent<T1, T2, T3, T4>(eventName);
            unityEventBase = GetEvent(eventName);
        }

        UnityEvent<T1, T2, T3, T4> unityEvent = (UnityEvent<T1, T2, T3, T4>)unityEventBase;
        Assert.IsFalse(unityEvent == null, MethodBase.GetCurrentMethod().Name + " - The event " + eventName + " already exists with different arguments defined!");

        unityEvent.Invoke(parameter1, parameter2, parameter3, parameter4);
    }
}