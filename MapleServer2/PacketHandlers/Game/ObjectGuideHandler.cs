using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ObjectGuideHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.GUIDE_OBJECT_SYNC;

        public ObjectGuideHandler(ILogger<ObjectGuideHandler> logger) : base(logger) { }

        private enum ObjectGuideMode : byte
        {
            Update = 0x01,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ObjectGuideMode mode = (ObjectGuideMode) packet.ReadByte();

            switch (mode)
            {
                case ObjectGuideMode.Update:
                    HandleUpdate(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleUpdate(GameSession session, PacketReader packet)
        {
            byte var1 = packet.ReadByte();
            byte var2 = packet.ReadByte();
            short var3 = packet.ReadShort();
            byte var4 = packet.ReadByte();
            CoordS coord = packet.Read<CoordS>();
            int var5 = packet.ReadInt();
            int var6 = packet.ReadInt();
            short var7 = packet.ReadShort();
            int var8 = packet.ReadInt();
            long counter = packet.ReadLong();
            int counter2 = packet.ReadInt();

            Console.WriteLine(
                $"var1 {var1} ,var2 {var2},var3 {var3},var4 {var4},coord {coord}" +
                $",var5 {var5},var6 {var6},var7 {var7},var8 {var8}," +
                $"counter {counter},counter2 {counter2} "
            );
        }
    }
}
