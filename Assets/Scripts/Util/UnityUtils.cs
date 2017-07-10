using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtils
{
    /// <summary>
    /// Recursively finds a child of the current transform with the specified name.
    /// </summary>
    /// <param name="current">The transform from where to start</param>
    /// <param name="name">The name of the child to look for</param>
    /// <returns>The child if found, otherwise null.</returns>
    public static Transform RecursiveFind(Transform current, string name)
    {
        if (current.name == name)
            return current;
        for (int i = 0; i < current.childCount; ++i)
        {
            Transform found = RecursiveFind(current.GetChild(i), name);
            if (found != null)
                return found;
        }
        return null;
    }
}
