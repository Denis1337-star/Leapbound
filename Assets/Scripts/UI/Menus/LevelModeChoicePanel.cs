using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class LevelModeChoicePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _normalPlayButton;
    [SerializeField] private Button _timeTrialButton;
    [SerializeField] private TMP_Text _targetTimeText;
    [SerializeField] private TMP_Text _bestTimeText;
    [SerializeField] private Button _backButton;

    private ISceneFlowService _sceneFlowService;
    private LevelRunContext _levelRunContext;
    private int _levelBuildIndex;
    private float _targetTimeSeconds;


    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, LevelRunContext levelRunContext)
    {
        _sceneFlowService = sceneFlowService;
        _levelRunContext = levelRunContext;
    }

    private void Awake()
    {
        _normalPlayButton.onClick.AddListener(OnNormalPlayClicked);
        _timeTrialButton.onClick.AddListener(OnTimeTrialClicked);
        _backButton.onClick.AddListener(Hide);
        Hide();
    }

    private void OnDestroy()
    {
        _normalPlayButton.onClick.RemoveListener(OnNormalPlayClicked);
        _timeTrialButton.onClick.RemoveListener(OnTimeTrialClicked);
        _backButton.onClick.RemoveListener(Hide);
    }

    public void Open(int levelBuildIndex, float targetSeconds, float bestSeconds, bool hasBestTime)
    {
        _levelBuildIndex = levelBuildIndex;
        _targetTimeSeconds = targetSeconds;
        _titleText.SetText("Уровень {0}", levelBuildIndex); 
        _targetTimeText.SetText(string.Format("Цель: {0}", LevelTimeFormat.Format(targetSeconds)));

        if (hasBestTime)
            _bestTimeText.SetText(string.Format("Лучшее: {0}", LevelTimeFormat.Format(bestSeconds)));
        else
            _bestTimeText.SetText("Рекорда ещё нет");

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnNormalPlayClicked()
    {
        _levelRunContext.SetNormal();
        _sceneFlowService.LoadLevelByBuildIndex(_levelBuildIndex);
    }

    private void OnTimeTrialClicked()
    {
        _levelRunContext.SetTimeTrial(_targetTimeSeconds);
        _sceneFlowService.LoadLevelByBuildIndex(_levelBuildIndex);
    }
}