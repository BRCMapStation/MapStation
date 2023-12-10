using System;
using System.IO;
using System.Text;

namespace Winterland.Common {

    public class PacketFactory {
        public static Packet CreateBlankFromId(string id) {
            switch(id) {
                case PlayerCollectGiftsPacket.PacketId:
                    return new PlayerCollectGiftsPacket();
                case EventProgressPacket.PacketId:
                    return new EventProgressPacket();
                default:
                    return null;
            }
        }
    }

    public abstract class Packet {
        public uint Version;
        public uint PlayerID;

        /// <summary>
        /// New packets send with this version. Lower, older version numbers
        /// should only exist on received packets from outdated clients.
        /// </summary>
        protected abstract uint LatestVersion {get;}

        public abstract string GetPacketId();

        public Packet() {
            Version = LatestVersion;
        }

        public byte[] Serialize() {
            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false)) {
                writer.Write((UInt16) Version);
                Write(writer);
            }
            return stream.ToArray();
        }

        /// <summary>
        /// Write packet to bytes. Packet version has already been written.
        /// </summary>
        protected virtual void Write(BinaryWriter writer) {
            throw new NotImplementedException();
        }

        public void Deserialize(byte[] data) {
            var stream = new MemoryStream(data, false);
            using (var reader = new BinaryReader(stream, Encoding.UTF8)) {
                Version = reader.ReadUInt16();
                Read(reader);
            }
        }

        /// <summary>
        /// Parse packet from bytes. Packet version has already been read and set.
        /// </summary>
        protected virtual void Read(BinaryReader reader) {
            throw new NotImplementedException();
        }

        protected void UnexpectedVersion() {
            throw new PacketParseException($"Got packet with unexpected version: {this.GetType().Name} {Version}");
        }
    }

    class PacketParseException : Exception {
        public PacketParseException(string message) : base(message) {}
    }

    [Serializable]
    public class PlayerCollectGiftsPacket : Packet {
        public const string PacketId = "b5874188-d86c-4780-8091-fe24c197ef70";
        public override string GetPacketId() { return PlayerCollectGiftsPacket.PacketId; }
        protected override uint LatestVersion => 1;

        public int giftsDepositedCount;

        protected override void Write(BinaryWriter writer) {
            writer.Write((UInt16)giftsDepositedCount);
        }
        protected override void Read(BinaryReader reader) {
            switch(Version) {
                case 1:
                    giftsDepositedCount = reader.ReadUInt16();
                    break;
                default:
                    UnexpectedVersion();
                    break;
            }
        }
    }

    [Serializable]
    public class EventProgressPacket : Packet {
        public const string PacketId = "d5ca0185-8444-40de-97aa-32b282daad4f";
        public override string GetPacketId() { return EventProgressPacket.PacketId; }
        protected override uint LatestVersion => 1;

        // float from 0 to 1, 1 == completed tree
        public float TreeConstructionPercentage;

        protected override void Write(BinaryWriter writer) {
            writer.Write(TreeConstructionPercentage);
        }
        protected override void Read(BinaryReader reader) {
            switch(Version) {
                case 1:
                    TreeConstructionPercentage = reader.ReadSingle();
                    break;
                default:
                    UnexpectedVersion();
                    break;
            }
        }
    }
}