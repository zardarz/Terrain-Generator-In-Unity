using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new();

    public static void Enqueue(Action action) {
        lock (_executionQueue) {
            _executionQueue.Enqueue(action);
        }
    }

    void Update() {
        while (_executionQueue.Count > 0) {
            Action action;
            lock (_executionQueue) {
                action = _executionQueue.Dequeue();
            }
            action?.Invoke();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init() {
        if (FindObjectOfType<MainThreadDispatcher>() == null) {
            GameObject go = new("MainThreadDispatcher");
            go.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
    }
}
