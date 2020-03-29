using System;
using UnityEngine;

/// <summary>
/// Displays a multi-line explanation string before a field.
/// </summary>
public class ExplanationAttribute : PropertyAttribute
{
    public readonly string text;

    public ExplanationAttribute(string text, int order = 0) {
        this.text = text;
        this.order = order;
    }
}
