using UnityEditor;

namespace litefeel.RenameTools
{
    public static class RenameToolsMenu
    {

        private const string WindowMenuPath = "Window/LiteFeel/Rename Tools #F2";

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


