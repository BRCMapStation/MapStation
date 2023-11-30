using System;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.MapStation.Common.Serialization {

    /// <summary>
    /// Example how to workaround BepInEx broken serialization of custom
    /// classes.
    /// </summary>

    class SerializationExample : MonoBehaviour {
        [Serializable]
        class Conversation {
            public string showIfUnlockableIsUnlocked;
        }

        [Serializable]
        class Dialog {
            public string say;
            public string animation;
            public bool askYesNo;
        }

        [SerializeReference] Conversation conversation = new();

        // subclass SList, Unity does not understand generics
        class SList_NpcDialog : SList<Dialog> {}
        
        // Serialize as an SList
        [SerializeReference] SList_NpcDialog npcDialog_ = new();

        // property makes our code cleaner
        List<Dialog> npcDialogs => npcDialog_.items;

        private void Awake() {
            string message = "";
            message += string.Format("{0} serialized data:\n", nameof(SerializationExample));
            message += string.Format("{0}.{1}={2}\n", nameof(Conversation), nameof(Conversation.showIfUnlockableIsUnlocked), conversation.showIfUnlockableIsUnlocked);
            message += string.Format("{0}.Count={1}\n", nameof(npcDialogs), npcDialogs.Count);
            for(int i = 0; i < npcDialogs.Count; i++) {
                message += string.Format("{0}[{1}].{2}={3}\n", nameof(npcDialogs), i, nameof(Dialog.say), npcDialogs[i].say);
                message += string.Format("{0}[{1}].{2}={3}\n", nameof(npcDialogs), i, nameof(Dialog.animation), npcDialogs[i].animation);
                message += string.Format("{0}[{1}].{2}={3}\n", nameof(npcDialogs), i, nameof(Dialog.askYesNo), npcDialogs[i].askYesNo);
            }
            Debug.Log(message);
        }
    }
}
