using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom editor for a <see cref="TrackController"/>.
/// </summary>
[CustomEditor(typeof(TrackController))]
public class TrackControllerEditor : Editor
{
    private const float HandleSize = 0.05f;

    private void OnSceneGUI()
    {
        foreach (var track in TrackControllers)
        {
            track.configuration.serializedTrainPath =
                SerializedPointPath.From(DrawPath(
                    track.transform.position,
                    track.configuration.serializedTrainPath.AsPointPath));
        }
    }

    /// <summary>
    /// Draws the given path and returns the updated version.
    ///
    /// In the future, this may allow editing.
    /// </summary>
    private PointPath DrawPath(Vector3 offset, PointPath path)
    {
        if (path.Length == 0)
            return path;

        var translatedPath = path.Map((p) => p + offset);

        CustomHandleUtils.WithHandleColor(Color.green, delegate
        {
            Handles.DrawAAPolyLine(translatedPath.ToArray());

            foreach (var (point, idx) in
                translatedPath.Select((p, idx) => (p, idx)))
            {
                Handles.CubeHandleCap(
                    GUIUtility.GetControlID(idx, FocusType.Passive),
                    point,
                    Quaternion.identity,
                    HandleUtility.GetHandleSize(point) * HandleSize,
                    Event.current.type);
            }
        });

        return path;
    }

    /// <summary>
    /// The selected <see cref="TrackController"/>s.
    /// </summary>
    private TrackController[] TrackControllers =>
        targets.OfType<TrackController>().ToArray();
}
