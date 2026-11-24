using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;

public sealed class LeaderboardPanelLogic
{
    public enum Tab { Time, Crowns, Stars }

    private readonly Dictionary<string, LBData> _leaderboardCache = new Dictionary<string, LBData>();
    private readonly Action<LBData> _onApply;
    private readonly int _topEntriesCount;
    private Tab _currentTab = Tab.Time;
    private int _currentLevel = 1;
    private string _requestedLeaderboardId;
    private float _nextLeaderboardRequestTime;

    public Tab CurrentTab => _currentTab;
    public int CurrentLevel => _currentLevel;

    public LeaderboardPanelLogic(int topEntriesCount, Action<LBData> onApply)
    {
        _topEntriesCount = topEntriesCount;
        _onApply = onApply;
    }

    public void Enable()
    {
        YG2.onGetLeaderboard += OnGetLeaderboard;
        Request();
    }

    public void Disable()
    {
        YG2.onGetLeaderboard -= OnGetLeaderboard;
    }

    public void SelectTab(Tab tab)
    {
        _currentTab = tab;
        Request();
    }

    public void SelectLevel(int level)
    {
        _currentLevel = level;
        Request();
    }

    private void Request()
    {
        if (_currentTab == Tab.Time)
            _requestedLeaderboardId = LeaderboardIds.Time(_currentLevel);
        else if (_currentTab == Tab.Crowns)
            _requestedLeaderboardId = LeaderboardIds.Crowns;
        else
            _requestedLeaderboardId = LeaderboardIds.Stars;

        LBData leaderboardcached;
        if (_leaderboardCache.TryGetValue(_requestedLeaderboardId, out leaderboardcached))
            _onApply(leaderboardcached);

        if (Time.unscaledTime < _nextLeaderboardRequestTime)
            return;

        _nextLeaderboardRequestTime = Time.unscaledTime + 1.1f;
        YG2.GetLeaderboard(_requestedLeaderboardId, _topEntriesCount, 3);
    }

    private void OnGetLeaderboard(LBData data)
    {
        if (data == null || data.technoName != _requestedLeaderboardId)
            return;

        _leaderboardCache[_requestedLeaderboardId] = data;
        _onApply(data);
    }
}