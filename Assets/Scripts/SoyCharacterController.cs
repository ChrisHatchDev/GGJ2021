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
    private float _hiderSpeed = 3.5f;

    [SerializeField]
    private float _seekerSpeed = 5.0f;

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

    private GameManager _gameManager;
    private MenuManager _menuManager;
    public SoyBoySync _playerDataSync;

    [SerializeField]private LayerMask _shankMask;
    [SerializeField]private float _shankDistance = 0.75f;
    [SerializeField]private SoyBoySync _playerInRange;


    private void Awake() {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
    }

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        _cam = FindObjectOfType<Camera>().transform;
        _camFreeLook = _cam.GetComponent<CinemachineFreeLook>();
        _gameManager = FindObjectOfType<GameManager>();
        _menuManager = FindObjectOfType<MenuManager>();
    }

    void CheckForPlayersInRange()
    {   
        RaycastHit _hit;

        Vector3 p1 = transform.position + _cc.center;
        float distanceToObstacle = 0;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (Physics.SphereCast(p1, 0.5f, transform.forward, out _hit, 2, _shankMask))
        {
            distanceToObstacle = _hit.distance;

            // Debug.Log("Hit distance: " + distanceToObstacle);

            if (distanceToObstacle < _shankDistance)
            {
                if (_hit.collider.tag == "SoyBoy")
                {
                    SoyBoySync _playersComponent = _hit.collider.GetComponent<SoyBoySync>();
                    if (_playerInRange != _playersComponent)
                    {
                        _playerInRange = _playersComponent;
                        Debug.Log("New Player in Range");
                    }
                }
                else
                {
                    _playerInRange = null;
                    Debug.Log("Hit something else name: " + _hit.collider.gameObject.name);
                }
            }
            else
            {
                _playerInRange = null;
            }
        }
        else
        {
            _playerInRange = null;
        }
    }

    public void Shank()
    {
        Debug.Log("Shank called player in range: " + _playerInRange);

        if (_playerInRange != null)
        {
            Debug.Log("Shanked this player: " + _playerInRange._isTagged);
            _playerInRange.TagPlayer(this._playerDataSync);
            Debug.Log("Shanked this player: " + _playerInRange._isTagged);
        }
    }

    void Update()
    {
        // If this CubePlayer prefab is not owned by this client, bail.
        if (!_realtimeView.isOwnedLocallySelf)
        {
            if (_gameManager._remotePlayers.Contains(this._playerDataSync) == false)
            {
                _gameManager.InitializePlayerObject(this._playerDataSync, true);
                _menuManager.InitializeUpdateEvents(this._playerDataSync);
            }
            return;
        }

        if (_gameManager._localPlayer == null)
        {
            _gameManager.InitializePlayerObject(this._playerDataSync, false);
            _menuManager.InitializeUpdateEvents(this._playerDataSync);
        }


        // Return if game hasnt started yet
        if (_gameManager._gameManagerSync._gameState != 1)
        {
            Debug.Log("Game hasnt started or is over, don't move player");
            if (_camFreeLook.enabled == true) {
                _camFreeLook.enabled = false;
            }

            return;
        }

        CheckForPlayersInRange();

        if (Input.GetMouseButtonDown(0))
        {
            Shank();
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

            float _speedToUse = this._playerDataSync._type == 0 ? _hiderSpeed : _seekerSpeed;

            _cc.Move(_moveDir.normalized * _hiderSpeed * Time.deltaTime);
        }
    }
}
