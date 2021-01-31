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
    public GameObject _gameOverScreen;

    [SerializeField]
    public GameObject _youGotCaughtText;

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
    public Text _seekerTimerText;

    [SerializeField]
    public Text _hiderTimerText;

    [SerializeField]
    public Text _hiderTimerSubtitle;

    [SerializeField]
    public Text _objectiveText;

    [SerializeField]
    public Transform _playersLeftGrid;

    [SerializeField]
    public GameObject _playersLeftUIPrefab;

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

    public void InitializeLocalPlayerMenuItems(SoyBoySync player)
    {
        player.SetPlayerName(player.playerNames[Random.Range(0, player.playerNames.Count)]);

        // Spawn as Hider
        if (IsSomeoneSeeker())
        {
            player.SetPlayerType(0);

            PlayerCardUIManager newCard = Instantiate(_hidersCardPrefab, transform.position, Quaternion.identity, _hidersGrid).GetComponent<PlayerCardUIManager>();
            newCard.InitializeCard(player);
            _hiderCards.Add(newCard);
            _activeHiderCard = newCard;

            _yourNameText.text = "Your Name: " + player._playerName;
        }
        else
        {
            // Spawn as Seeker Filling that Spot
            player.SetPlayerType(1);
            _seekerCard.InitializeCard(player);

            _yourNameText.text = "Your Name: " + player._playerName;
        }
    }

    public void InitializeUpdateEvents(SoyBoySync player)
    {        
        _gameManager._gameManagerSync.onGameStateChange.AddListener(UpdateGameStatusText);
        _gameManager._gameManagerSync.onLobbyStatusChange.AddListener(LobbyStatusChanged);
        
        player.onTypeChange.AddListener(RefreshUIStates);

        player.onTagged.AddListener(() => {
            _youGotCaughtText.SetActive(true);
        });

        LobbyStatusChanged();
        UpdateGameStatusText();
        RefreshUIStates();
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

    void RefreshUIStates()
    {
        Debug.Log("Menu Manager On Type Changed Ran");
        
        // Refresh UI states based on remote data
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

        if (_gameManager._localPlayer == null)
        {
            Debug.Log("Local PLayer is null in refresh UI state");
            return;
        }

        // Refresh local UI states based on type
        if (_gameManager._localPlayer._type == 0)
        {
            _becomeHiderButton.interactable = false;
            _becomeSeekerButton.interactable = true;
            _objectiveText.text = "Hide from the Seeker!\n\nUse cabinets, vents, baths, and more to hide.\n\nUse 'C' to crouch, hold to lay down";
        }
        else if (_gameManager._localPlayer._type == 1)
        {
            _becomeHiderButton.interactable = true;
            _becomeSeekerButton.interactable = false;
            _objectiveText.text = "Find the hiders!\n\nThey can be in cabinets, vents, baths, and more!\n\nUse 'Left Click' to attack/capture a hider";
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
            SetActiveMenu(3);
        }
    }

    public void SetActiveMenu(int menuIndex)
    {
        if (menuIndex == 0)
        {
            _mainMenu.SetActive(true);
            _seekerDelayMenu.SetActive(false);
            _inGameMenu.SetActive(false);
            _gameOverScreen.SetActive(false);
        }
        if (menuIndex == 1)
        {
            // If you are seeker show delay screen, if not, then show in game screen
            if (_gameManager._localPlayer._type == 1)
            {
                _mainMenu.SetActive(false);
                _seekerDelayMenu.SetActive(true);
                _inGameMenu.SetActive(false);
                _gameOverScreen.SetActive(false);
            }
            else
            {
                _mainMenu.SetActive(false);
                _seekerDelayMenu.SetActive(false);
                _inGameMenu.SetActive(true);
                _gameOverScreen.SetActive(false);
            }
        }
        if (menuIndex == 2)
        {
            _mainMenu.SetActive(false);
            _seekerDelayMenu.SetActive(false);
            _inGameMenu.SetActive(true);
            _gameOverScreen.SetActive(false);
        }
        if (menuIndex == 3)
        {
            _mainMenu.SetActive(false);
            _seekerDelayMenu.SetActive(false);
            _inGameMenu.SetActive(false);
            _gameOverScreen.SetActive(true);
        }
    }

    IEnumerator HidingSequenceTimer()
    {
        hidingTimer -= 1;

        if (hidingTimer <= 0)
        {
            _seekerTimerText.text = "Seek!";
            _hiderTimerText.text = "Shhh!!!";

            yield return new WaitForSeconds(0.8f);
            _hiderTimerText.enabled = false;
            _hiderTimerSubtitle.enabled = false;
        }
        else
        {
            _seekerTimerText.text = hidingTimer.ToString();
            
            _hiderTimerText.text = hidingTimer.ToString();
            _hiderTimerText.enabled = true;
            _hiderTimerSubtitle.enabled = true;
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

            RefreshUIStates();
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
        RefreshUIStates();
    }

    private bool IsSomeoneSeeker()
    {
        bool someoneElseIsSeeker = false;
        foreach (var remoteP in _gameManager._remotePlayers)
        {
            if (remoteP._type == 1)
            {
                someoneElseIsSeeker = true;
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
