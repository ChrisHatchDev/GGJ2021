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
    private Camera _cam;

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


    [SerializeField]
    private Animator _ccAnim;

    [SerializeField]
    private bool _crouchButtonPressed;
    private float _timeCrouchHasBeenPressed = 0.0f;
    
    // 0 = Standing, 1 = Crouched, 2 = Prone
    [SerializeField]
    private int _stanceState;

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
        _cam = FindObjectOfType<Camera>();
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

    void DisableCameraControls()
    {
        if (_camFreeLook.enabled == true) {
            _camFreeLook.enabled = false;
        }
    }

    void EnableCameraControls()
    {
        if (_camFreeLook.enabled == false) {
            _camFreeLook.enabled = true;
        }
    }

    void GoIntoBlackScreenMode()
    {
        //Hide Camera View as Stub for Black Screen
        if (_cam.enabled == true)
        {
            _cam.enabled = false;
        }
        DisableCameraControls();
    }

    void ExitBlackScreenMode()
    {
        if (_cam.enabled == false)
        {
            _cam.enabled = true;
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
                _playerDataSync.onStanceChanged.AddListener(RemoteCrouchLogicResponse);
                RemoteCrouchLogicResponse();
            }
            return;
        }

        if (_gameManager._localPlayer == null)
        {
            _gameManager.InitializePlayerObject(this._playerDataSync, false);
            _menuManager.InitializeUpdateEvents(this._playerDataSync);
        }

        // Maybe move crouch further down to disable it while in lobby, but for now have fun
        CrouchLogic();

        // Regardless of Player Type if we are in lobby, dont enable character controller
        if (_gameManager._gameManagerSync._gameState == 0)
        {
            return; // Dont allow movement
        }

        // Player is Seeker
        if (_gameManager._localPlayer._type == 1)
        {
            if (_gameManager._gameManagerSync._gameState == 2)
            { 
                GoIntoBlackScreenMode();
                DisableCameraControls();
                return; // Dont allow movement
            }
            else
            {
                ExitBlackScreenMode();
                EnableCameraControls();
            }
        }

        // Player is Hider
        if (_gameManager._localPlayer._type == 0)
        {
            int _gameState = _gameManager._gameManagerSync._gameState;

            if (_gameState == 0 || _gameState == 3)
            {
                DisableCameraControls();
                return; // Dont allow movement
            }
        }

        // Return if game hasnt started yet or its hiding mode and you are the hider
        if (_gameManager._gameManagerSync._gameState == 0 || _gameManager._gameManagerSync._gameState == 3)
        {
            Debug.Log("Game hasnt started or is over, don't move player");
            DisableCameraControls();

            return;
        }
        else
        {
            EnableCameraControls();
        }

    // Made it to player is in game and should be able to control their character

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
            float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
            float _smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, _smoothedAngle, 0f);

            Vector3 _moveDir = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;

            float _speedToUse = this._playerDataSync._type == 0 ? _hiderSpeed : _seekerSpeed;

            if (_stanceState == 0)
            {
                _speedToUse *= 1.0f;
            }
            else if (_stanceState == 1)
            {
                _speedToUse *= 0.8f;
            }
            else if (_stanceState == 2)
            {
                _speedToUse *= 0.5f;
            }
            
            // Trigger Walk Animation
            if (Mathf.Abs(_vertical) > 0.1f || Mathf.Abs(_horizontal) > 0.1f)
            {
                _ccAnim.SetFloat("WalkSpeed", 1);
            }
            else
            {
                _ccAnim.SetFloat("WalkSpeed", 0);
            }
            
            _cc.Move(_moveDir.normalized * _speedToUse * Time.deltaTime);
        }
    }

    void RemoteCrouchLogicResponse ()
    {
        _stanceState = _playerDataSync._stance;

        if (_stanceState == 2)
        {
            Debug.Log("Should Go Prone");
            GoProne();
        }
        
        if (_stanceState == 1)
        {
            Debug.Log("Should Crouch Only");
            Crouch();
        }

        if (_stanceState == 0)
        {
            StandUp();
        }
    }

    void CrouchLogic ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _crouchButtonPressed = true;
            _timeCrouchHasBeenPressed = 0.0f;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _crouchButtonPressed = false;
            _timeCrouchHasBeenPressed = 0.0f;
        }

        if (_crouchButtonPressed)
        {
            _timeCrouchHasBeenPressed += Time.deltaTime;
        }

        if (_crouchButtonPressed && _timeCrouchHasBeenPressed > 0.5f)
        {
            Debug.Log("Should Go Prone");
            if (_stanceState != 2)
            {
                GoProne();
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && _timeCrouchHasBeenPressed < 0.5f)
        {
            Debug.Log("Should Crouch Only");
            // If we are already crouched standup. If not, then crouch
            if(_stanceState == 1 || _stanceState == 2)
            {
                StandUp();
            }
            else
            {
                Crouch();
            }
        }
    }

    void GoProne()
    {
        _stanceState = 2;
        _ccAnim.SetInteger("Stance", _stanceState);

        _cc.height = 0.2f;
        _cc.center = new Vector3(_cc.center.x, -0.82f, _cc.center.z);
    }

    void Crouch()
    {
        _stanceState = 1;
        _ccAnim.SetInteger("Stance", _stanceState);

        _cc.height = 0.6f;
        _cc.center = new Vector3(_cc.center.x, -0.71f, _cc.center.z);
    }

    void StandUp()
    {
        _stanceState = 0;
        _ccAnim.SetInteger("Stance", _stanceState);

        _cc.height = 1.2f;
        _cc.center = new Vector3(_cc.center.x, -0.47f, _cc.center.z);
    }
}
