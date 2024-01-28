using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    Vector2 _inputs;
    InputActionAsset inputAsset;
    InputActionMap player;
    public PlayerManager playerManager;

    [SerializeField] bool _inputJump;
    [SerializeField] Rigidbody2D _rb;


    [Header("Movements")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _acceleration;

    [Header("Groundcheck")]
    [SerializeField] float _groundOffset;
    [SerializeField] float _groundRadius;
    [SerializeField] LayerMask _GroundLayer;
    [SerializeField][Tooltip("Collider list of all plateforms of the scene")] Collider2D[] _collidersGround;
    [Tooltip("Touching ground or not")] bool _isGrounded;

    [Header("Jump")]
    [SerializeField][Tooltip("Time minimum between jumps")] float _timerMinBetweenJump;
    [SerializeField] float _jumpForce;
    [SerializeField][Tooltip("Fall speed")] float _velocityFallMin;
    [SerializeField][Tooltip("Gravity when the player goes up and press jump")] float _gravityUpJump;
    [SerializeField][Tooltip("Gravity otherwise")] float _gravity;
    [SerializeField] float _jumpInputTimer = 0.1f;
    float _timerNoJump;
    float _timerSinceJumpPressed;
    float _TimeSinceGrounded;
    float _TimeSinceNotGrounded;

    [Header("Anim")]
    [SerializeField] GameObject PlayerMesh;

    [Header("Sounds")]
    [SerializeField] AudioSource _audioSource;
    //[SerializeField] AudioClip _walkSound;
    //[SerializeField] AudioClip _jumpSound;
    //[SerializeField] AudioClip _hitGround;

    [Header("Idk")]
    [SerializeField] float _coyoteTime;
    [SerializeField] float _slopeDetectOffset;
    bool _isOnSlope;
    [SerializeField] Collider2D _collider;
    [SerializeField] PhysicsMaterial2D _physicsFriction;
    [SerializeField] PhysicsMaterial2D _physicsNoFriction;
    Vector3 _offsetCollisionBox;
    Vector3 _offsetToReplace;
    Vector2 _collisionBox;

    [Header("UI")]
    public GameObject LoseUi;
    public GameObject WinUi;

    RaycastHit2D[] _hitResults = new RaycastHit2D[2];
    float[] directions = new float[] { 1, -1 };
    //public VisualEffect _particules;
    //public GameObject Rooms;
    //public GameObject _cam;
    //public float _offSetCamFall;
    public Animator _animator;
    public RuntimeAnimatorController _animatorControllerSkinny;
    public RuntimeAnimatorController _animatorControllerFat;




    private void Awake()
    {
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("PlayerInputs");

        LoseUi.SetActive(false);
        WinUi.SetActive(false);
    }

    private void OnEnable()
    {
        player.FindAction("Move").performed += GetMoveInputs;
        player.FindAction("Jump").performed += GetJumpInputs;
        player.FindAction("Jump").canceled += GetJumpInputsCanceled;
        player.FindAction("Bomb").started += GetBombInputs;
        player.Enable();
    }

    private void OnDisable()
    {
        player.FindAction("Move").performed -= GetMoveInputs;
        player.FindAction("Jump").performed -= GetJumpInputs;
        player.FindAction("Jump").canceled -= GetJumpInputsCanceled;
        player.FindAction("Bomb").started -= GetBombInputs;
        player.Disable();
    }

    #region inputs
    void GetMoveInputs(InputAction.CallbackContext move)
    {
        _inputs = move.ReadValue<Vector2>();
    }

    void GetJumpInputs(InputAction.CallbackContext jump)
    {
        _inputJump = true;
        _timerSinceJumpPressed = 0;

        if (playerManager.Ld.GetComponentInChildren<End>().isLevelFinished && playerManager.Ld.GetComponentInChildren<End>().canChange)
        {
            playerManager.Ld.GetComponentInChildren<End>().ChangeLd();
        }
    }

    void GetJumpInputsCanceled(InputAction.CallbackContext jump)
    {
        _inputJump = false;
    }

    void GetBombInputs(InputAction.CallbackContext bomb)
    {
        if (playerManager.Ld.GetComponentInChildren<End>().isLevelFinished && playerManager.Ld.GetComponentInChildren<End>().canChange)
        {
            playerManager.Ld.GetComponentInChildren<End>().ChangeLd();
        }
        else
        {
            GetComponentInChildren<PlayerInventory>().PutBomb();
        }
    }
    #endregion

    private void FixedUpdate()
    {
        AnimPlayer();
        HandleMovements();
        HandleGrounded();
        HandleJump();
        HandleSlope();
        HandleCorners();
    }

    #region move ground n jump
    void HandleMovements()
    {
        var velocity = _rb.velocity;
        Vector2 wantedVelocity = new Vector2(_inputs.x * _walkSpeed, velocity.y);
        _rb.velocity = Vector2.MoveTowards(velocity, wantedVelocity, _acceleration * Time.deltaTime);

        //if (_rb.velocity.x != 0 && _isGrounded)
        //{
        //    PlaySound(_walkSound, _audioSource);
        //}
    }

    Vector2 point;

    void HandleGrounded()
    {
        _TimeSinceGrounded += Time.deltaTime;

        point = transform.position + Vector3.up * _groundOffset;
        bool currentGrounded = Physics2D.OverlapCircleNonAlloc(point, _groundRadius, _collidersGround, _GroundLayer) > 0;

        if (currentGrounded == false && _isGrounded)
        {
            _TimeSinceGrounded = 0;
        }

        _isGrounded = currentGrounded;

        if (_isGrounded)
        {
            _TimeSinceNotGrounded = 0;
        }
        else
        {
            _TimeSinceNotGrounded += Time.deltaTime;
        }

        bool currentTouching = Physics2D.OverlapCircleNonAlloc(new Vector2(point.x, point.y - 0.2f), _groundRadius, _collidersGround, _GroundLayer) > 0;

        //if (currentTouching && _rb.velocity.y < 0 && currentGrounded == false)
        //{
        //    PlaySound(_hitGround, _audioSource);
        //}
        //
        //if (_TimeSinceNotGrounded > 3)
        //{
        //    Rooms.SetActive(false);
        //}
        //else if (_isGrounded)
        //{
        //    Rooms.SetActive(true);
        //}
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point, _groundRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(point.x, point.y - 0.2f), _groundRadius);
        Gizmos.color = Color.white;
    }


    void HandleJump()
    {
        _timerNoJump -= Time.deltaTime;
        _timerSinceJumpPressed += Time.deltaTime;

        //Limite vitesse chute
        if (_rb.velocity.y < _velocityFallMin)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _velocityFallMin);
        }

        if (_isGrounded == false)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _gravity;
            }
            else
            {
                _rb.gravityScale = _inputJump ? _gravityUpJump : _gravity;
            }
        }
        else
        {
            _rb.gravityScale = _gravity;
        }

        if (_inputJump && (_rb.velocity.y <= 0 || _isOnSlope) && (_isGrounded || _TimeSinceGrounded < _coyoteTime) && _timerNoJump <= 0 && _timerSinceJumpPressed < _jumpInputTimer)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _timerNoJump = _timerMinBetweenJump;

            //PlaySound(_jumpSound, _audioSource);
            //
            //_particules.SendEvent("OnPlay");
        }
    }
    #endregion

    #region slope n corner
    void HandleSlope()
    {
        Vector3 origin = transform.position + Vector3.up * _groundOffset;
        bool slopeRight = Physics2D.RaycastNonAlloc(origin, Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;
        bool slopeLeft = Physics2D.RaycastNonAlloc(origin, -Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;

        _isOnSlope = (slopeRight || slopeLeft) && (slopeRight == false || slopeLeft == false);

        if (Mathf.Abs(_inputs.x) < 0.1f && (slopeLeft || slopeRight))
        {
            _collider.sharedMaterial = _physicsFriction;
        }
        else
        {
            _collider.sharedMaterial = _physicsNoFriction;
        }
    }

    void HandleCorners()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            float dir = directions[i];

            if (Mathf.Abs(_inputs.x) > 0.1f && Mathf.Abs(Mathf.Sign(dir) - Mathf.Sign(_inputs.x)) < 0.001f && _isGrounded == false && _isOnSlope == false)
            {
                Vector3 position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x, _offsetCollisionBox.y, 0);
                int result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero, _hitResults, 0, _GroundLayer);

                if (result > 0)
                {
                    position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x, _offsetCollisionBox.y + _offsetToReplace.y, 0);
                    result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero, _hitResults, 0, _GroundLayer);

                    if (result == 0)
                    {
                        Debug.Log("replace");
                        transform.position += new Vector3(dir * _offsetToReplace.x, _offsetToReplace.y);

                        if (_rb.velocity.y < 0)
                        {
                            _rb.velocity = new Vector2(_rb.velocity.x, 0);
                        }
                    }
                }
            }
        }
    }
    #endregion

    void AnimPlayer()
    {
        if (_inputs.x != 0)
        {
            float side = 0;

            if (_inputs.x < 0)
                side = -0.1f;
            else
                side = 0.1f;

            if (PlayerMesh.transform.localScale.x != side && _rb.velocity.y == 0)
            {
                //_particules.SendEvent("OnPlay");
            }

            PlayerMesh.transform.localScale = new Vector3(side, PlayerMesh.transform.localScale.y, PlayerMesh.transform.localScale.z);
        }



        if (_inputs.x == 0 && _rb.velocity.y == 0)
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Run", false);
            _animator.SetBool("Jump", false);
        }
        else if (_rb.velocity.y != 0)
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("Run", false);
            _animator.SetBool("Jump", true);
        }
        else if (_inputs.x != 0 && _isGrounded)
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("Run", true);
            _animator.SetBool("Jump", false);
        }
    }

    public void AnimEat()
    {
        _animator.SetTrigger("Eat");
    }

    public void PlaySound(AudioClip _sound, AudioSource _audioSource)
    {
        if (_audioSource.isPlaying && _audioSource.clip == _sound)
        {
            return;
        }
        else
        {
            _audioSource.clip = _sound;
            _audioSource.Play();
        }
    }
}
