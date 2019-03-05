using UnityEditor;

public class SplitFireEditor
{
    [UnityEditor.MenuItem("Assets/Create/Weapons/SplitFire")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<SplitFire>();
    }
}
