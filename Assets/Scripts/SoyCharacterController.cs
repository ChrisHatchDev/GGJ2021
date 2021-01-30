using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Cinemachine;

public class SoyCharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _cc;

    [SerializeField]
    private Transform _cam;

    [SerializeField]
    private CinemachineFreeLook _camFreeLook;

    [SerializeField]
    private float _speed = 6;

    [SerializeField]
    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;
    
    private Vector3 _currentGravityVelocity = new Vector3();
    [SerializeField]private float _gravityScale = -9.81f;
    [SerializeField]private Transform _groundCheck;
    [SerializeField]private LayerMask _groundMask;
    [SerializeField]private bool _isGrounded;
    private float _groundDistance = 0.2f;

    private MenuManager _menuManager;
    private GameManager _gameManager;
    public SoyBoySync _playerDataSync;

    private void Awake() {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
    }

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        _cam = FindObjectOfType<Camera>().transform;
        _camFreeLook = _cam.GetComponent<CinemachineFreeLook>();
        _menuManager = FindObjectOfType<MenuManager>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // If this CubePlayer prefab is not owned by this client, bail.
        if (!_realtimeView.isOwnedLocallySelf)
        {
            if (_menuManager._remotePlayer == null)
            {
                _menuManager.InitializeEvents(this._playerDataSync, true);
                _gameManager.InitializeEvents(this._playerDataSync, true);
            }
            return;
        }

        if (_menuManager._localPlayer == null)
        {
            _menuManager.InitializeEvents(this._playerDataSync, false);
            _gameManager.InitializeEvents(this._playerDataSync, false);
        }

        if (_camFreeLook.enabled == false) {
            _camFreeLook.enabled = true;
        }
        if (_camFreeLook.Follow == null) {
            _camFreeLook.Follow = this.transform;
            _camFreeLook.LookAt = this.transform;
        }

        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _currentGravityVelocity.y < 0) {
            _currentGravityVelocity.y = -2f;
        }

        // Apply Gravity
        _currentGravityVelocity.y += (_gravityScale * Time.deltaTime);
        _cc.Move(_currentGravityVelocity * Time.deltaTime);

        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
        _realtimeTransform.RequestOwnership();

        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        Vector3 _direction = new Vector3(_horizontal, 0, _vertical).normalized;

        if (_direction.magnitude >= 0.1f)
        {
            float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            float _smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, _smoothedAngle, 0f);

            Vector3 _moveDir = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;
            _cc.Move(_moveDir.normalized * _speed * Time.deltaTime);
        }
    }
}
