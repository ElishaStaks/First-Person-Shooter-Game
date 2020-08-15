using System;
using System.Collections;
using UnityEngine;

public static class MonobehaviourExtensions
{
    public static void Invoke(this MonoBehaviour mono, Action theDelegate, float time)
    {
        mono.StartCoroutine(ExecuteAfterTime(theDelegate, time));
    }

    // Simple coroutine that waits a certain amount of secs before executing the delegate function
    private static IEnumerator ExecuteAfterTime(Action theDelegate, float delay)
    {
        yield return new WaitForSeconds(delay);
        theDelegate();
    }
}
