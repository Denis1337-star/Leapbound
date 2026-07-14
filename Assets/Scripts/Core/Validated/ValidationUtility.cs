using UnityEngine;

public static class ValidationUtility 
{
    private const string UnknownOwnerName = "Unknown Owner";

    public static bool IsAssigned(Object owner, Object value, string fieldName)
    {
        if (value != null)
            return true;

        LogError(owner, fieldName, "не назначено");
        return false;
    }
    private static void LogError(Object owner, string fieldName, string reason)
    {
        string ownerName = owner != null
            ? owner.name
            : UnknownOwnerName;

        Debug.LogError($"{ownerName}: обязательное поле \"{fieldName}\" {reason}.", owner);
    }

}
