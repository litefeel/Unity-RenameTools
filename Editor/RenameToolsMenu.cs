using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEditor.ShortcutManagement;
using UnityEngine;
#endif

namespace litefeel.RenameTools
{
    public static class RenameToolsMenu
    {
#if UNITY_2019_1_OR_NEWER
        private const string WindowMenuPath = "Window/LiteFeel/Rename Tools";
#else
        private const string WindowMenuPath = "Window/LiteFeel/Rename Tools/Align Tools %#R";
#endif
        // Creation of window
        [MenuItem(WindowMenuPath)]
        private static void RenameToolsWindows()
        {
            var window = EditorWindow.GetWindow<RenameToolsWindow>(false, "Rename Tools", true);
            window.Show();
            window.Focus();
            window.SetSelectionName();
            window.autoRepaintOnSceneChange = true;
        }
    }
}


