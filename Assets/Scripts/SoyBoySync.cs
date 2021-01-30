using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class SoyBoySync : RealtimeComponent<PlayerDataModel>
{
    // 0 = Hider 1 = Seeker
    public int _type = -1;

    public bool _isTagged = false;

    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;

    [SerializeField]
    private Material _hiderMaterial;

    [SerializeField]
    private Material _seekerMaterial;

    public UnityEvent onTypeChange;
    public UnityEvent onTagged;

    private void Awake()
    {
    }
    
    private void PlayerTypeDidChange(PlayerDataModel model, int type)
    {
        UpdateType(type);
    }

    private void PlayerIsTaggedDidChange(PlayerDataModel model, bool isTagged)
    {
        UpdateTaggedState(isTagged);
    }

    private void UpdateType(int type)
    {
        _type = type;
        _meshRenderer.material = _type == 0 ? _hiderMaterial : _seekerMaterial;
        onTypeChange.Invoke();
    }

    private void UpdateTaggedState(bool isTagged)
    {
        _isTagged = isTagged;
        onTagged.Invoke();
    }

    public void SetTaggedState (bool isTagged)
    {
        model.isTagged = isTagged;
    }

    public void SetPlayerType (int type)
    {
        model.playerType = type;
    }

    protected override void OnRealtimeModelReplaced(PlayerDataModel previousModel, PlayerDataModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.playerTypeDidChange -= PlayerTypeDidChange;
            currentModel.isTaggedDidChange -= PlayerIsTaggedDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.playerType = -1;
                currentModel.isTagged = false;
            }
        
            // Update the mesh render to match the new model
            UpdateType(currentModel.playerType);
            UpdateTaggedState(currentModel.isTagged);

            // Register for events so we'll know if the color changes later
            currentModel.playerTypeDidChange += PlayerTypeDidChange;
            currentModel.isTaggedDidChange += PlayerIsTaggedDidChange;
        }
    }

    public void TagPlayer (SoyBoySync _taggingPlayer)
    {
        if(_taggingPlayer._type == 1)
        {
            SetTaggedState(true);
            onTagged.Invoke();
        }
    }
}
