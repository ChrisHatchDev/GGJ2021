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

    private void Start()
    {
    }

    public void InitializeUpdateEvents(SoyBoySync player)
    {
        player.onTypeChange.AddListener(UpdateTextObjects);
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
        _typeText.text = _gameManager._localPlayer._type.ToString();

        foreach (var remoteP in _gameManager._remotePlayers)
        {
            _remotePlayerTypeText.text = remoteP._type.ToString();
        }
    }

}
