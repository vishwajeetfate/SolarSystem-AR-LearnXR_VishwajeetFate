using UnityEngine;

public static class InteractionManager
{
    private static bool isFrozen = false;

    public static bool IsFrozen => isFrozen;

    public static void Freeze()
    {
        isFrozen = true;
        Debug.Log("INTERACTION FROZEN");
    }

    public static void Unfreeze()
    {
        isFrozen = false;
        Debug.Log("INTERACTION UNFROZEN");
    }
}
