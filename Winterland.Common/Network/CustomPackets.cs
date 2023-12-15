using System;
using System.IO;
using System.Text;

namespace Winterland.Common {

    public class PacketFactory {
        public static Packet CreateBlankFromId(string id) {
            switch(id) {
                case ServerAcceptGiftPacket.PacketId:
                    return new ServerAcceptGiftPacket();
                case ServerRejectGiftPacket.PacketId:
                    return new ServerRejectGiftPacket();
                case ClientCollectGiftPacket.PacketId:
                    return new ClientCollectGiftPacket();
                case ServerEventProgressPacket.PacketId:
                    return new ServerEventProgressPacket();
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
    public class ClientCollectGiftPacket : Packet {
        public const string PacketId = "Xmas-Client-CollectGift";
        public override string GetPacketId() { return ClientCollectGiftPacket.PacketId; }
        protected override uint LatestVersion => 1;

        protected override void Write(BinaryWriter writer) {

        }

        protected override void Read(BinaryReader reader) {

        }
    }

    [Serializable]
    public class ServerAcceptGiftPacket : Packet {
        public const string PacketId = "Xmas-Server-AcceptGift";
        public override string GetPacketId() { return ServerAcceptGiftPacket.PacketId; }
        protected override uint LatestVersion => 1;
        protected override void Write(BinaryWriter writer) {

        }

        protected override void Read(BinaryReader reader) {

        }
    }

    [Serializable]
    public class ServerRejectGiftPacket : Packet {
        public const string PacketId = "Xmas-Server-RejectGift";
        public override string GetPacketId() { return ServerRejectGiftPacket.PacketId; }
        protected override uint LatestVersion => 1;
        protected override void Write(BinaryWriter writer) {

        }

        protected override void Read(BinaryReader reader) {

        }
    }

    [Serializable]
    public class ServerEventProgressPacket : Packet {
        public const string PacketId = "Xmas-Server-EventProgress";
        public override string GetPacketId() { return ServerEventProgressPacket.PacketId; }
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
