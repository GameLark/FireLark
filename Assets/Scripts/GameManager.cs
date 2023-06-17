using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    
    public static bool quickStart = false;

    void Awake() {
        #if UNITY_EDITOR
            if (!quickStart) {
                quickStart = EditorUtility.DisplayDialog("Quickstart", "Skip intro?", "Yes", "No");
            }
        #endif
    }
}
