using System;
using YG;


public sealed class HeroesPanelLogic
{
    private const string RogueProductId = "rogue";
    private readonly HeroesPanelView _heroesPanelview;
    private readonly LevelRunContext _levelRunContext;
    private readonly HeroProgress _heroProgress;
    private readonly Action _onClosedToMenu;
    private HeroId _focusedId = HeroId.Knight;

    public HeroesPanelLogic(HeroesPanelView heroesPanelview,LevelRunContext levelRunContext,
        HeroProgress heroProgress,Action onClosedToMenu)
    {
        _heroesPanelview = heroesPanelview;
        _levelRunContext = levelRunContext;
        _heroProgress = heroProgress;
        _onClosedToMenu = onClosedToMenu;

        _heroesPanelview.Closed += OnClosed;
        _heroesPanelview.SlotClicked += OnSlot;
        _heroesPanelview.SelectClicked += OnSelect;
        _heroesPanelview.TrialClicked += OnTrial;
        _heroesPanelview.BuyClicked += OnBuy;

        YG2.onPurchaseSuccess += OnPurchaseSuccess;
    }

    public void Dispose()
    {
        _heroesPanelview.Closed -= OnClosed;
        _heroesPanelview.SlotClicked -= OnSlot;
        _heroesPanelview.SelectClicked -= OnSelect;
        _heroesPanelview.TrialClicked -= OnTrial;
        _heroesPanelview.BuyClicked -= OnBuy;

        YG2.onPurchaseSuccess -= OnPurchaseSuccess;
    }

    public void Open()
    {
        _focusedId = _levelRunContext.SelectedHero;
        _heroesPanelview.Open(_focusedId);
        RefreshActions(_focusedId);
    }

    private void OnClosed()
    {
        _heroesPanelview.Close();
        if (_onClosedToMenu != null)
            _onClosedToMenu();
    }

    private void OnSlot(HeroId heroId)
    {
        _focusedId = heroId;
        _heroesPanelview.ShowHero(heroId);
        RefreshActions(heroId);
    }

    private void OnSelect(HeroId heroId)
    {
        if (!_heroProgress.CanSelect(heroId))
            return;

        _levelRunContext.SetHero(heroId);
        _heroesPanelview.RefreshChecks(heroId);
        _heroProgress.Equip(heroId, _levelRunContext);
        _heroesPanelview.RefreshChecks(heroId);
    }

    private void OnTrial()
    {
        if (_focusedId != HeroId.Rogue)
            return;

        YandexRewardedBridge.Show("rogue_trial", OnTrialGranted);
    }

    private void OnTrialGranted()
    {
        _heroProgress.GrantRogueTrial();
        RefreshActions(HeroId.Rogue);
    }

    private void OnBuy()
    {
        if (_focusedId != HeroId.Rogue)
            return;
        YG2.BuyPayments(RogueProductId);
    }

    private void RefreshActions(HeroId heroId)
    {
        if (heroId == HeroId.Mage)
        {
            _heroesPanelview.ApplyActions(false, false);
            return;
        }

        if (heroId == HeroId.Knight)
        {
            _heroesPanelview.ApplyActions(true, false);
            return;
        }

        bool unlocked = _heroProgress.IsUnlocked(HeroId.Rogue);
        bool trial = _heroProgress.HasTrial(HeroId.Rogue);

        if (unlocked || trial)
            _heroesPanelview.ApplyActions(true, false);
        else
            _heroesPanelview.ApplyActions(false, true);
    }
    private void OnPurchaseSuccess(string heroId)
    {
        if (heroId != RogueProductId)
            return;
        _heroProgress.UnlockRogue();
        RefreshActions(HeroId.Rogue);
        YG2.ConsumePurchaseByID(heroId);
    }
}