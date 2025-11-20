using UnityEngine;

public static class GridUtils
{
    public static Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(Mathf.FloorToInt(worldPos.x), 0, Mathf.FloorToInt(worldPos.z));
    }

    public static Vector3 GridToWorld(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.z);
    }
}
