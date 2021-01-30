using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameManager _gameManager;

    [SerializeField]
    public Text _typeText;

    [SerializeField]
    public Text _remotePlayerTypeText;

    [SerializeField]
    public Text _lobbyTipText;

    [SerializeField]
    public Text _gameStatusText;

    [SerializeField]
    public Text _hidingTimerText;

    private int hidingTimer = 0;

    private void Start()
    {
        _lobbyTipText.text = "_UnsetLobbyTip";
        _gameStatusText.text = "Waiting for Players to Join...";
    }

    public void InitializeUpdateEvents(SoyBoySync player)
    {
        player.onTypeChange.AddListener(UpdateTextObjects);

        _gameManager._gameManagerSync.onGameStateChange.AddListener(UpdateGameStatusText);
        _gameManager._gameManagerSync.onLobbyStatusChange.AddListener(LobbyStatusChanged);

        LobbyStatusChanged();
        // UpdateGameStatusText();
    }

    public void UpdateGameStatusText()
    {
        if (_gameManager._gameManagerSync._gameState == 0)
        {
            _gameStatusText.text = "Game Hasnt Started Yet";
        }
        if (_gameManager._gameManagerSync._gameState == 1)
        {
            _gameStatusText.text = "Game is In Progress";
        }
        if (_gameManager._gameManagerSync._gameState == 2)
        {
            _gameStatusText.text = "Go Hide!";
            
            hidingTimer = 20;
            StartCoroutine(HidingSequenceTimer());
        }
        if (_gameManager._gameManagerSync._gameState == 3)
        {
            _gameStatusText.text = "Game Over!";
        }
    }

    IEnumerator HidingSequenceTimer()
    {
        hidingTimer -= 1;

        if (hidingTimer <= 0)
        {
            _hidingTimerText.text = "Run!";
        }
        else
        {
            _hidingTimerText.text = hidingTimer.ToString();
        }

        yield return new WaitForSeconds(1);

        if (hidingTimer > 0)
        {        
            StartCoroutine(HidingSequenceTimer());
        }
    }

    public void LobbyStatusChanged()
    {
        _lobbyTipText.text = _gameManager._gameManagerSync._lobbyStatus;
    }

    public void ChooseHider()
    {
        _gameManager._localPlayer.SetPlayerType(0);
        UpdateTextObjects();
    }

    public void ChooseSeeker()
    {
        _gameManager._localPlayer.SetPlayerType(1);
        UpdateTextObjects();
    }

    private void UpdateTextObjects()
    {
        if (_gameManager._localPlayer._type == 0)
        {
            _typeText.text = "Hider";
        }
        else if (_gameManager._localPlayer._type == 1)
        {
            _typeText.text = "Seeker";
        }
        else
        {
            _typeText.text = "Not Selected";
        }

        foreach (var remoteP in _gameManager._remotePlayers)
        {
            if (remoteP._type == 0)
            {
                _remotePlayerTypeText.text = "Hider";
            }
            else if (remoteP._type == 1)
            {
                _remotePlayerTypeText.text = "Seeker";
            }
            else
            {
                _remotePlayerTypeText.text = "Not Selected";
            }
        }
    }

    private void OnGameStatusChange()
    {
        _gameStatusText.text = "Game Started!";
    }

    private void OnGameStart()
    {
        _gameStatusText.text = "Game Started!";
    }

}
