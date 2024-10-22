using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 5;
    [SerializeField] private float _runSpeed = 8;
    [SerializeField] private float _rotateSpeed = 100;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private float _gravity = -9.81f;



    [SerializeField] private float _pickUpDistance = 5;
    [SerializeField] private float _trowForce = 6;
    [SerializeField] private LayerMask _canPickUpLayer;

    private CharacterController _characterController;
    private Camera _playerCamera;

    private Vector3 _velocity;
    private Vector2 _rotation;
    private Vector2 _inputDirection;

    private Joint _joint;
    private Rigidbody _currentRigidbodyObject;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;

        _joint = GetComponentInChildren<Joint>();
    }

    private void Update()
    {
        
        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

       
        if (_characterController.isGrounded)
        {
            _velocity.y = Input.GetKeyDown(KeyCode.Space) ? _jumpForce : -0.1f;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0)) PickUp();
        if (Input.GetMouseButtonUp(0)) Drop();
        if (Input.GetMouseButtonDown(1)) Drop(true);

        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDelta *= _rotateSpeed * Time.deltaTime;
        _rotation.y += mouseDelta.x;
        _rotation.x = Mathf.Clamp(_rotation.x - mouseDelta.y, -90, 90);
        _playerCamera.transform.localEulerAngles = new Vector3(_rotation.x, 0, 0);
        transform.Rotate(Vector3.up * mouseDelta.x); 
    }

    private void FixedUpdate()
    {
       
        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;

        
        Vector3 move = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;

        
        _characterController.Move(move * speed * Time.deltaTime);

        
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void PickUp()
    {
        if (!Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out RaycastHit hit, _pickUpDistance, _canPickUpLayer)) return;

        _currentRigidbodyObject = hit.collider.gameObject.GetComponent<Rigidbody>();

        _currentRigidbodyObject.drag = 15;

        _joint.gameObject.transform.position = _currentRigidbodyObject.gameObject.transform.position;
        _joint.connectedBody = _currentRigidbodyObject;

    }
    
    private void Drop(bool isTrow = false)
    {
        if (_currentRigidbodyObject == null) return;

        _joint.connectedBody = null;

        _currentRigidbodyObject.velocity = _velocity;
        if(isTrow) _currentRigidbodyObject.AddForce(_playerCamera.transform.forward * _trowForce, ForceMode.Impulse);
        _currentRigidbodyObject.drag = 0;

        _currentRigidbodyObject = null;
    }
}
