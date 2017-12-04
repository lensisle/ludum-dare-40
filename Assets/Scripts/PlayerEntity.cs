using UnityEngine;

public enum PlayerAnimation
{
    IdleDown,
    IdleUp,
    IdleRight,
    IdleLeft,
    WalkUp,
    WalkDown,
    WalkLeft,
    WalkRight,
    Dead
}

public class PlayerEntity : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    public PlayerInput PlayerInput
    {
        get
        {
            return _playerInput;
        }
    }

    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _cameraFollow;

    [SerializeField]
    private SpriteRenderer _playerRenderer;
    public SpriteRenderer PlayerRenderer
    {
        get
        {
            return _playerRenderer;
        }
    }

    [SerializeField]
    private StatsData _initialStatsData;

    [SerializeField]
    private Animator _playerAnimator;

    private Stats _stats;
    public Stats Stats
    {
        get
        {
            return _stats;
        }
    }

    private bool _isStatVulnerable;
    public bool IsStatVulnerable
    {
        get
        {
            return _isStatVulnerable;
        }
    }

    private float _coldDecreaseTimeBase = 2;
    private float _currentColdDecreaseTime;

    private Inventory _inventory;
    public Inventory Inventory
    {
        get
        {
            return _inventory;
        }
    }

    [SerializeField]
    private SpriteRenderer _exclamation;
    public SpriteRenderer Exclamation
    {
        get
        {
            return _exclamation;
        }
    }

    private PlayerAnimation _currentAnimation;

	void Start ()
    {
        _inventory = new Inventory();
        _playerInput.Active = true;
        _stats = new Stats(_initialStatsData.InitialStatsData);

        _currentAnimation = PlayerAnimation.IdleDown;
    }

    void Update()
    {
        if (_playerInput.Active == false || UIManager.Instance.CurrentState != UIManagerState.Available)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            UIManager.Instance.InventoryView.Show();
            return;
        }

        if (_cameraFollow)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }

        if (_isStatVulnerable)
        {

            _currentColdDecreaseTime += Time.deltaTime;
           if (_currentColdDecreaseTime >= _coldDecreaseTimeBase + _stats.GetStat(StatType.ColdResistance).Value)
           {
                _currentColdDecreaseTime = 0;
                _stats.SetStatValue(StatType.Cold, 1, true);
           }

        }
    }

    public void SetIsStatVulnerable(bool statVulnerable)
    {
        _isStatVulnerable = statVulnerable;
    }

    void FixedUpdate ()
    {
        if (_playerInput.Active == false || UIManager.Instance.CurrentState != UIManagerState.Available)
        {
            return;
        }

        var xMovement = _playerInput.LeftPressed ? _speed * Time.deltaTime * -1 :
            _playerInput.RightPressed ? _speed * Time.deltaTime :
            0;

        xMovement = xMovement + transform.position.x;

        var yMovement = _playerInput.UpPressed ? _speed * Time.deltaTime * 1 :
            _playerInput.DownPressed ? _speed * Time.deltaTime * -1 :
            0;

        yMovement = yMovement + transform.position.y;
        
        if (_playerInput.LeftPressed == false && _playerInput.RightPressed == false && _playerInput.DownPressed == false && _playerInput.UpPressed && _currentAnimation != PlayerAnimation.WalkUp)
        {
            _currentAnimation = PlayerAnimation.WalkUp;
            _playerAnimator.SetTrigger("WalkUp");
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_playerInput.LeftPressed == false && _playerInput.RightPressed == false && _playerInput.UpPressed == false && _playerInput.DownPressed && _currentAnimation != PlayerAnimation.WalkDown)
        {
            _currentAnimation = PlayerAnimation.WalkDown;
            _playerAnimator.SetTrigger("WalkDown");
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_playerInput.RightPressed == false && _playerInput.DownPressed == false && _playerInput.UpPressed == false && _playerInput.LeftPressed && _currentAnimation != PlayerAnimation.WalkLeft)
        {
            _currentAnimation = PlayerAnimation.WalkLeft;
            _playerAnimator.SetTrigger("WalkRight");
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_playerInput.LeftPressed == false && _playerInput.DownPressed == false && _playerInput.UpPressed == false && _playerInput.RightPressed && _currentAnimation != PlayerAnimation.WalkRight)
        {
            _currentAnimation = PlayerAnimation.WalkRight;
            _playerAnimator.SetTrigger("WalkRight");
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (_playerInput.LeftPressed == false && _playerInput.RightPressed == false && _playerInput.DownPressed == false && _playerInput.UpPressed == false)
        {
            if (_currentAnimation == PlayerAnimation.WalkDown)
            {
                _currentAnimation = PlayerAnimation.IdleDown;
                _playerAnimator.SetTrigger("IdleDown");
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_currentAnimation == PlayerAnimation.WalkUp)
            {
                _currentAnimation = PlayerAnimation.WalkUp;
                _playerAnimator.SetTrigger("IdleUp");
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_currentAnimation == PlayerAnimation.WalkLeft)
            {
                _currentAnimation = PlayerAnimation.IdleLeft;
                _playerAnimator.SetTrigger("IdleRight");
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_currentAnimation == PlayerAnimation.WalkRight)
            {
                _currentAnimation = PlayerAnimation.IdleRight;
                _playerAnimator.SetTrigger("IdleRight");
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        transform.position = new Vector3(xMovement, yMovement, transform.position.z);
    }
}
