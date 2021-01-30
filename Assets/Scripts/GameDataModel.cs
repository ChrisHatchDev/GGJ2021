using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class GameDataModel
{
    [RealtimeProperty(1, true, true)]
    private int _activeRound;

    [RealtimeProperty(2, true, true)]
    private int _numberOfRounds;

    [RealtimeProperty(3, true, true)]
    private string _seekerName;
    
    [RealtimeProperty(4, true, true)]
    private string _hiderName;

    [RealtimeProperty(5, true, true)]
    private bool _gameInProgress;
    
    [RealtimeProperty(6, true, true)]
    private string _gameWinnerText;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class GameDataModel : RealtimeModel {
    public int activeRound {
        get {
            return _cache.LookForValueInCache(_activeRound, entry => entry.activeRoundSet, entry => entry.activeRound);
        }
        set {
            if (this.activeRound == value) return;
            _cache.UpdateLocalCache(entry => { entry.activeRoundSet = true; entry.activeRound = value; return entry; });
            InvalidateReliableLength();
            FireActiveRoundDidChange(value);
        }
    }
    
    public int numberOfRounds {
        get {
            return _cache.LookForValueInCache(_numberOfRounds, entry => entry.numberOfRoundsSet, entry => entry.numberOfRounds);
        }
        set {
            if (this.numberOfRounds == value) return;
            _cache.UpdateLocalCache(entry => { entry.numberOfRoundsSet = true; entry.numberOfRounds = value; return entry; });
            InvalidateReliableLength();
            FireNumberOfRoundsDidChange(value);
        }
    }
    
    public string seekerName {
        get {
            return _cache.LookForValueInCache(_seekerName, entry => entry.seekerNameSet, entry => entry.seekerName);
        }
        set {
            if (this.seekerName == value) return;
            _cache.UpdateLocalCache(entry => { entry.seekerNameSet = true; entry.seekerName = value; return entry; });
            InvalidateReliableLength();
            FireSeekerNameDidChange(value);
        }
    }
    
    public string hiderName {
        get {
            return _cache.LookForValueInCache(_hiderName, entry => entry.hiderNameSet, entry => entry.hiderName);
        }
        set {
            if (this.hiderName == value) return;
            _cache.UpdateLocalCache(entry => { entry.hiderNameSet = true; entry.hiderName = value; return entry; });
            InvalidateReliableLength();
            FireHiderNameDidChange(value);
        }
    }
    
    public bool gameInProgress {
        get {
            return _cache.LookForValueInCache(_gameInProgress, entry => entry.gameInProgressSet, entry => entry.gameInProgress);
        }
        set {
            if (this.gameInProgress == value) return;
            _cache.UpdateLocalCache(entry => { entry.gameInProgressSet = true; entry.gameInProgress = value; return entry; });
            InvalidateReliableLength();
            FireGameInProgressDidChange(value);
        }
    }
    
    public string gameWinnerText {
        get {
            return _cache.LookForValueInCache(_gameWinnerText, entry => entry.gameWinnerTextSet, entry => entry.gameWinnerText);
        }
        set {
            if (this.gameWinnerText == value) return;
            _cache.UpdateLocalCache(entry => { entry.gameWinnerTextSet = true; entry.gameWinnerText = value; return entry; });
            InvalidateReliableLength();
            FireGameWinnerTextDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(GameDataModel model, T value);
    public event PropertyChangedHandler<int> activeRoundDidChange;
    public event PropertyChangedHandler<int> numberOfRoundsDidChange;
    public event PropertyChangedHandler<string> seekerNameDidChange;
    public event PropertyChangedHandler<string> hiderNameDidChange;
    public event PropertyChangedHandler<bool> gameInProgressDidChange;
    public event PropertyChangedHandler<string> gameWinnerTextDidChange;
    
    private struct LocalCacheEntry {
        public bool activeRoundSet;
        public int activeRound;
        public bool numberOfRoundsSet;
        public int numberOfRounds;
        public bool seekerNameSet;
        public string seekerName;
        public bool hiderNameSet;
        public string hiderName;
        public bool gameInProgressSet;
        public bool gameInProgress;
        public bool gameWinnerTextSet;
        public string gameWinnerText;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        ActiveRound = 1,
        NumberOfRounds = 2,
        SeekerName = 3,
        HiderName = 4,
        GameInProgress = 5,
        GameWinnerText = 6,
    }
    
    public GameDataModel() : this(null) {
    }
    
    public GameDataModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireActiveRoundDidChange(int value) {
        try {
            activeRoundDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireNumberOfRoundsDidChange(int value) {
        try {
            numberOfRoundsDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireSeekerNameDidChange(string value) {
        try {
            seekerNameDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireHiderNameDidChange(string value) {
        try {
            hiderNameDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireGameInProgressDidChange(bool value) {
        try {
            gameInProgressDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireGameWinnerTextDidChange(string value) {
        try {
            gameWinnerTextDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteVarint32Length((uint)PropertyID.ActiveRound, (uint)_activeRound);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.NumberOfRounds, (uint)_numberOfRounds);
            length += WriteStream.WriteStringLength((uint)PropertyID.SeekerName, _seekerName);
            length += WriteStream.WriteStringLength((uint)PropertyID.HiderName, _hiderName);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.GameInProgress, _gameInProgress ? 1u : 0u);
            length += WriteStream.WriteStringLength((uint)PropertyID.GameWinnerText, _gameWinnerText);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.activeRoundSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.ActiveRound, (uint)entry.activeRound);
            }
            if (entry.numberOfRoundsSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.NumberOfRounds, (uint)entry.numberOfRounds);
            }
            if (entry.seekerNameSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.SeekerName, entry.seekerName);
            }
            if (entry.hiderNameSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.HiderName, entry.hiderName);
            }
            if (entry.gameInProgressSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.GameInProgress, entry.gameInProgress ? 1u : 0u);
            }
            if (entry.gameWinnerTextSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.GameWinnerText, entry.gameWinnerText);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteVarint32((uint)PropertyID.ActiveRound, (uint)_activeRound);
            stream.WriteVarint32((uint)PropertyID.NumberOfRounds, (uint)_numberOfRounds);
            stream.WriteString((uint)PropertyID.SeekerName, _seekerName);
            stream.WriteString((uint)PropertyID.HiderName, _hiderName);
            stream.WriteVarint32((uint)PropertyID.GameInProgress, _gameInProgress ? 1u : 0u);
            stream.WriteString((uint)PropertyID.GameWinnerText, _gameWinnerText);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.activeRoundSet || entry.numberOfRoundsSet || entry.seekerNameSet || entry.hiderNameSet || entry.gameInProgressSet || entry.gameWinnerTextSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.activeRoundSet) {
                stream.WriteVarint32((uint)PropertyID.ActiveRound, (uint)entry.activeRound);
                didWriteProperties = true;
            }
            if (entry.numberOfRoundsSet) {
                stream.WriteVarint32((uint)PropertyID.NumberOfRounds, (uint)entry.numberOfRounds);
                didWriteProperties = true;
            }
            if (entry.seekerNameSet) {
                stream.WriteString((uint)PropertyID.SeekerName, entry.seekerName);
                didWriteProperties = true;
            }
            if (entry.hiderNameSet) {
                stream.WriteString((uint)PropertyID.HiderName, entry.hiderName);
                didWriteProperties = true;
            }
            if (entry.gameInProgressSet) {
                stream.WriteVarint32((uint)PropertyID.GameInProgress, entry.gameInProgress ? 1u : 0u);
                didWriteProperties = true;
            }
            if (entry.gameWinnerTextSet) {
                stream.WriteString((uint)PropertyID.GameWinnerText, entry.gameWinnerText);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.ActiveRound: {
                    int previousValue = _activeRound;
                    _activeRound = (int)stream.ReadVarint32();
                    bool activeRoundExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.activeRoundSet);
                    if (!activeRoundExistsInChangeCache && _activeRound != previousValue) {
                        FireActiveRoundDidChange(_activeRound);
                    }
                    break;
                }
                case (uint)PropertyID.NumberOfRounds: {
                    int previousValue = _numberOfRounds;
                    _numberOfRounds = (int)stream.ReadVarint32();
                    bool numberOfRoundsExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.numberOfRoundsSet);
                    if (!numberOfRoundsExistsInChangeCache && _numberOfRounds != previousValue) {
                        FireNumberOfRoundsDidChange(_numberOfRounds);
                    }
                    break;
                }
                case (uint)PropertyID.SeekerName: {
                    string previousValue = _seekerName;
                    _seekerName = stream.ReadString();
                    bool seekerNameExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.seekerNameSet);
                    if (!seekerNameExistsInChangeCache && _seekerName != previousValue) {
                        FireSeekerNameDidChange(_seekerName);
                    }
                    break;
                }
                case (uint)PropertyID.HiderName: {
                    string previousValue = _hiderName;
                    _hiderName = stream.ReadString();
                    bool hiderNameExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.hiderNameSet);
                    if (!hiderNameExistsInChangeCache && _hiderName != previousValue) {
                        FireHiderNameDidChange(_hiderName);
                    }
                    break;
                }
                case (uint)PropertyID.GameInProgress: {
                    bool previousValue = _gameInProgress;
                    _gameInProgress = (stream.ReadVarint32() != 0);
                    bool gameInProgressExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.gameInProgressSet);
                    if (!gameInProgressExistsInChangeCache && _gameInProgress != previousValue) {
                        FireGameInProgressDidChange(_gameInProgress);
                    }
                    break;
                }
                case (uint)PropertyID.GameWinnerText: {
                    string previousValue = _gameWinnerText;
                    _gameWinnerText = stream.ReadString();
                    bool gameWinnerTextExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.gameWinnerTextSet);
                    if (!gameWinnerTextExistsInChangeCache && _gameWinnerText != previousValue) {
                        FireGameWinnerTextDidChange(_gameWinnerText);
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
        _activeRound = activeRound;
        _numberOfRounds = numberOfRounds;
        _seekerName = seekerName;
        _hiderName = hiderName;
        _gameInProgress = gameInProgress;
        _gameWinnerText = gameWinnerText;
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
