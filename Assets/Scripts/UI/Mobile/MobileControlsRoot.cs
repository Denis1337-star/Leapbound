using UnityEngine;
using YG;

public sealed class MobileControlsRoot : MonoBehaviour
{
    [SerializeField] private bool _forceShowInEditor = true;

    private void Start()
    {
        bool show = _forceShowInEditor;
        if (!Application.isEditor)
            show = YG2.envir.isMobile;

        gameObject.SetActive(show);
    }
}