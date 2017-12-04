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
    public Animator PlayerAnimator
    {
        get
        {
            return _playerAnimator;
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

    private int _puppiesCount;
    public int PuppiesCount
    {
        get
        {
            return _puppiesCount;
        }
    }

    private Item _hat;
    private Item _boots;
    private Item _gloves;

    private int _coldLevel;
    public int ColdLevel
    {
        get
        {
            return _coldLevel;
        }
    }

    private int _limitColdLevel = 15;

	void Start ()
    {
        _inventory = new Inventory();
    }

    public void Initialize()
    {
        _playerInput.Active = true;

        _currentAnimation = PlayerAnimation.IdleDown;

        _coldLevel = _limitColdLevel;
        UpdateStatsUI();
        UIManager.Instance.StatsView.SetPuppyCounter(_puppiesCount);
    }

    void Update()
    {
        if (_playerInput.Active == false || UIManager.Instance.CurrentState != UIManagerState.Available)
        {
            return;
        }

        if (_coldLevel <= 0)
        {
            _playerAnimator.SetTrigger("Dead");
            UIManager.Instance.DialogueView.ShowText("You died frozen", () =>
            {
                _coldLevel = _limitColdLevel;
                UIManager.Instance.DialogueView.Hide();
                _playerInput.Active = false;
                GameManager.Instance.Reboot();
            });
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
           if (_currentColdDecreaseTime >= _coldDecreaseTimeBase + GetItemsColdResistance())
           {
                _currentColdDecreaseTime = 0;
                _coldLevel--;

                float newColdFill =(float) _coldLevel / _limitColdLevel;

                UIManager.Instance.StatsView.FillColdBar(newColdFill);
           }

        }
    }

    public void SetPuppiesCount(int count)
    {
        _puppiesCount = count;
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

        int bootsValue = _boots != null ? _boots.GetStatValue(StatType.Speed) : 0;
        int glovesValue = _gloves != null ? _gloves.GetStatValue(StatType.Strenght) : 0;

        _speed = Mathf.Max(0.1f, _speed + bootsValue - _puppiesCount + glovesValue);

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

    private int GetItemsColdResistance()
    {
        int count = 0;

        if (_hat != null)
        {
            foreach(Stat stat in _hat.Stats)
            {
                if (stat.Type == StatType.ColdResistance)
                {
                    count += stat.Value;
                }
            }
        }

        if (_boots != null)
        {
            foreach (Stat stat in _boots.Stats)
            {
                if (stat.Type == StatType.ColdResistance)
                {
                    count += stat.Value;
                }
            }
        }

        if (_gloves != null)
        {
            foreach (Stat stat in _gloves.Stats)
            {
                if (stat.Type == StatType.ColdResistance)
                {
                    count += stat.Value;
                }
            }
        }

        return count;
    }

    private void UpdateStatsUI()
    {
        UIManager.Instance.StatsView.UpdateStat(1, GetItemsColdResistance());

        if (_gloves != null)
        {
            UIManager.Instance.StatsView.UpdateStat(2, _gloves.GetStatValue(StatType.Strenght));
        }
        else
        {
            UIManager.Instance.StatsView.UpdateStat(2, 0);
        }
       
        if (_boots != null)
        {
            UIManager.Instance.StatsView.UpdateStat(3, _boots.GetStatValue(StatType.Speed));
        }
        else
        {
            UIManager.Instance.StatsView.UpdateStat(3, 0);
        }
    }

    public void EquipHat(Item hat)
    {
        _hat = hat;
        UIManager.Instance.StatsView.UpdateStat(1, GetItemsColdResistance());
    }

    public void EquipGloves(Item gloves)
    {
        _gloves = gloves;
        UIManager.Instance.StatsView.UpdateStat(2, _gloves.GetStatValue(StatType.Strenght));
    }

    public void EquipBoots(Item boots)
    {
        _boots = boots;
        UIManager.Instance.StatsView.UpdateStat(3, _boots.GetStatValue(StatType.Speed));
    }
}
