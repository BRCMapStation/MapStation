#if MAPSTATION_DEBUG
using System;
using System.Collections.Generic;
using UnityEngine;
namespace MapStation.Common {
    
    // cspotcode uses this to test FixPluginTypesSerialization
    // https://github.com/cspotcode/FixPluginTypesSerialization
    // It's easier than creating a dedicated DLL and assetbundle workflow in that repo
    
    [ExecuteInEditMode]
    public class TestFixPluginTypesSerialization : MonoBehaviour {
        public MyStruct structField;
        public MyClass classField;
        public List<MyStruct> listOfStructField;
        public List<MyClass> listOfClassField;

        private void Awake() {
            void logStruct(string indent, MyStruct s) {
                Debug.Log($"{indent}foo={s.foo}");
                Debug.Log($"{indent}bar={s.bar}");
            }
            void logClass(string indent, MyClass s) {
                Debug.Log($"{indent}foo={s.foo}");
                Debug.Log($"{indent}bar={s.bar}");
            }
            
            Debug.Log($"Dotnet version: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
            Debug.Log($"{nameof(TestFixPluginTypesSerialization)} fields");
            Debug.Log($"  {nameof(classField)}:");
            logClass("    ", classField);
            Debug.Log($"  {nameof(structField)}:");
            logStruct("    ", structField);
            foreach (var (i, v) in listOfClassField.Pairs()) {
                Debug.Log($"  {nameof(listOfClassField)}[{i}]:");
                logClass("      ", v);
            }
            foreach (var (i, v) in listOfStructField.Pairs()) {
                Debug.Log($"  {nameof(listOfStructField)}[{i}]:");
                logStruct("      ", v);
            }
        }
    }

    [Serializable]
    public class MyClass {
        public int foo;
        public string bar;
    }
    
    [Serializable]
    public struct MyStruct {
        public int foo;
        public string bar;
    }
}
#endif
