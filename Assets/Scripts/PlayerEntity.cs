using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

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
    
	void Start ()
    {
        _playerInput.Active = true;
	}

    void Update()
    {
        if (_cameraFollow)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }

    void FixedUpdate ()
    {
        if (_playerInput.Active == false)
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

        transform.position = new Vector3(xMovement, yMovement, transform.position.z);
    }
}
