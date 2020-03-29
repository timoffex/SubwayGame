using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Updates the generated .sln file to include FSharp and FSharp.Tests.
/// </summary>
public class AddProjectsToSln : AssetPostprocessor
{
    /// <summary>
    /// This is an undocumented method called by Unity.
    /// 
    /// See https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/AssetPostprocessor.cs
    ///
    /// It must be public and inside an AssetPostprocessor.
    /// Its return value is the new content of the .sln file.
    /// </summary>
    public static string OnGeneratedSlnSolution(string path, string content)
    {
        // path is the path to the .sln file
        // content is the content of the .sln file

        var projectDirectory = Directory.GetParent(path).FullName;
        var scriptPath = Path.Combine(projectDirectory, "add_fsharp_to_sln.sh");

        // Use the add_fsharp_to_sln.sh script.
        var process =
            Process.Start(new ProcessStartInfo(scriptPath, projectDirectory));

        // Wait up to 2 seconds for the process to complete.
        process.WaitForExit(2000);

        if (!process.HasExited)
        {
            process.Kill();
            UnityEngine.Debug.LogError(
                "Failed to add FSharp and FSharp.Tests to .sln file." +
                $" Command: {scriptPath} '{path}' failed to complete within" +
                " 2 seconds.");
            return content;
        }
        else
        {
            UnityEngine.Debug.Log(
                "Ran command to add FSharp and FSharp.Tests to .sln file." +
                " See add_fsharp_to_sln.sh.last_run for the result.");

            // Return the modified content.
            return File.ReadAllText(path);
        }
    }
}
