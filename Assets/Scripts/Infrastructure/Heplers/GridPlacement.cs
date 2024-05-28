
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridPlacement : MonoBehaviour
{
    public Vector3 gridSize = new Vector3(1f, 1f, 1f); // Size of the grid
    public Color debugLineColor = Color.green; // Color of the grid debug lines
    public Vector3 setupGridSizeCount = new Vector3(1f, 1f, 1f); // Size of the grid

    public bool isDrawGrid;
    public bool isDrawCoords;
    private static bool isWorking = false;

    private void OnDrawGizmos()
    {
        if (isWorking && isDrawGrid)
        {
            Gizmos.color = debugLineColor;

            Vector3 startPosition;
            if (setupGridSizeCount.x % 2 != 0)
                startPosition = new Vector3((-setupGridSizeCount.x / 2.0f) + gridSize.x / 2.0f, 0.0f,
                    (-setupGridSizeCount.z / 2.0f) + gridSize.z / 2.0f);
            else
                startPosition = new Vector3((-setupGridSizeCount.x / 2.0f), 0.0f, (-setupGridSizeCount.z / 2.0f));

            Vector3 gridSizeCount = setupGridSizeCount;

            var editorStyle = EditorStyles.whiteBoldLabel;
            editorStyle.fontSize = 1;

            for (float x = 0; x <= gridSizeCount.x; x += gridSize.x)
            {
                for (float z = 0; z <= gridSizeCount.z; z += gridSize.z)
                {
                    for (float y = 0; y <= gridSizeCount.y; y += gridSize.y)
                    {
                        Vector3 pos = startPosition + new Vector3(x, y, z);
                        if (isDrawCoords)
                            Handles.Label(pos, $"({x}, {y}, {z})", EditorStyles.whiteLabel);
                        Gizmos.DrawWireCube(pos, gridSize);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (isWorking && !Application.isPlaying && Selection.transforms.Length > 0)
            foreach (Transform selectedTransform in Selection.transforms)
                if (selectedTransform.IsChildOf(transform))
                    SnapToGrid(selectedTransform);
    }

    [MenuItem("Tools/GridPlacement/Toggle %g")]
    private static void ToggleComponent()
    {
        isWorking = !isWorking;
    }

    private void SnapToGrid(Transform selectedTransform)
    {
        Vector3 currentPosition = selectedTransform.position;

        Vector3 snappedPosition = new Vector3(
            Mathf.Round(currentPosition.x / gridSize.x) * gridSize.x,
            Mathf.Round(currentPosition.y / gridSize.y) * gridSize.y,
            Mathf.Round(currentPosition.z / gridSize.z) * gridSize.z
        );

        selectedTransform.position = snappedPosition;
    }
}
#endif