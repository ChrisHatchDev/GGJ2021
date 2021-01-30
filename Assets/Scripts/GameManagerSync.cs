using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;
using UnityEngine.UI;

public class GameManagerSync : RealtimeComponent<GameDataModel>
{
    public int _type = -1;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Text _gameWinnerText;

    public UnityEvent onTypeChange;

    private void Awake()
    {
    }
    
    private void PlayerTypeDidChange(GameDataModel model, int type)
    {
        UpdateType(type);
    }

    private void UpdateType(int type)
    {
        _type = type;
        onTypeChange.Invoke();
    }

    public void SetPlayerType (int type)
    {
        model.numberOfRounds = type;
    }

    protected override void OnRealtimeModelReplaced(GameDataModel previousModel, GameDataModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.numberOfRoundsDidChange -= PlayerTypeDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.numberOfRounds = -1;
        
            // Update the mesh render to match the new model
            UpdateType(currentModel.numberOfRounds);

            // Register for events so we'll know if the color changes later
            currentModel.numberOfRoundsDidChange += PlayerTypeDidChange;
        }
    }
}
