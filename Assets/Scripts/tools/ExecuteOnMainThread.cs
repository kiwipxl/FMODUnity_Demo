using System;
using System.Collections.Generic;
using UnityEngine;

class ExecuteOnMainThread : MonoBehaviour
{
    public static Queue<Action> queue = new Queue<Action>();

    public void Update()
    {
        try {
            lock (queue)
            {
                while (queue.Count > 0)
                {
                    queue.Dequeue().Invoke();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occurred during main thread invokes: " + ex.Message);
        }
    }
}
