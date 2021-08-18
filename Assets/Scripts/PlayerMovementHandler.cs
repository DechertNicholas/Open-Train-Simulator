using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _rotateRate = 8f;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private GameObject _camera;
    [SerializeField]
    [Range(0f, 1f)]
    private float _lookSensitivity = 0.5f;
    private CharacterController _characterController = null;
    private Vector3 _movementVector = new Vector3();
    float _cameraXRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Debug.Log("Trying to move: " + _movementVector * Time.deltaTime * _speed);
        _characterController.Move(_movementVector * Time.deltaTime * _speed);

    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        var values = ctx.ReadValue<Vector2>();
        _movementVector.x = values.x;
        _movementVector.z = values.y;
        Debug.Log("Set movement input: " + _movementVector);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        var values = ctx.ReadValue<Vector2>();
        Debug.Log("Read mouse values: " + values);

        _cameraXRotation -= values.y * Time.deltaTime;
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(_cameraXRotation * _lookSensitivity, 0f, 0f);
        _playerBody.Rotate(Vector3.up * values.x * Time.deltaTime * _lookSensitivity);
    }
}
