using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditorInternal;

public static class EditorHelper {
    public static Component FindFirstComponentAboveMe(Component component, Type componentType, out int distance, Predicate<Component> predicate = null) {
        var componentList = component.gameObject.GetComponents(typeof(Component));
        Component lastComponentPassed = null;
        distance = 0;
        for(var i = 0; i < componentList.Length; i++) {
            distance++;
            var currentComponent = componentList[i];
            if (currentComponent == component)
                break;
            if (componentType.IsAssignableFrom(currentComponent.GetType())) {
                if (predicate != null) {
                    var predicateResult = predicate.Invoke(currentComponent);
                    if (predicateResult) {
                        lastComponentPassed = currentComponent;
                        distance = 0;
                    }
                } else {
                    lastComponentPassed = currentComponent;
                    distance = 0;
                }
            }
        }
        return lastComponentPassed;
    }

    public static Component FindFirstComponentBelowMe(Component component, Type componentType, out int distance, Predicate<Component> predicate = null) {
        var componentList = component.gameObject.GetComponents(typeof(Component));
        Component lastComponentPassed = null;
        distance = 0;
        for (var i = componentList.Length - 1; i >= 0; i--) {
            distance++;
            var currentComponent = componentList[i];
            if (currentComponent == component)
                break;
            if (componentType.IsAssignableFrom(currentComponent.GetType())) {
                if (predicate != null) {
                    var predicateResult = predicate.Invoke(currentComponent);
                    if (predicateResult) {
                        lastComponentPassed = currentComponent;
                        distance = 0;
                    }
                } else {
                    lastComponentPassed = currentComponent;
                    distance = 0;
                }
            }
        }
        return lastComponentPassed;
    }

    public static void MoveUp(Component component, Type componentType, Predicate<Component> predicate = null) {
        var aboveMe = FindFirstComponentAboveMe(component, componentType, out var distance, predicate);
        Debug.Log($"Up: {distance}");
        for (var i = 0; i < distance; i++) {
            ComponentUtility.MoveComponentUp(component);
        }
    }

    public static void MoveDown(Component component, Type componentType, Predicate<Component> predicate = null) {
        var belowMe = FindFirstComponentBelowMe(component, componentType, out var distance, predicate);
        Debug.Log($"Down: {distance}");
        for (var i = 0; i < distance; i++) {
            ComponentUtility.MoveComponentDown(component);
        }
    }
}
