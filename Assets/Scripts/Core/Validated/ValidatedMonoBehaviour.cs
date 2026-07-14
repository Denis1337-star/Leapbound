using UnityEngine;

public abstract class ValidatedMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        bool isValid =ValidateInternal();

        if(isValid)
            return;

        Debug.LogError($"{name} проверка не пройдена в {GetType().Name}", this);
        enabled = false;
    }
    protected abstract bool ValidateInternal();
}
