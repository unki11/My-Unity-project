using UnityEngine;

public static class TileSystem
{
    public static float tileWidth = 2.0f;   // 타일의 가로 크기 (위치 간격으로 추정)
    public static float tileDepth = 2.0f;   // 타일의 깊이 크기 (위치 간격으로 추정)
    public static Vector3 boardOrigin = new Vector3(-8.0f, 0.0f, -1.0f); // 체스판의 시작 위치 (첫 번째 타일의 왼쪽 아래 모서리 기준으로 조정 필요)
    public static int tilesPerColum = 4;
    public static int tilesPerRow = 8;      // 한 줄에 있는 타일 개수

    public static int GetTileNumber(Vector3 worldPosition)
    {
        // 마우스 위치를 체스판 시작 위치 기준으로 변환합니다.
        Vector3 relativePos = worldPosition - boardOrigin;

        // X, Z 축 기준으로 타일 인덱스를 계산합니다.
        int xIndex = Mathf.FloorToInt(relativePos.x / tileWidth);
        int zIndex = Mathf.FloorToInt(relativePos.z / tileDepth);

        Debug.Log(string.Format("xIndex : {0}  zIndex : {1} ", xIndex, zIndex));

        // 인덱스가 유효한 범위 내에 있는지 확인합니다.
        if (xIndex >= 0 && xIndex < tilesPerRow && zIndex >= 0 && zIndex < tilesPerColum)
        {
            // 타일 번호를 계산합니다 (1부터 시작).
            int tileNumber = zIndex * tilesPerRow + xIndex + 1;
            Debug.Log("마우스 над 타일 번호: " + tileNumber);
            return tileNumber;
        }

        return 0;
    }

    public static Vector3 GetTilePositon(ChessboardGenerator board, int tileNumber){
        // 씬에 있는 모든 Tile 컴포넌트를 찾습니다.
        Tile[,] allTiles = board.tiles;
        int rows = board.rowCnt;
        int cols = board.columnCnt;

        if (tileNumber > 0) {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (allTiles[i, j] != null && allTiles[i, j].Number == tileNumber)
                    {

                        return allTiles[i, j].GetTileWorldPosition();
                    }
                }
            }
        }else{
            Debug.Log("타일 번호 " + tileNumber + "를 찾을 수 없습니다.");
        }

        return allTiles[0, 0].GetTileWorldPosition();
    }

}