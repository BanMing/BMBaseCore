using UnityEditor;

namespace BMBaseCore
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //gameConfig.isAssertBundle = EditorGUILayout.Toggle("Is Load AssertBundle", gameConfig.isAssertBundle);
        }
    }
}

