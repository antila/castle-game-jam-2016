using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnapToGrid : ScriptableObject
{

    [MenuItem("Tools/Snap to Grid %g")]
    static void MenuSnapToGrid()
    {
        foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable))
        {
            Debug.Log(t.position);
            Debug.Log(t.localPosition);
            Debug.Log(EditorPrefs.GetFloat("MoveSnapX"));

            float snapX = EditorPrefs.GetFloat("MoveSnapX");
            float snapY = EditorPrefs.GetFloat("MoveSnapY");
            float snapZ = EditorPrefs.GetFloat("MoveSnapZ");

            if (snapX == 0)
            {
                snapX = 1;
            }
            if (snapY == 0)
            {
                snapY = 1;
            }
            if (snapZ == 0)
            {
                snapZ = 1;
            }

            Debug.Log(snapX);

            t.localPosition = new Vector3(
                Mathf.Round(t.localPosition.x / snapX) * snapX,
                Mathf.Round(t.localPosition.y / snapY) * snapY,
                Mathf.Round(t.localPosition.z / snapZ) * snapZ
            );
            Debug.Log(t.localPosition);
        }
    }

}