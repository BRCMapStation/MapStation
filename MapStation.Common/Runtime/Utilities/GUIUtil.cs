using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using GL = UnityEditor.EditorGUILayout;
using G = UnityEditor.EditorGUI;
#else
using GL = UnityEngine.GUILayout;
using G = UnityEngine.GUI;
#endif

namespace cspotcode.UnityGUI {
    public static class GUIUtil {
        
        public const char UpTriangle = '\u25b2';
        public const char DownTriangle = '\u25bc';
        public const char RightTriangle = '\u25b6';
        
        public static IDisposable ScrollView(ref Vector2 position) {
            var scope = new GL.ScrollViewScope(position);
            position = scope.scrollPosition;
            return scope;
        }
        public static readonly IDisposable EndScrollView = new EndScrollViewDisposable();
        private class EndScrollViewDisposable : IDisposable {
            public void Dispose() {
                GL.EndScrollView();
            }
        }
        
        private class EndVerticalDisposable : IDisposable {
            public void Dispose() {
                GL.EndVertical();
            }
        }
        
        public static IDisposable Horizontal() {
            GL.BeginHorizontal();
            return EndHorizontal;
        }
        public static readonly IDisposable EndHorizontal = new EndHorizontalDisposable();
        private class EndHorizontalDisposable : IDisposable {
            public void Dispose() {
                GL.EndHorizontal();
            }
        }
        
        public static IDisposable Disabled(bool disabled = true) {
#if UNITY_EDITOR
            return new EditorGUI.DisabledScope(disabled);
#else
            var scope = new EndDisabledDisposable(GUI.enabled);
            GUI.enabled = !disabled;
            return scope;
#endif
        }
        private class EndDisabledDisposable : IDisposable {
            private readonly bool stateBefore;
            internal EndDisabledDisposable(bool stateBefore) {
                this.stateBefore = stateBefore;
            }
            public void Dispose() {
                GUI.enabled = this.stateBefore;
            }
        }

        // SerializedProperty stuff is only in-editor
#if UNITY_EDITOR
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

        public static IEnumerable<SerializedProperty> IterChildren(this SerializedProperty prop, bool skipScriptName = true, bool onlyIfExpanded = false) {
            if(onlyIfExpanded && !prop.isExpanded) yield break;
            prop = prop.Copy();
            var endOfChildrenIteration = prop.GetEndProperty();
            prop.NextVisible(true);
            do {
                if(skipScriptName && prop.name == "m_Script") continue;
                yield return prop;
            } while(prop.NextVisible(false) && !SerializedProperty.EqualContents(prop, endOfChildrenIteration));
        }

        public static IEnumerable<SerializedProperty> IterChildren(this SerializedObject obj, bool skipScriptName = true) {
            var prop = obj.GetIterator();
            prop.NextVisible(true);
            do {
                if(skipScriptName && prop.name == "m_Script") continue;
                yield return prop;
            } while(prop.NextVisible(false));
        }
#endif

        // Only EditorGUI supports indentation;
        // we approximate it for Runtime.
        public static int indentLevel {
#if UNITY_EDITOR
            get => EditorGUI.indentLevel;
            set => EditorGUI.indentLevel = value;
#else
            get;
            set;
#endif
        }
        private class IndentDisposable : IDisposable {
            private readonly int increment;

            internal IndentDisposable(int increment) {
                this.increment = increment;
            }
            public void Dispose() {
                indentLevel -= increment;
            }
        }
        private class ApplyIndentDisposable : IDisposable {
            public void Dispose() {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        public static IDisposable Indent(int increment = 1, bool apply = false) {
#if UNITY_EDITOR
            var indentScope = new EditorGUI.IndentLevelScope(increment);
#else
            indentLevel += increment;
            var indentScope = new IndentDisposable(increment);
#endif
            if (apply) {
                var applyIndentScope = ApplyIndent();
                return new CompositeDisposable(applyIndentScope, indentScope);
            } else {
                return indentScope;
            }
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
            // Don't use GUILayout.Space()
            // This Label trick enforces a fixed indentation, it doesn't wiggle around as contents change.
            GUILayout.Label("", GUILayout.Width(PixelsPerIndentLevel * indentLevel), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
            return EndApplyIndent;
        }
        public static readonly IDisposable EndApplyIndent = new ApplyIndentDisposable();
    }

    public static class Extensions {
        // Only Editor has SerializedProperty stuff
#if UNITY_EDITOR
        public static SerializedProperty Prop(this Editor editor, string name) {
            return editor.serializedObject.FindProperty(name);
        }
        public static SerializedProperty Prop(this SerializedObject serializedObject, string name) {
            return serializedObject.FindProperty(name);
        }
        public static SerializedProperty Prop(this SerializedProperty serializedProperty, string name) {
            return serializedProperty.FindPropertyRelative(name);
        }
#endif
    }

    /// <summary>
    /// When disposed, disposes the passed disposables in the order they were passed.
    /// </summary>
    public class CompositeDisposable : IDisposable {
        private readonly IDisposable[] disposables;
        public CompositeDisposable(params IDisposable[] disposables) {
            this.disposables = disposables;
        }
        public void Dispose() {
            foreach (var d in disposables) {
                d.Dispose();
            }
        }
    }
}

// For future reference:
// I used this snippet to test a bunch of unicode characters, see which ones are supported by Unity's font.

// foreach(var c in new string[] {
//     "\u2b95",
//     "\u2192",
//     "\u21d2",
//     "\u2b46",
//     "\u21c1",
//     "\u27a4",
//     "\u2b9c",
//     "\u2b9e",
//     "\u2b9d",
//     "\u2b9f",
//     "\u27a2",
//     "\u27a3",
//     "\u2b98",
//     "\u2b9a",
//     "\u2b99",
//     "\u2b9b",
//     "🠹", "🠸", "🠻", "🠺",
//     "🡄", "🡆", "🡅", "🡇",
//     "🠴", "🠶", "🠵", "🠷",
//     "←", "→", "↔", "↑", "↓", ",", "↕", ",", "↖", "↗", "↘", "↙", ",", "⤡", "⤢",
//     "\u25b2", // up
//     "\u23f5",
//     "\u23f6",
//     "\u25bc", // down
//     "\u25b6" // right
//     })
// {
//     Label($"{j++} {c}");
// }
