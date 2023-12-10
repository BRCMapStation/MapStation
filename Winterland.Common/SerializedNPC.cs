using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class SerializedNPC {
        public Guid GUID;
        public int DialogueLevel;
        private const byte Version = 0;

        public SerializedNPC(BinaryReader reader) {
            Read(reader);
        }

        public SerializedNPC(CustomNPC npc) {
            GUID = npc.GUID;
            DialogueLevel = npc.CurrentDialogueLevel;
        }

        public void Read(BinaryReader reader) {
            var version = reader.ReadByte();
            if (version > Version) {
                Debug.LogError($"Tried to load an NPC too new! (Version {version}), current version is {Version}");
                return;
            }
            GUID = Guid.Parse(reader.ReadString());
            DialogueLevel = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(Version);
            writer.Write(GUID.ToString());
            writer.Write(DialogueLevel);
        }
    }
}
