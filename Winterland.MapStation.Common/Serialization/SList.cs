using System;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.MapStation.Common.Serialization {
    // Empty non-generic superclass lets us `[PropertyDrawer(typeof(SList), true)]`
    // in the future.  `PropertyDrawer` does not understand generics.
    public class SList {}

    [Serializable]
    public class SList<T> : SList, ISerializationCallbackReceiver
        where T : new()
    {
        [HideInInspector] // <-- Uncomment for debugging
        [SerializeReference]
        private Node linkedList;

        // It breaks without [SerializeReference] here, not sure why
        [SerializeReference]
        public List<T> items;

        public void copyToLinkedList() {
            if(items == null || items.Count == 0) {
                linkedList = null;
            } else {
                linkedList ??= new Node();
                var node = linkedList;
                for(int i = 0, l = items.Count; i < l; i++) {
                    node.value = items[i];
                    if(i < l - 1) {
                        node.next ??= new Node();
                        node = node.next;
                    } else {
                        node.next = null;
                    }
                }
            }
        }

        public void copyToList() {
            // TODO optimize for common case: they already match.
            if(items == null) items = new();
            else items.Clear();
            for(var node = linkedList; node != null; node = node.next) {
                items.Add((T)node.value);
            }
        }

        private void instantiateNullList() {
            items ??= new();
        }

        /// <summary>
        /// Replace nulls in list with instances.
        /// Only necessary when List is marked [SerializeReference] TODO testing
        /// </summary>
        private void instantiateNullItemsInList() {
            for(int i = 0, l = items.Count; i < l; i++) {
                if(items[i] == null) {
                    items[i] = new ();
                }
            }
        }

        public void OnBeforeSerialize() {
            // NOTE: We are repeatedly serialized w/out being deserialized between.
            //       Do not erase the list, it is still being used.

            // NOTE: Cannot call `isBuildingPlayer` during serialization!!
            //       Serialization runs on another thread.

            instantiateNullList();
            instantiateNullItemsInList();
            copyToLinkedList();
        }

        public void OnAfterDeserialize() {
            // BepInEx fails to deserialize the list, so it must hydrate from SList
            // In the editor this is never a problem.
            // Inspector edits the List directly. Overwriting it w/LinkedList would
            // prevent inspector changes from saving.
            if(!Application.isEditor) {
                copyToList();
            }
        }
    }

    [Serializable]
    public class Node {
        [SerializeReference] public object value;
        [SerializeReference] public Node next;
    }
}