using System.Collections;
using UnityEngine;

public static class Utility 
{
    public static IEnumerator Defer(float delay, System.Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
