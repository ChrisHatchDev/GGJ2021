using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class SoyBoySync : RealtimeComponent<PlayerDataModel>
{
    public int _type = -1;

    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;

    [SerializeField]
    private Material _hiderMaterial;

    [SerializeField]
    private Material _seekerMaterial;

    public UnityEvent onTypeChange;

    private void Awake()
    {
    }
    
    private void PlayerTypeDidChange(PlayerDataModel model, int type)
    {
        UpdateType(type);
    }

    private void UpdateType(int type)
    {
        _type = type;
        _meshRenderer.material = _type == 0 ? _hiderMaterial : _seekerMaterial;
        onTypeChange.Invoke();
    }

    public void SetPlayerType (int type)
    {
        model.playerType = type;
    }

    protected override void OnRealtimeModelReplaced(PlayerDataModel previousModel, PlayerDataModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.playerTypeDidChange -= PlayerTypeDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.playerType = -1;
        
            // Update the mesh render to match the new model
            UpdateType(currentModel.playerType);

            // Register for events so we'll know if the color changes later
            currentModel.playerTypeDidChange += PlayerTypeDidChange;
        }
    }
}
