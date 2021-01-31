using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameManager _gameManager;

    [SerializeField]
    public GameObject _mainMenu;

    [SerializeField]
    public GameObject _seekerDelayMenu;

    [SerializeField]
    public GameObject _inGameMenu;

    [SerializeField]
    public Button _becomeHiderButton;

    [SerializeField]
    public Button _becomeSeekerButton;

    [SerializeField]
    public Button _startGameButton;

    [SerializeField]
    public Transform _hidersGrid;

    [SerializeField]
    public Transform _hidersCardPrefab;

    [SerializeField]
    public List<PlayerCardUIManager> _hiderCards = new List<PlayerCardUIManager>();

    [SerializeField]
    public PlayerCardUIManager _seekerCard;

    [SerializeField]
    public Text _yourNameText;

    [SerializeField]
    public Text _lobbyTipText;

    [SerializeField]
    public Text _gameStatusText;

    [SerializeField]
    public Text _hidingTimerText;

    public PlayerCardUIManager _activeHiderCard = null;

    private int hidingTimer = 0;

    private void Start()
    {
        _lobbyTipText.text = "_UnsetLobbyTip";
        _gameStatusText.text = "Waiting for Players to Join...";
    }

    public void ToggleStartButton(bool onOff)
    {
        _startGameButton.interactable = onOff;
    }

    private void Update()
    {
        ToggleStartButton(_gameManager.CanStartGame());
    }

    public void InitializeUpdateEvents(SoyBoySync player)
    {
        // Spawn as Hider
        if (IsSomeoneSeeker())
        {
            player.SetPlayerType(0);

            PlayerCardUIManager newCard = Instantiate(_hidersCardPrefab, transform.position, Quaternion.identity, _hidersGrid).GetComponent<PlayerCardUIManager>();
            player.SetPlayerName(player.playerNames[Random.Range(0, player.playerNames.Count)]);
            newCard.InitializeCard(player);
            _hiderCards.Add(newCard);
            _activeHiderCard = newCard;

            _yourNameText.text = "Your Name: " + player._playerName;
        }
        else
        {
            // Spawn as Seeker Filling that Spot
            player.SetPlayerType(1);
            player.SetPlayerName(player.playerNames[Random.Range(0, player.playerNames.Count)]);
            _seekerCard.InitializeCard(player);

            _yourNameText.text = "Your Name: " + player._playerName;
        }

        player.onTypeChange.AddListener(OnTypeChange);

        _gameManager._gameManagerSync.onGameStateChange.AddListener(UpdateGameStatusText);
        _gameManager._gameManagerSync.onLobbyStatusChange.AddListener(LobbyStatusChanged);

        LobbyStatusChanged();
        UpdateGameStatusText();
        UpdateTextObjects();
        OnTypeChange();
    }

    PlayerCardUIManager GetRemotePlayerCard(SoyBoySync remoteP)
    {
        PlayerCardUIManager _hiderCardToRemove = null;

        foreach (var hiderCard in _hiderCards)
        {
            if (hiderCard._assignedPlayer == remoteP)
            {
                _hiderCardToRemove = hiderCard;
            };
        }

        return _hiderCardToRemove;
    }

    void OnTypeChange()
    {
        Debug.Log("Menu Manager On Type Changed Ran");
        if (_gameManager._localPlayer?._type == 0)
        {
            _becomeHiderButton.interactable = false;
            _becomeSeekerButton.interactable = true;
        }
        else if (_gameManager._localPlayer?._type == 1)
        {
            _becomeHiderButton.interactable = true;
            _becomeSeekerButton.interactable = false;
        }
        
        UpdateTextObjects();

        foreach (var remoteP in _gameManager._remotePlayers)
        {
            if (remoteP._type == 0 && GetRemotePlayerCard(remoteP) == null)
            {
                PlayerCardUIManager newCard = Instantiate(_hidersCardPrefab, transform.position, Quaternion.identity, _hidersGrid).GetComponent<PlayerCardUIManager>();
                newCard.InitializeCard(remoteP);
                _hiderCards.Add(newCard);
            }
            else if (remoteP._type == 1)
            {
                PlayerCardUIManager _remotePlayerCard = GetRemotePlayerCard(remoteP);

                if (_remotePlayerCard)
                {
                    _hiderCards.Remove(_remotePlayerCard);
                    Destroy(_remotePlayerCard.gameObject);
                }
            }
        }
    }

    public void UpdateGameStatusText()
    {
        if (_gameManager._remotePlayers.Count == 0)
        {
            _gameStatusText.text = "Waiting for Players to Join...";
            return;
        }

        if (_gameManager._gameManagerSync._gameState == 0)
        {
            SetActiveMenu(0);
            if(IsSomeoneSeeker())
            {
                _gameStatusText.text = "Waiting to start";
                return;
            }
            _gameStatusText.text = "Someone needs to be seeker";
        }
        if (_gameManager._gameManagerSync._gameState == 1)
        {
            SetActiveMenu(2);
            _gameStatusText.text = "Game is In Progress";
        }
        if (_gameManager._gameManagerSync._gameState == 2)
        {
            SetActiveMenu(1);
            _gameStatusText.text = "Go Hide!";
            
            // Janky way to prevent double corroutine counting down UI too quickly
            if(hidingTimer == 0)
            {
                hidingTimer = 20;
                StartCoroutine(HidingSequenceTimer());
            }
        }
        if (_gameManager._gameManagerSync._gameState == 3)
        {
            _gameStatusText.text = "Game Over!";
        }
    }

    public void SetActiveMenu(int menuIndex)
    {
        if (menuIndex == 0)
        {
            _mainMenu.SetActive(true);
            _seekerDelayMenu.SetActive(false);
            _inGameMenu.SetActive(false);
        }
        if (menuIndex == 1)
        {
            // If you are seeker show delay screen, if not, then show in game screen
            if (_gameManager._localPlayer._type == 1)
            {
                _mainMenu.SetActive(false);
                _seekerDelayMenu.SetActive(true);
                _inGameMenu.SetActive(false);
            }
            else
            {
                _mainMenu.SetActive(false);
                _seekerDelayMenu.SetActive(false);
                _inGameMenu.SetActive(true);
            }
        }
        if (menuIndex == 2)
        {
            _mainMenu.SetActive(false);
            _seekerDelayMenu.SetActive(false);
            _inGameMenu.SetActive(true);
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

    void AddHiderCard()
    {
        
    }

    public void ChooseHider()
    {
        if (_gameManager._localPlayer._type == 1)
        {
            _gameManager._localPlayer.SetPlayerType(0);
            
            PlayerCardUIManager newCard = Instantiate(_hidersCardPrefab, transform.position, Quaternion.identity, _hidersGrid).GetComponent<PlayerCardUIManager>();
            newCard.InitializeCard(_gameManager._localPlayer);
            _hiderCards.Add(newCard);
            _activeHiderCard = newCard;

            _seekerCard.ClearCardData();

            UpdateTextObjects();
        }
    }

    public void ChooseSeeker()
    {
        if (IsSomeoneSeeker())
        {
            _gameManager._gameManagerSync.SetLobbyStatus("Seeker Already Set");
            _becomeSeekerButton.interactable = false;
            return;
        }

        if (_activeHiderCard != null)
        {
            GameObject _hiderCardToRemove = _activeHiderCard.gameObject;
            _hiderCards.Remove(_activeHiderCard);

            Destroy(_hiderCardToRemove);
        }

        _gameManager._localPlayer.SetPlayerType(1);

        _seekerCard.InitializeCard(_gameManager._localPlayer);
        UpdateTextObjects();
    }

    private bool IsSomeoneSeeker()
    {
        bool someoneElseIsSeeker = false;
        foreach (var remoteP in _gameManager._remotePlayers)
        {
            if (remoteP._type == 1)
            {
                someoneElseIsSeeker = true;
                _seekerCard.InitializeCard(remoteP);
            }
        }

        return someoneElseIsSeeker;
    }

    private void UpdateTextObjects()
    {
        // if (_gameManager._localPlayer._type == 0)
        // {
        //     _typeText.text = "Hider";
        // }
        // else if (_gameManager._localPlayer._type == 1)
        // {
        //     _typeText.text = "Seeker";
        // }
        // else
        // {
        //     _typeText.text = "Not Selected";
        // }

        // foreach (var remoteP in _gameManager._remotePlayers)
        // {
        //     if (remoteP._type == 0)
        //     {
        //         _remotePlayerTypeText.text = "Hider";
        //     }
        //     else if (remoteP._type == 1)
        //     {
        //         _remotePlayerTypeText.text = "Seeker";
        //     }
        //     else
        //     {
        //         _remotePlayerTypeText.text = "Not Selected";
        //     }
        // }
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
