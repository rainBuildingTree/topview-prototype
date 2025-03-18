using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction move;
    [SerializeField] private InputActionAsset inputActions;
    private readonly float speed = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = inputActions.FindActionMap("Player").FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = move.ReadValue<Vector2>();
        transform.position += speed * Time.deltaTime * (Vector3)direction;
    }
}
