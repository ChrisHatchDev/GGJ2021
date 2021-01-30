using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public SoyBoySync _localPlayer;

    [HideInInspector]
    public SoyBoySync _remotePlayer;

    [SerializeField]
    public Text _typeText;

    [SerializeField]
    public Text _remotePlayerTypeText;

    public void InitializeEvents(SoyBoySync player, bool isRemote)
    {
        if (isRemote)
        {
            this._remotePlayer = player;
            this._remotePlayer.onTypeChange.AddListener(UpdateTextObjects);
        }
        else
        {
            this._localPlayer = player;
        }
    }

    public void InitializeEvents(SoyBoySync player)
    {
        this._localPlayer = player;
    }

    public void ChooseHider()
    {
        _localPlayer.SetPlayerType(0);
        UpdateTextObjects();
    }

    public void ChooseSeeker()
    {
        _localPlayer.SetPlayerType(1);
        UpdateTextObjects();
    }

    private void UpdateTextObjects()
    {
        _typeText.text = _localPlayer._type.ToString();

        if (_remotePlayer)
        {
            _remotePlayerTypeText.text = _remotePlayer._type.ToString();
        }
    }

}
