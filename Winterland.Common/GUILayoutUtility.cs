using System;
using UnityEngine;

namespace Winterland.Common;

public static class GUILayoutUtility {
    public static IDisposable Horizontal(params GUILayoutOption[] options) {
        GUILayout.BeginHorizontal(options);
        return new HorizontalDisposable();
    }
    public static IDisposable Vertical(params GUILayoutOption[] options) {
        GUILayout.BeginVertical(options);
        return new VerticalDisposable();
    }
    private class HorizontalDisposable : IDisposable {
        public void Dispose() {
            GUILayout.EndHorizontal();
        }
    }
    private class VerticalDisposable : IDisposable {
        public void Dispose() {
            GUILayout.EndVertical();
        }
    }

    public static IDisposable Horizontal() {
        throw new NotImplementedException();
    }
}
