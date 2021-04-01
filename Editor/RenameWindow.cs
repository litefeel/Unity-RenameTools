using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace litefeel.RenameTools
{
    public class RenameWindow : EditorWindow
    {
        private TextField m_NameTF;
        private VisualElement m_NameTI;
        private TextElement m_CountTxt;
        private List<TextField> m_TFList = new List<TextField>();

        private string m_TemplateName = "";
        private List<Transform> m_Selections = new List<Transform>();
        private TemplateContainer m_RootTree;

        [MenuItem("Window/LiteFeel/Rename Tools #F2")]
        public static void ShowExample()
        {
            RenameWindow wnd = GetWindow<RenameWindow>();
            wnd.titleContent = new GUIContent("Rename Tools");
            wnd.SetActive();
        }

        private void SetActive()
        {
            m_NameTI.Focus();
            OnSelectionChange();
            m_NameTF.SelectRange(m_NameTF.text.Length, m_NameTF.text.Length);
        }

        private void OnSelectionChange()
        {
            m_Selections = Selection.gameObjects
                .Where(go => !EditorUtility.IsPersistent(go))
                .Select(go => go.transform)
                .OrderBy(go => go, new SortCamper())
                .ToList();

            SetSelectionName();
            m_NameTF.value = m_TemplateName;
        }
        public void SetSelectionName()
        {
            m_TemplateName = m_Selections.Count > 0 ? m_Selections[0].name : "";
            UpdateChangeInfo();
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.litefeel.renametools/Editor/RenameWindow.uxml");
            m_RootTree = visualTree.CloneTree();
            root.Add(m_RootTree);
            m_NameTF = m_RootTree.Q<TextField>("nameTxt");
            m_NameTF.isDelayed = true;
            m_NameTI = m_NameTF.Q(TextField.textInputUssName);
            m_NameTI.RegisterCallback<KeyUpEvent>(e =>
            {
                m_TemplateName = m_NameTF.text;
                UpdateChangeInfo();
            });
            m_NameTI.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.KeypadEnter)
                {
                    m_NameTI.Focus();
                    DoRename();
                }
            });
            m_RootTree.Q<Button>("renameBtn").RegisterCallback<MouseUpEvent>(evt =>
            {
                m_NameTI.Focus();
                DoRename();
            });

            m_CountTxt = m_RootTree.Q<TextElement>("countTxt");
            m_CountTxt.text = "count:0";
        }

        private TextField CreateText()
        {
            var tf = new TextField();
            tf.SetEnabled(false);
            tf.focusable = false;
            return tf;
        }

        private void UpdateChangeInfo()
        {
            m_CountTxt.text = "\ncount:" + m_Selections.Count;
            int i;
            for (i = 0; i < m_Selections.Count; i++)
            {
                TextField tf = null;
                if (i < m_TFList.Count)
                    tf = m_TFList[i];
                else
                {
                    tf = CreateText();
                    m_TFList.Add(tf);
                    m_RootTree.Add(tf);
                }
                tf.label = m_Selections[i].name;
                tf.value = GetNewName(m_TemplateName, i);
                tf.style.display = DisplayStyle.Flex;
            }
            for (; i < m_TFList.Count; i++)
            {
                m_TFList[i].style.display = DisplayStyle.None;
            }
        }

        private void DoRename()
        {
            for (var i = 0; i < m_Selections.Count; i++)
            {
                Undo.RecordObject(m_Selections[i].gameObject, "Rename");
                m_Selections[i].name = GetNewName(m_TemplateName, i);
            }
            UpdateChangeInfo();
        }

        private string GetNewName(string format, int index)
        {
            string n = (index + Settings.StartNumber).ToString();
            if (format.Contains("$n"))

                return format.Replace("$n", n);
            else
                return $"{format}{n}";
        }

        class SortCamper : IComparer<Transform>
        {
            private Dictionary<Transform, int> s_Map = new Dictionary<Transform, int>();
            public int Compare(Transform a, Transform b)
            {
                // 是否兄弟节点
                if (a.parent == b.parent)
                    return a.GetSiblingIndex() - b.GetSiblingIndex();

                // 是否非跟节点
                var rootA = a.root;
                var rootB = b.root;
                if (rootA != rootB)
                    return rootA.GetSiblingIndex() - rootB.GetSiblingIndex();

                s_Map.Clear();
                int siblingIndx = -1;
                Transform transA = a;
                do
                {
                    s_Map.Add(transA, siblingIndx);
                    siblingIndx = transA.GetSiblingIndex();
                    transA = transA.parent;
                } while (transA != null);


                Transform transB = b;
                while (transB != null)
                {
                    if (s_Map.TryGetValue(transB.parent, out siblingIndx))
                    {
                        s_Map.Clear();
                        return siblingIndx - transB.GetSiblingIndex();
                    }
                    transB = transB.parent;
                }
                s_Map.Clear();
                return 0;
            }
        }
    }
}
