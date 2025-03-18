using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spirit : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction move;
    private InputAction mousePosition;
    private bool isMoving = false;
    private readonly float speed = 30.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = inputActions.FindActionMap("Spirit").FindAction("Move");
        mousePosition = inputActions.FindActionMap("Spirit").FindAction("MousePosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (move.triggered)
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        if (isMoving) yield break;
        isMoving = true;
        Vector2 position = mousePosition.ReadValue<Vector2>();

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 direction = worldPosition - (Vector2)transform.position;
        while (direction.magnitude > 0.25f)
        {
            transform.position += speed * Time.deltaTime * (Vector3)direction.normalized;
            yield return null;
            direction = worldPosition - (Vector2)transform.position;
        }
        transform.position = worldPosition;
        isMoving = false;
    }
}
