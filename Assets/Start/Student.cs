using UnityEngine;

public class Student : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 dragOffset;
    private Plane dragPlane;
    private float initialY; // 오브젝트의 초기 Y 좌표를 저장할 변수
    public ChessboardGenerator board;

    void Update()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 targetPosition = ray.GetPoint(hitDistance) + dragOffset;
                transform.position = new Vector3(targetPosition.x, 2f, targetPosition.z); // 드는 동안 Y를 2로 유지
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                // 마우스 버튼을 놓았을 때 Y 좌표를 1로 고정
                Vector3 tilePositon = new Vector3(transform.position.x, 1f, transform.position.z);
                Debug.Log(string.Format("Student 타일 위치 {0}", 1));

                transform.position = TileSystem.GetTilePositon(board, TileSystem.GetTileNumber(tilePositon));
                
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 드래그 시작 시 오브젝트의 현재 높이에서 평면을 생성하여 드래그
        dragPlane = new Plane(Vector3.up, transform.position);

        if (dragPlane.Raycast(ray, out float hitDistance))
        {
            dragOffset = transform.position - ray.GetPoint(hitDistance);
        }

        // 드는 순간 Y 좌표를 2로 변경
        initialY = transform.position.y; // 초기 Y 값 저장 (선택 사항)
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
    }

    void OnMouseUp()
    {
        // OnMouseUp은 OnMouseDown과 분리되어 마우스 버튼이 놓였을 때 호출될 수도 있습니다.
        // Update()에서 처리하므로 여기서는 불필요할 수 있지만, 필요에 따라 활용 가능합니다.
    }
}