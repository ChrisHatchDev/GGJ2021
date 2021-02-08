using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameManagerSync _gameManagerSync;

    public Realtime _realtime;

    public string _newRoomCode = "";

    public string _connectedRoomCode = "";

    [HideInInspector]
    public SoyBoySync _localPlayer;

    [HideInInspector]
    public List<SoyBoySync> _remotePlayers = new List<SoyBoySync>();

    public UnityEvent _onConnectToRoom;

    public UnityEvent _onDisconnectFromRoom;

    public void InitializePlayerObject(SoyBoySync player, bool isRemote)
    {
        if (isRemote)
        {
            _remotePlayers.Add(player);
        }
        else
        {
            this._localPlayer = player;
        }
    }

    void Start()
    {
        _realtime.didConnectToRoom += DidConnectToRoom;
        _realtime.didDisconnectFromRoom += DidDisconnectFromRoom;
    }

    void Update()
    {
        if (_gameManagerSync._gameState == 2 && _remotePlayers.Count > 0)
        {
            bool atLeastOnePlayerIsAlive = false;
            foreach (var remoteP in _remotePlayers)
            {
                if (remoteP._isTagged == false)
                {
                    atLeastOnePlayerIsAlive = true;
                }
            }

            if (atLeastOnePlayerIsAlive == false)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over, All People Have Been Caught");
        _gameManagerSync.SetGameState(3);
    }

    public bool CanStartGame()
    {
        return _remotePlayers.Count > 0;
    }

    public bool IsRoomCodeSet()
    {
        return (_newRoomCode != "");
    }

    public void JoinGame (string roomName)
    {
        roomName = roomName.ToUpper();

        Debug.Log("Join game called with room name: " + roomName);
        _newRoomCode = roomName;
        _realtime.Connect(roomName);
    }

    public void DisconnectFromRoom()
    {
        _realtime.Disconnect();
    }

    private void DidDisconnectFromRoom(Realtime realtime)
    {
        _newRoomCode = "";
        _connectedRoomCode = "";

        _onDisconnectFromRoom.Invoke();
        Debug.Log("Did disconnect from the host server");
    }

    public void HostGame()
    {
        string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
        char[] randomRoomCode = new char[4];

        for (int i = 0; i < 4; i++)
        {
            randomRoomCode[i] = _letters[Random.Range(0, _letters.Length)];
        }

        this._newRoomCode = new string(randomRoomCode);

        // Debug.Log("Random room name: " + new string(randomRoomCode));
        _realtime.Connect(_newRoomCode);
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        _connectedRoomCode = _newRoomCode;

        _gameManagerSync.SetGameState(1);

        _onConnectToRoom.Invoke();
        Debug.Log("Did connect to host server");
    }

    public void StartGame ()
    {
        if (_remotePlayers.Count == 0)
        {
            Debug.Log("Not enough players");

            _gameManagerSync.SetLobbyStatus("Not enough players to start!");
            return;
        }

        bool isRemoteSeeker = false;
        foreach (var _remoteP in _remotePlayers)
        {
            if (_localPlayer._type == -1 || _remoteP._type == -1)
            {
                _gameManagerSync.SetLobbyStatus("All Players Must Select a Side");
                return;
            }

            if (_remoteP._type == 1)
            {
                isRemoteSeeker = true;
            }
        }

        if (_localPlayer._type == 0 && isRemoteSeeker == false)
        {
            Debug.Log("There must be at least one seeker");

            _gameManagerSync.SetLobbyStatus("There needs to be at least one seeker!");
            return;
        }


        if (_localPlayer._type == 1 && isRemoteSeeker)
        {
            Debug.Log("There Can Only Be One Seeker");

            _gameManagerSync.SetLobbyStatus("There can only be one seeker!");
            return;
        }

        StartCoroutine(HidingSequence());

        Debug.Log("Made it to start game, hiding mode started");
    }

    IEnumerator HidingSequence()
    {
        Debug.Log("Start Hiding");

        _gameManagerSync.SetGameState(3);
        yield return new WaitForSeconds(20f);
        
        Debug.Log("Hiding has ended");
        _gameManagerSync.SetGameState(2);
    }

    public void RestartGame()
    {
        // Clear Previously Tagged State for Rematch
        _localPlayer.SetTaggedState(false);
        foreach (var remoteP in _remotePlayers)
        {
            remoteP.SetTaggedState(false);
        }

        _gameManagerSync.SetLobbyStatus("REMATCH!");
        _gameManagerSync.SetGameState(1);
    }
}
