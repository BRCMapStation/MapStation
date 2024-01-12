using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace cspotcode.UnityGUI {
    public static class GUIUtil {
        private class EndScrollViewDisposable : IDisposable {
            public void Dispose() {
                EditorGUILayout.EndScrollView();
            }
        }
        private class EndVerticalDisposable : IDisposable {
            public void Dispose() {
                EditorGUILayout.EndVertical();
            }
        }
        private class EndHorizontalDisposable : IDisposable {
            public void Dispose() {
                EditorGUILayout.EndHorizontal();
            }
        }
        private class IndentDisposable : IDisposable {
            public void Dispose() {
                EditorGUI.indentLevel--;
            }
        }
        private class ApplyIndentDisposable : IDisposable {
            public void Dispose() {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        public static IDisposable ScrollView(ref Vector2 position) {
            var scope = new EditorGUILayout.ScrollViewScope(position);
            position = scope.scrollPosition;
            return scope;
        }
        public static readonly IDisposable EndScrollView = new EndScrollViewDisposable();

        public static IDisposable Horizontal() {
            EditorGUILayout.BeginHorizontal();
            return EndScrollView;
        }
        public static readonly IDisposable EndHorizontal = new EndHorizontalDisposable();
        public static IDisposable Indent(int increment = 1) {
            return new EditorGUI.IndentLevelScope(increment);
        }
        public static readonly IDisposable EndIndent = new IndentDisposable();

        public static void Draw(this SerializedProperty property) {
            EditorGUILayout.PropertyField(property);
        }

        public static void Draw(this SerializedObject obj, bool skipScriptName = true) {
            var prop = obj.GetIterator();
            prop.NextVisible(true);
            do {
                if(skipScriptName && prop.name == "m_Script") continue;
                prop.Draw();
            } while(prop.NextVisible(false));
        }

        public static bool DrawSelf(this SerializedProperty prop) {
            return EditorGUILayout.PropertyField(prop, false);
        }

        public static IEnumerable<SerializedProperty> DrawSelfIterChildren(this SerializedProperty prop, bool skipScriptName = true) {
            if(EditorGUILayout.PropertyField(prop, false))
            {
                using(Indent()) {
                    foreach(var child in IterChildren(prop, skipScriptName)) {
                        yield return child;
                    }
                }
            }
        }

        public static IEnumerable<SerializedProperty> IterChildren(this SerializedProperty prop, bool skipScriptName = true, bool ignoreVisibility = false) {
            if(!prop.isExpanded) yield break;
            prop = prop.Copy();
            var endOfChildrenIteration = prop.GetEndProperty();
            prop.NextVisible(true);
            do {
                if(skipScriptName && prop.name == "m_Script") continue;
                yield return prop;
            } while(prop.NextVisible(false) && !SerializedProperty.EqualContents(prop, endOfChildrenIteration));
        }

        public static IEnumerable<SerializedProperty> IterChildren(this SerializedObject obj, bool skipScriptName = true, bool ignoreVisibility = false) {
            var prop = obj.GetIterator();
            prop.NextVisible(true);
            do {
                if(skipScriptName && prop.name == "m_Script") continue;
                yield return prop;
            } while(prop.NextVisible(false));
        }

        private const int PixelsPerIndentLevel = 15;

        /// <summary>
        /// GUILayout (non-editor) does not obey EditorGUI's indentation
        /// so you can wrap that stuff with `using(ApplyIndent())`
        /// 
        /// Don't wrap EditorGUILayout stuff, cuz it'll be double-indented!
        /// </summary>
        public static IDisposable ApplyIndent() {
            GUILayout.BeginHorizontal();
            GUILayout.Space(PixelsPerIndentLevel * EditorGUI.indentLevel);
            GUILayout.BeginVertical();
            return EndApplyIndent;
        }
        public static readonly IDisposable EndApplyIndent = new ApplyIndentDisposable();
    }

    public static class Extensions {
        public static SerializedProperty Prop(this Editor editor, string name) {
            return editor.serializedObject.FindProperty(name);
        }
        public static SerializedProperty Prop(this SerializedObject serializedObject, string name) {
            return serializedObject.FindProperty(name);
        }
        public static SerializedProperty Prop(this SerializedProperty serializedProperty, string name) {
            return serializedProperty.FindPropertyRelative(name);
        }
    }
}
