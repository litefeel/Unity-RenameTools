using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace litefeel.RenameTools
{
    public class RenameToolsWindow : EditorWindow
    {
        const string FocusName = "FocusName";
        const string FocusButton = "FocusButton";

        private string m_NewName;
        private bool m_NeedFocusMyGoName;
        private bool m_NeedFocusMyButton;
        private bool m_NeedFocusMyButton2;

        private List<Transform> m_Selections = new List<Transform>();

        public void SetSelectionName()
        {
            OnSelectionChange();
            m_NewName = m_Selections.Count > 0 ? m_Selections[0].name : "";
            m_NeedFocusMyGoName = true;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
        }
        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChange;
        }

        private void OnSelectionChange()
        {
            m_Selections = Selection.gameObjects
                .Where(go => !EditorUtility.IsPersistent(go))
                .Select(go => go.transform)
                .OrderBy(go => go, new SortCamper())
                .ToList();
        }

        // Update the editor window when user changes something (mainly useful when selecting objects)
        void OnInspectorUpdate()
        {
            Repaint();
            if (m_NeedFocusMyButton)
            {
                m_NeedFocusMyButton = false;
                m_NeedFocusMyButton2 = true;
            }
        }

        private void OnGUI()
        {
            GUI.SetNextControlName(FocusName);
            m_NewName = EditorGUILayout.TextField("NewName", m_NewName);

            var dontRename = string.IsNullOrWhiteSpace(m_NewName) || m_Selections.Count == 0;
            using (new EditorGUI.DisabledScope(dontRename))
            {
                GUI.SetNextControlName(FocusButton);
                if (GUILayout.Button("Rename"))
                    DoRename();
            }

            EditorGUILayout.Space();
            if (!dontRename)
            {
                EditorGUILayout.LabelField($"rename count:{m_Selections.Count} -----");
                var startNum = Settings.StartNumber;
                for (var i = 0; i < m_Selections.Count; i++)
                {
                    EditorGUILayout.LabelField(m_Selections[i].name, $"{m_NewName}{i + startNum}");
                }
            }

            if (m_NeedFocusMyGoName)
            {
                m_NeedFocusMyGoName = false;
                EditorGUI.FocusTextInControl(FocusName);
            }
            if (m_NeedFocusMyButton2)
            {
                m_NeedFocusMyButton2 = false;
                GUI.FocusControl(FocusButton);
            }
            if (Event.current.type == EventType.KeyDown)
            {
                //Debug.Log($"key code {Event.current.type}, {Event.current.keyCode}, {GUI.GetNameOfFocusedControl()}");
                if (Event.current.keyCode == KeyCode.Tab)
                {
                    if (GUI.GetNameOfFocusedControl() == FocusName)
                    {
                        m_NeedFocusMyButton = true;
                        Event.current.Use();
                    }
                }
            }
        }

        private void DoRename()
        {
            var startNum = Settings.StartNumber;
            for (var i = 0; i < m_Selections.Count; i++)
            {
                Undo.RecordObject(m_Selections[i].gameObject, "Rename");
                m_Selections[i].name = $"{m_NewName}{i + startNum}";
            }
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


