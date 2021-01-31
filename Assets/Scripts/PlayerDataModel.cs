using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class PlayerDataModel
{
    [RealtimeProperty(1, true, true)]
    private int _playerType;

    [RealtimeProperty(2, true, true)]
    private bool _isTagged;

    [RealtimeProperty(3, true, true)]
    private int _stanceState;

    [RealtimeProperty(4, true, true)]
    private int _walkingState;

    [RealtimeProperty(5, true, true)]
    private string _playerName;

    [RealtimeProperty(6, true, true)]
    private int _knifeState;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class PlayerDataModel : RealtimeModel {
    public int playerType {
        get {
            return _cache.LookForValueInCache(_playerType, entry => entry.playerTypeSet, entry => entry.playerType);
        }
        set {
            if (this.playerType == value) return;
            _cache.UpdateLocalCache(entry => { entry.playerTypeSet = true; entry.playerType = value; return entry; });
            InvalidateReliableLength();
            FirePlayerTypeDidChange(value);
        }
    }
    
    public bool isTagged {
        get {
            return _cache.LookForValueInCache(_isTagged, entry => entry.isTaggedSet, entry => entry.isTagged);
        }
        set {
            if (this.isTagged == value) return;
            _cache.UpdateLocalCache(entry => { entry.isTaggedSet = true; entry.isTagged = value; return entry; });
            InvalidateReliableLength();
            FireIsTaggedDidChange(value);
        }
    }
    
    public int stanceState {
        get {
            return _cache.LookForValueInCache(_stanceState, entry => entry.stanceStateSet, entry => entry.stanceState);
        }
        set {
            if (this.stanceState == value) return;
            _cache.UpdateLocalCache(entry => { entry.stanceStateSet = true; entry.stanceState = value; return entry; });
            InvalidateReliableLength();
            FireStanceStateDidChange(value);
        }
    }
    
    public int walkingState {
        get {
            return _cache.LookForValueInCache(_walkingState, entry => entry.walkingStateSet, entry => entry.walkingState);
        }
        set {
            if (this.walkingState == value) return;
            _cache.UpdateLocalCache(entry => { entry.walkingStateSet = true; entry.walkingState = value; return entry; });
            InvalidateReliableLength();
            FireWalkingStateDidChange(value);
        }
    }
    
    public string playerName {
        get {
            return _cache.LookForValueInCache(_playerName, entry => entry.playerNameSet, entry => entry.playerName);
        }
        set {
            if (this.playerName == value) return;
            _cache.UpdateLocalCache(entry => { entry.playerNameSet = true; entry.playerName = value; return entry; });
            InvalidateReliableLength();
            FirePlayerNameDidChange(value);
        }
    }
    
    public int knifeState {
        get {
            return _cache.LookForValueInCache(_knifeState, entry => entry.knifeStateSet, entry => entry.knifeState);
        }
        set {
            if (this.knifeState == value) return;
            _cache.UpdateLocalCache(entry => { entry.knifeStateSet = true; entry.knifeState = value; return entry; });
            InvalidateReliableLength();
            FireKnifeStateDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(PlayerDataModel model, T value);
    public event PropertyChangedHandler<int> playerTypeDidChange;
    public event PropertyChangedHandler<bool> isTaggedDidChange;
    public event PropertyChangedHandler<int> stanceStateDidChange;
    public event PropertyChangedHandler<int> walkingStateDidChange;
    public event PropertyChangedHandler<string> playerNameDidChange;
    public event PropertyChangedHandler<int> knifeStateDidChange;
    
    private struct LocalCacheEntry {
        public bool playerTypeSet;
        public int playerType;
        public bool isTaggedSet;
        public bool isTagged;
        public bool stanceStateSet;
        public int stanceState;
        public bool walkingStateSet;
        public int walkingState;
        public bool playerNameSet;
        public string playerName;
        public bool knifeStateSet;
        public int knifeState;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        PlayerType = 1,
        IsTagged = 2,
        StanceState = 3,
        WalkingState = 4,
        PlayerName = 5,
        KnifeState = 6,
    }
    
    public PlayerDataModel() : this(null) {
    }
    
    public PlayerDataModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FirePlayerTypeDidChange(int value) {
        try {
            playerTypeDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireIsTaggedDidChange(bool value) {
        try {
            isTaggedDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireStanceStateDidChange(int value) {
        try {
            stanceStateDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireWalkingStateDidChange(int value) {
        try {
            walkingStateDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FirePlayerNameDidChange(string value) {
        try {
            playerNameDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireKnifeStateDidChange(int value) {
        try {
            knifeStateDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteVarint32Length((uint)PropertyID.PlayerType, (uint)_playerType);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.IsTagged, _isTagged ? 1u : 0u);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.StanceState, (uint)_stanceState);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.WalkingState, (uint)_walkingState);
            length += WriteStream.WriteStringLength((uint)PropertyID.PlayerName, _playerName);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.KnifeState, (uint)_knifeState);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.playerTypeSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.PlayerType, (uint)entry.playerType);
            }
            if (entry.isTaggedSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.IsTagged, entry.isTagged ? 1u : 0u);
            }
            if (entry.stanceStateSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.StanceState, (uint)entry.stanceState);
            }
            if (entry.walkingStateSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.WalkingState, (uint)entry.walkingState);
            }
            if (entry.playerNameSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.PlayerName, entry.playerName);
            }
            if (entry.knifeStateSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.KnifeState, (uint)entry.knifeState);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteVarint32((uint)PropertyID.PlayerType, (uint)_playerType);
            stream.WriteVarint32((uint)PropertyID.IsTagged, _isTagged ? 1u : 0u);
            stream.WriteVarint32((uint)PropertyID.StanceState, (uint)_stanceState);
            stream.WriteVarint32((uint)PropertyID.WalkingState, (uint)_walkingState);
            stream.WriteString((uint)PropertyID.PlayerName, _playerName);
            stream.WriteVarint32((uint)PropertyID.KnifeState, (uint)_knifeState);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.playerTypeSet || entry.isTaggedSet || entry.stanceStateSet || entry.walkingStateSet || entry.playerNameSet || entry.knifeStateSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.playerTypeSet) {
                stream.WriteVarint32((uint)PropertyID.PlayerType, (uint)entry.playerType);
                didWriteProperties = true;
            }
            if (entry.isTaggedSet) {
                stream.WriteVarint32((uint)PropertyID.IsTagged, entry.isTagged ? 1u : 0u);
                didWriteProperties = true;
            }
            if (entry.stanceStateSet) {
                stream.WriteVarint32((uint)PropertyID.StanceState, (uint)entry.stanceState);
                didWriteProperties = true;
            }
            if (entry.walkingStateSet) {
                stream.WriteVarint32((uint)PropertyID.WalkingState, (uint)entry.walkingState);
                didWriteProperties = true;
            }
            if (entry.playerNameSet) {
                stream.WriteString((uint)PropertyID.PlayerName, entry.playerName);
                didWriteProperties = true;
            }
            if (entry.knifeStateSet) {
                stream.WriteVarint32((uint)PropertyID.KnifeState, (uint)entry.knifeState);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.PlayerType: {
                    int previousValue = _playerType;
                    _playerType = (int)stream.ReadVarint32();
                    bool playerTypeExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.playerTypeSet);
                    if (!playerTypeExistsInChangeCache && _playerType != previousValue) {
                        FirePlayerTypeDidChange(_playerType);
                    }
                    break;
                }
                case (uint)PropertyID.IsTagged: {
                    bool previousValue = _isTagged;
                    _isTagged = (stream.ReadVarint32() != 0);
                    bool isTaggedExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.isTaggedSet);
                    if (!isTaggedExistsInChangeCache && _isTagged != previousValue) {
                        FireIsTaggedDidChange(_isTagged);
                    }
                    break;
                }
                case (uint)PropertyID.StanceState: {
                    int previousValue = _stanceState;
                    _stanceState = (int)stream.ReadVarint32();
                    bool stanceStateExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.stanceStateSet);
                    if (!stanceStateExistsInChangeCache && _stanceState != previousValue) {
                        FireStanceStateDidChange(_stanceState);
                    }
                    break;
                }
                case (uint)PropertyID.WalkingState: {
                    int previousValue = _walkingState;
                    _walkingState = (int)stream.ReadVarint32();
                    bool walkingStateExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.walkingStateSet);
                    if (!walkingStateExistsInChangeCache && _walkingState != previousValue) {
                        FireWalkingStateDidChange(_walkingState);
                    }
                    break;
                }
                case (uint)PropertyID.PlayerName: {
                    string previousValue = _playerName;
                    _playerName = stream.ReadString();
                    bool playerNameExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.playerNameSet);
                    if (!playerNameExistsInChangeCache && _playerName != previousValue) {
                        FirePlayerNameDidChange(_playerName);
                    }
                    break;
                }
                case (uint)PropertyID.KnifeState: {
                    int previousValue = _knifeState;
                    _knifeState = (int)stream.ReadVarint32();
                    bool knifeStateExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.knifeStateSet);
                    if (!knifeStateExistsInChangeCache && _knifeState != previousValue) {
                        FireKnifeStateDidChange(_knifeState);
                    }
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
        }
    }
    
    #region Cache Operations
    
    private StreamEventDispatcher _streamEventDispatcher;
    
    private void FlattenCache() {
        _playerType = playerType;
        _isTagged = isTagged;
        _stanceState = stanceState;
        _walkingState = walkingState;
        _playerName = playerName;
        _knifeState = knifeState;
        _cache.Clear();
    }
    
    private void ClearCache(uint updateID) {
        _cache.RemoveUpdateFromInflight(updateID);
    }
    
    private void ClearCacheOnStreamCallback(StreamContext context) {
        if (_streamEventDispatcher != context.dispatcher) {
            UnsubscribeClearCacheCallback(); // unsub from previous dispatcher
        }
        _streamEventDispatcher = context.dispatcher;
        _streamEventDispatcher.AddStreamCallback(context.updateID, ClearCache);
    }
    
    private void UnsubscribeClearCacheCallback() {
        if (_streamEventDispatcher != null) {
            _streamEventDispatcher.RemoveStreamCallback(ClearCache);
            _streamEventDispatcher = null;
        }
    }
    
    #endregion
}
/* ----- End Normal Autogenerated Code ----- */
