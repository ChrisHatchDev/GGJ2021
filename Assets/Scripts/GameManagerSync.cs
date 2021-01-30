using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;
using UnityEngine.UI;

public class GameManagerSync : RealtimeComponent<GameDataModel>
{
    // 0 = "In Lobby", 1 = "In Game", 2 = "Hiding Time!",  3 = "Game Over"
    public int _gameState = 0;

    public string _lobbyStatus = "";

    [SerializeField]
    private string _gameWinnerName;

    public UnityEvent onGameStateChange;
    public UnityEvent onGameWinnerChange;
    public UnityEvent onLobbyStatusChange;

    private void Awake()
    {
    }

    private void LobbyStatusDidChange(GameDataModel model, string lobbyStatus)
    {
        UpdateLobbyStatus(lobbyStatus);
    }

    private void UpdateLobbyStatus(string lobbyStatus)
    {
        _lobbyStatus = lobbyStatus;
        onLobbyStatusChange.Invoke();
    }

    public void SetLobbyStatus (string lobbyStatus)
    {
        model.lobbyStatus = lobbyStatus;
    }
    
    private void GameStateDidChange(GameDataModel model, int gameState)
    {
        UpdateGameState(gameState);
    }

    private void UpdateGameState(int gameState)
    {
        _gameState = gameState;
        onGameStateChange.Invoke();
    }

    public void SetGameState (int gameState)
    {
        model.gameState = gameState;
    }


    private void WinnerNameDidChange(GameDataModel model, string playerName)
    {
        UpdateWinnerName(playerName);
    }

    private void UpdateWinnerName(string playerName)
    {
        _gameWinnerName = playerName;
        onGameWinnerChange.Invoke();
    }

    public void SetGameWinner(string playerName)
    {
        model.gameWinnerName = playerName;
    }

    protected override void OnRealtimeModelReplaced(GameDataModel previousModel, GameDataModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.gameStateDidChange -= GameStateDidChange;
            previousModel.gameWinnerNameDidChange -= WinnerNameDidChange;
            previousModel.lobbyStatusDidChange -= LobbyStatusDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.gameState = 0;
                currentModel.gameWinnerName = "";
                currentModel.lobbyStatus = "";
            }
        
            // Update the mesh render to match the new model
            UpdateGameState(currentModel.gameState);
            UpdateWinnerName(currentModel.gameWinnerName);
            UpdateLobbyStatus(currentModel.lobbyStatus);

            // Register for events so we'll know if the color changes later
            currentModel.gameStateDidChange += GameStateDidChange;
            currentModel.gameWinnerNameDidChange += WinnerNameDidChange;
            currentModel.lobbyStatusDidChange += LobbyStatusDidChange;
        }
    }
}
