using UnityEngine;

public class ChessboardGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // 복제할 타일 프리팹
    public Material tileMaterial;
    public float offset = 2f; // 옆으로 이동할 거리
    public int columnCnt = 8;
    public int rowCnt = 4;
    public bool isBlack = true;
    public Tile[,] tiles = new Tile[4, 8];

    void Start()
    {
        if (tilePrefab != null)
        {

            Vector3 originalPosition = tilePrefab.transform.position;
            Quaternion originalRotation = tilePrefab.transform.rotation;

            Vector3 size = tilePrefab.GetComponent<Renderer>().bounds.size;

            int TileCount = 0;

            for (int z = 0; z < rowCnt; z++)
                {
                    for (int x = 0; x < columnCnt; x++)
                   {
                        Vector3 spawnPosition = originalPosition + tilePrefab.transform.right * size.x * x + tilePrefab.transform.up * size.z * z; 
                        GameObject tileInstance = Instantiate(tilePrefab, spawnPosition, originalRotation);

                        tileInstance.name = "Tile";

                        Tile tile = tileInstance.GetComponent<Tile>();
                        tile.gridPos = new Vector2Int(z,x);
                        tile.Number = ++TileCount;
                        tiles[z, x] = tile;

                        Renderer renderer = tileInstance.GetComponent<Renderer>();
                        if (renderer != null && renderer.material != null)
                        {
                            // 복제된 인스턴스의 Material 가져오기
                            Material instanceMaterial = renderer.material; // 인스턴스화된 Material
                            ChangeColor(instanceMaterial);
                        }

                        Debug.Log(string.Format("타일 위치 {0} / 타일 이름 : {1} / 타일 번호 : {2}", tileInstance.transform.position , tileInstance.name, tile.Number));
                    }

                    isBlack = !isBlack; // z축에서 한번 꼬아서 격자무늬 만들기
                }

            Debug.Log("게임 시작!");
        }
        else
        {
            Debug.LogError("원본 오브젝트가 할당되지 않았습니다!");
        }
    }

    void ChangeColor(Material instanceMaterial)
    {
        // 검은색과 하얀색 번갈아 가며 변경
        if (isBlack)
        {
            instanceMaterial.color = Color.white;
        }
        else
        {
            instanceMaterial.color = Color.black;
        }
        isBlack = !isBlack; // 상태 토글
    }
}