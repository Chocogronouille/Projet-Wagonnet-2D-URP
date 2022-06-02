using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools : EditorWindow
{
    [MenuItem("Tools / Reset Player Prefs")]

    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
