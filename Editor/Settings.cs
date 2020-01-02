using UnityEditor;
using UnityEngine;

namespace litefeel.RenameTools
{
    public static class Settings
    {
        private const string StartNumberKey = "litefeel.RenameTools.StartNumberKey";


        [InitializeOnLoadMethod]
        private static void Init()
        {
            _StartNumber = EditorPrefs.GetInt(StartNumberKey, 1);
        }

        private static int _StartNumber;
        public static int StartNumber
        {
            get { return _StartNumber; }
            set
            {
                value = Mathf.Max(0, value);
                if (value != _StartNumber)
                {
                    _StartNumber = value;
                    EditorPrefs.SetInt(StartNumberKey, value);
                }
            }
        }


        private class MyPrefSettingsProvider : SettingsProvider
        {
            public MyPrefSettingsProvider(string path, SettingsScope scopes = SettingsScope.User)
            : base(path, scopes)
            { }

            public override void OnGUI(string searchContext)
            {
                Settings.OnGUI();
            }
        }

        [SettingsProvider]
        static SettingsProvider NewPreferenceItem()
        {
            return new MyPrefSettingsProvider("Preferences/Rename Tools");
        }

        public static void OnGUI()
        {
            EditorGUILayout.LabelField("Start number of game object name suffix.");
            Settings.StartNumber = EditorGUILayout.IntField("Start Number", Settings.StartNumber);
        }
    }
}


