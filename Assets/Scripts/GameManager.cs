using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public SoyBoySync _localPlayer;

    [HideInInspector]
    public SoyBoySync _remotePlayer;

    public void InitializeEvents(SoyBoySync player, bool isRemote)
    {
        if (isRemote)
        {
            this._remotePlayer = player;
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
        
    }
}
