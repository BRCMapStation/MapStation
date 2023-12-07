using System;
using System.Data.Common;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Mono.Cecil;
using UnityEngine;

namespace Winterland.Common {
    public abstract class Packet {
        public byte[] Serialize() {
            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false)) {
                Write(writer);
            }
            return stream.ToArray();
        }

        protected virtual void Write(BinaryWriter writer) {
            throw new NotImplementedException();
        }

        public void Deserialize(byte[] data) {
            var stream = new MemoryStream(data, false);
            using (var reader = new BinaryReader(stream, Encoding.UTF8)) {
                Read(reader);
            }
        }

        protected virtual void Read(BinaryReader reader) {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class PlayerCollectGiftsPacket : Packet {
        public const string PacketId = "WinterlandPlayerCollectGifts";

        public int giftDepositedCount;

        protected override void Write(BinaryWriter writer) {
            writer.Write((UInt16)giftDepositedCount);
        }
        protected override void Read(BinaryReader reader) {
            giftDepositedCount = reader.ReadUInt16();
        }
    }

    [Serializable]
    public class EventProgressPacket : Packet {
        public const string PacketId = "WinterlandEventProgress";

        // Int from 0 to 100, 100 == tree is completely grown
        public int treeGrowthPercentage;

        protected override void Write(BinaryWriter writer) {
            writer.Write((UInt16)treeGrowthPercentage);
        }
        protected override void Read(BinaryReader reader) {
            treeGrowthPercentage = reader.ReadUInt16();
        }
    }
}