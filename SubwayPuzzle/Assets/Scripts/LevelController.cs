using FSharp;
using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    private void OnEnable()
    {
        trackControllers = new HashSet<ITrackController>(
            FindObjectsOfType<MonoBehaviour>()
                .OfType<ITrackController>());

        foreach (var c in trackControllers)
        {
            var controller = c;
            void clickHandler() => controller.OrientTo(
                controller.TrackPiece.RotatedBy(ClockDirection.Cw));

            controller.OnClick += clickHandler;
            DisableActions.Add(() => controller.OnClick -= clickHandler);
        }
    }

    private void OnDisable()
    {
        foreach (var action in DisableActions)
            action();

        DisableActions.Clear();
    }


    private HashSet<ITrackController> trackControllers;
    private readonly List<Action> DisableActions = new List<Action>();
}
