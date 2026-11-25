using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class HeroesPanelView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _knightSlotButton;
    [SerializeField] private Button _rogueSlotButton;
    [SerializeField] private Button _mageSlotButton;
    [SerializeField] private GameObject _knightCheck;
    [SerializeField] private GameObject _rogueCheck;
    [SerializeField] private GameObject _mageCheck;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Animator _previewAnimator;
    [SerializeField] private Button _selectButton;
    [SerializeField] private GameObject _lockedButtonsRoot;
    [SerializeField] private HeroDefinition _knight;
    [SerializeField] private HeroDefinition _rogue;
    [SerializeField] private HeroDefinition _mage;
    [SerializeField] private Button _trialButton;
    [SerializeField] private Button _buyButton;

    public event Action TrialClicked;
    public event Action BuyClicked;

    public event Action Closed;
    public event Action<HeroId> SelectClicked;
    public event Action<HeroId> SlotClicked;

    private HeroId _focusedId = HeroId.Knight;
    private void Awake()
    {
        _closeButton.onClick.AddListener(() => Closed?.Invoke());
        _selectButton.onClick.AddListener(() => SelectClicked?.Invoke(_focusedId));
        _knightSlotButton.onClick.AddListener(() => SlotClicked?.Invoke(HeroId.Knight));
        _rogueSlotButton.onClick.AddListener(() => SlotClicked?.Invoke(HeroId.Rogue));
        _mageSlotButton.onClick.AddListener(() => SlotClicked?.Invoke(HeroId.Mage));
        _lockedButtonsRoot.SetActive(false);
        _trialButton.onClick.AddListener(() => TrialClicked?.Invoke());
        _buyButton.onClick.AddListener(() => BuyClicked?.Invoke());

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();
        _knightSlotButton.onClick.RemoveAllListeners();
        _rogueSlotButton.onClick.RemoveAllListeners();
        _mageSlotButton.onClick.RemoveAllListeners();
    }

    public void Open(HeroId equipped)
    {
        gameObject.SetActive(true);
        ShowHero(equipped);
        RefreshChecks(equipped);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void ShowHero(HeroId heroId)
    {
        _focusedId = heroId;
        HeroDefinition heroDefinition = GetDef(heroId);
        _nameText.SetText(heroDefinition.DisplayName);
        _descriptionText.SetText(heroDefinition.Description);

        if (_previewAnimator != null)
        {
            _previewAnimator.runtimeAnimatorController = heroDefinition.MenuIdle;
            if (heroDefinition.MenuIdle != null)
                _previewAnimator.Play(0, 0, 0f);
        }
    }
    public void RefreshChecks(HeroId equipped)
    {
        _knightCheck.SetActive(equipped == HeroId.Knight);
        _rogueCheck.SetActive(equipped == HeroId.Rogue);
        _mageCheck.SetActive(equipped == HeroId.Mage);
    }
    private HeroDefinition GetDef(HeroId heroId)
    {
        if (heroId == HeroId.Rogue)
            return _rogue;

        if (heroId == HeroId.Mage)
            return _mage;

        return _knight;
    }
    public void ApplyActions(bool showSelect, bool showLocked)
    {
        _selectButton.gameObject.SetActive(showSelect);
        _lockedButtonsRoot.SetActive(showLocked);
    }
}