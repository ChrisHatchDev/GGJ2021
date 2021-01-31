using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class SoyBoySync : RealtimeComponent<PlayerDataModel>
{
    // 0 = Hider 1 = Seeker
    public int _type = -1;

    // 0 = Standing 1 = Crouched 2 = Proned
    public int _stance = 0;

    // 0 = Standing Still, 1 = Walking
    public int _walkingState = 0;

    public bool _isTagged = false;

    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;

    [SerializeField]
    private Material _hiderMaterial;

    [SerializeField]
    private Material _seekerMaterial;

    public UnityEvent onTypeChange;
    public UnityEvent onTagged;
    public UnityEvent onStanceChanged;
    public UnityEvent onWalkingStateChanged;

    private void Awake()
    {
    }
    
    private void WalkingStateDidChange(PlayerDataModel model, int walkingState)
    {
        UpdateWalkingState(walkingState);
    }

    private void UpdateWalkingState(int walkingState)
    {
        _walkingState = walkingState;
        onWalkingStateChanged.Invoke();
    }

    public void SetWalkingStateState (int walkingState)
    {
        model.walkingState = walkingState;
    }



    private void PlayerStanceDidChange(PlayerDataModel model, int stance)
    {
        UpdateStance(stance);
    }

    private void UpdateStance(int stance)
    {
        _stance = stance;
        onStanceChanged.Invoke();
    }

    public void SetPlayerStance (int stance)
    {
        model.stanceState = stance;
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

    private void PlayerIsTaggedDidChange(PlayerDataModel model, bool isTagged)
    {
        UpdateTaggedState(isTagged);
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

    protected override void OnRealtimeModelReplaced(PlayerDataModel previousModel, PlayerDataModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.playerTypeDidChange -= PlayerTypeDidChange;
            previousModel.isTaggedDidChange -= PlayerIsTaggedDidChange;
            previousModel.stanceStateDidChange -= PlayerStanceDidChange;
            previousModel.walkingStateDidChange -= WalkingStateDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.playerType = -1;
                currentModel.isTagged = false;
                currentModel.stanceState = 0;
                currentModel.walkingState = 0;
            }
        
            // Update the mesh render to match the new model
            UpdateType(currentModel.playerType);
            UpdateTaggedState(currentModel.isTagged);
            UpdateStance(currentModel.stanceState);
            UpdateWalkingState(currentModel.walkingState);

            // Register for events so we'll know if the color changes later
            currentModel.playerTypeDidChange += PlayerTypeDidChange;
            currentModel.isTaggedDidChange += PlayerIsTaggedDidChange;
            currentModel.stanceStateDidChange += PlayerStanceDidChange;
            currentModel.walkingStateDidChange += WalkingStateDidChange;
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
