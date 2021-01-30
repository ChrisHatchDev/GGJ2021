using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public SoyBoySync _localPlayer;

    [SerializeField]
    public Text _typeText;

    public void InitializeEvents(SoyBoySync player)
    {
        this._localPlayer = player;
    }

    public void ChooseHider()
    {
        _localPlayer.SetPlayerType(0);
        _typeText.text = _localPlayer._type.ToString();
    }

    public void ChooseSeeker()
    {
        _localPlayer.SetPlayerType(1);
        _typeText.text = _localPlayer._type.ToString();
    }

}
