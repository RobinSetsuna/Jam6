using UnityEditor;

public class StraightFireEditor
{
    [UnityEditor.MenuItem("Assets/Create/Weapons/StraightFire")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<StraightFire>();
    }
}
