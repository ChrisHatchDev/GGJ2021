using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameManagerSync _gameManagerSync;

    [HideInInspector]
    public SoyBoySync _localPlayer;

    [HideInInspector]
    public List<SoyBoySync> _remotePlayers = new List<SoyBoySync>();

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
        
    }

    void Update()
    {
        if (_gameManagerSync._gameState == 1 && _remotePlayers.Count > 0)
        {
            bool atLeastOnePlayerIsAlive = false;
            foreach (var remoteP in _remotePlayers)
            {
                if (remoteP._isTagged == false && remoteP._type == 0)
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

        _gameManagerSync.SetGameState(2);
        yield return new WaitForSeconds(20f);
        
        Debug.Log("Hiding has ended");
        _gameManagerSync.SetGameState(1);
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
        _gameManagerSync.SetGameState(0);
    }
}
