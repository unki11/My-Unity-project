using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dragSpeed = 1f;
    private Vector3 lastMousePos;

    void Update()
    {
        HandleKeyboardMove();
        HandleMouseDrag();
    }

    void HandleKeyboardMove()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼 눌렀을 때 시작
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) // 드래그 중
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * dragSpeed * Time.deltaTime;

            transform.Translate(move, Space.World);
            lastMousePos = Input.mousePosition;
        }
    }
}
