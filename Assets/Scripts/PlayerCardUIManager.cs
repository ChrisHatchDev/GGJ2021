using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUIManager : MonoBehaviour
{
    [SerializeField]
    public Text _playerNameText;

    [SerializeField]
    public SoyBoySync _assignedPlayer;

    public void InitializeCard(SoyBoySync playerData)
    {
        _assignedPlayer = playerData;
        _playerNameText.text = playerData._playerName;
    }

    public void ClearCardData()
    {
        _assignedPlayer = null;
        _playerNameText.text = "No Seeker";
    }
}
