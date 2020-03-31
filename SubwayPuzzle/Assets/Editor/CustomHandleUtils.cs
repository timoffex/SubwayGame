using System;
using UnityEditor;
using UnityEngine;

public static class CustomHandleUtils
{
    /// <summary>
    /// Sets <see cref="Handles.color"/> while running the action.
    /// </summary>
    public static void WithHandleColor(Color color, Action action)
    {
        var oldColor = Handles.color;
        Handles.color = color;
        action();
        Handles.color = oldColor;
    }
}
