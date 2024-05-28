#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    public class CopyPasteGameObjectTool : EditorWindow
    {
        private static Vector3 _copiedPosition;
        private static Quaternion _copiedRotation;
        private static Vector3 _copiedScale;

        [MenuItem("Tools/Copy Transform &C")]
        private static void CopyTransform()
        {
            if (Selection.activeGameObject != null)
            {
                Transform selected = Selection.activeGameObject.transform;
                _copiedPosition = selected.position;
                _copiedRotation = selected.rotation;
                _copiedScale = selected.localScale;
                Debug.Log("Transform copied from " + selected.name + ".");
            }
            else
            {
                Debug.LogWarning("Please select a game object to copy position from.");
            }
        }

        [MenuItem("Tools/Paste Transform &V")]
        private static void PasteTransform()
        {
            if (Selection.activeGameObject != null)
            {
                Transform selected = Selection.activeGameObject.transform;
                Undo.RegisterFullObjectHierarchyUndo(selected, "Paste new Transform");
                selected.position = _copiedPosition;
                selected.rotation = _copiedRotation;
                selected.localScale = _copiedScale;
                Debug.Log("Transform pasted to " + selected.name + ".");
            }
            else
            {
                Debug.LogWarning("Please select a game object to paste position to.");
            }
        }
    }
}
#endif