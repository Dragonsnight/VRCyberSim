using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public static class Pack 
{

    [MenuItem("Utility/Pack Selected", false)]
    public static void PackSelected()
    {
        if (Selection.count == 0)
        {
            Debug.LogError("No objects selected");
            return;
        }

        Transform newParent = new GameObject("Parent").transform;

        foreach (Transform item in Selection.transforms)
        {
            item.SetParent(newParent);
        }

        CenterOnChildren(newParent);

        Selection.objects = new Object[1] { newParent.gameObject };




    }
    private static void CenterOnChildren(Transform parent)
    {
        Vector3 position = Vector3.zero;

        Transform[] children = parent.Cast<Transform>().ToArray();

        foreach (Transform child in children)
        {
            position += child.position;
            child.parent = null;
        }

        position /= children.Length;
        parent.position = position;

        foreach (Transform child in children)
            child.parent = parent;
    }
}
