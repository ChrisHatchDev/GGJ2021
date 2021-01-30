using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public SoyBoySync _localPlayer;

    [HideInInspector]
    public List<SoyBoySync> _remotePlayers = new List<SoyBoySync>();

    public UnityEvent OnNotEnoughPlayers;
    public UnityEvent OnAtLeastOneSeeker;
    public UnityEvent OnOnlyOneSeeker;
    public UnityEvent<SoyBoySync> OnLocalPlayerJoined;
    public UnityEvent<SoyBoySync> OnRemotePlayerJoined;

    // Use Soyboysync to 
    public UnityEvent<SoyBoySync> OnGameOver;

    public void InitializePlayerObject(SoyBoySync player, bool isRemote)
    {
        if (isRemote)
        {
            _remotePlayers.Add(player);
            OnRemotePlayerJoined.Invoke(player);
        }
        else
        {
            this._localPlayer = player;
            OnLocalPlayerJoined.Invoke(player);
        }
    }

    void Start()
    {
        
    }

    void Update()
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

    public void GameOver ()
    {
        Debug.Log("Game Over, All People Have Been Caught");
    }

    public void StartGame ()
    {
        if (_remotePlayers.Count == 0)
        {
            Debug.Log("Not enough players");
            OnNotEnoughPlayers.Invoke();
            return;
        }

        bool isRemoteSeeker = false;
        foreach (var _remoteP in _remotePlayers)
        {
            if (_remoteP._type == 1)
            {
                isRemoteSeeker = true;
            }
        }

        if (_localPlayer._type == 0 && isRemoteSeeker == false)
        {
            Debug.Log("There must be at least oen seeker");
            OnAtLeastOneSeeker.Invoke();
            return;
        }


        if (_localPlayer._type == 1 && isRemoteSeeker)
        {
            Debug.Log("There Can Only Be One Seeker");
            OnOnlyOneSeeker.Invoke();
            return;
        }

        Debug.Log("Made it to start game");
    }
}
