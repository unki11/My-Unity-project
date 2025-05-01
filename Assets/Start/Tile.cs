using UnityEngine;

public class Tile : MonoBehaviour
{
    public int uniqueID;
    public int playerNum;
    public int Number;
    public Vector2Int gridPos;     // 체스판 좌표 (x, z)
    public GameObject currentUnit; // 해당 타일 위 유닛 (없으면 null)\

    void Start()
    {
        uniqueID = gameObject.GetInstanceID();  // ID 생성 함수 만들어서 부여
    }

    public void SetUnit(GameObject unit)
    {
        currentUnit = unit;
    }

    public bool IsOccupied()
    {
        return currentUnit != null;
    }

    public Vector3 GetTileWorldPosition()
    {
        Vector3 pos = transform.position;
        pos.y += 1f;
        return pos;
    }
}
