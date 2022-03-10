using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackCreator), true), CanEditMultipleObjects]
public class TrackCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TrackCreator trackCreator = (TrackCreator)target;
        if(GUILayout.Button("Generate Track"))
        {
            trackCreator.GenerateTrack();
        }

    }
}
