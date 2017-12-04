using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueView : MonoBehaviour
{
    [SerializeField]
    private Text _textLabel;

    [SerializeField]
    private float _charDelay = 0.3f;

    private string _currentTextChunk;
    private float _currentTime;
    private int _currentChar;

    private Action _onFinishCallback;

	public void ShowText(string text, Action onFinishCallback)
    {
        if (UIManager.Instance.CurrentState != UIManagerState.Available)
        {
            Debug.Log("Can't use UIManager now");
            return;
        }

        _textLabel.text = string.Empty;
        gameObject.SetActive(true); 
        UIManager.Instance.SetState(UIManagerState.ShowingText);
        _currentTime = 0;
        _currentTextChunk = text;
        _currentChar = 0;
        _onFinishCallback = onFinishCallback;
    }

    private void Update()
    {
        if (UIManager.Instance.CurrentState == UIManagerState.ShowingText)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _charDelay)
            {
                _currentChar += 1;
                _currentTime = 0;
                _textLabel.text = _currentTextChunk.Substring(0, _currentChar);
                if (_currentChar >= _currentTextChunk.Length)
                {
                    UIManager.Instance.SetState(UIManagerState.WaitingResponse);
                }
            }
        }
        else if (UIManager.Instance.CurrentState == UIManagerState.WaitingResponse && Input.GetKeyDown(KeyCode.Z))
        {
            UIManager.Instance.SetState(UIManagerState.Available);

            if (_onFinishCallback != null)
            {
                _onFinishCallback();
            }
        }
    }

    public void Hide()
    {
        UIManager.Instance.SetState(UIManagerState.Available);
        gameObject.SetActive(false);
    }
}
