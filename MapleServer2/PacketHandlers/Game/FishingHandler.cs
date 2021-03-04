using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    // This is not working yet. for some reason it does not give the "start" prompt after casting line.
    public class FishingHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.FISHING;

        public FishingHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            switch (function)
            {
                case 0:
                    HandleStart(session, packet);
                    break;
                case 1:
                    HandleStop(session);
                    break;
                case 8: // Complete Fishing
                    // When fishing manually, 0 = success minigame, 1 = no minigame
                    // When auto-fishing, it seems to send 0, gets back failed fishing response
                    // Then it sends 1 and gets back 0x04 response before restarting fishing again
                    bool completed = packet.ReadBool(); // Completed without minigame
                    // Give fish!
                    session.Send(FishingPacket.CatchFish(10000001, 100, true));
                    break;
                case 10: // Failed minigame
                    break;
            }
        }


        private static void HandleStart(GameSession session, PacketReader packet)
        {
            long rodItemUid = packet.ReadLong(); // Testing RodItemId: 32000055
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteHexString("04 00 0F 00 00 00 1D FC 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1C FD 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1B FE 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1D FD 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1C FE 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1D FE 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1B FA 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1C FA 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1B FB 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1D FA 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1C FB 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1B FC 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1D FB 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1C FC 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 1B FD 08 00 81 96 98 00 19 00 00 00 98 3A 00 00 01");

            session.Send(pWriter);
            session.Send(GuideObjectPacket.Add(session));
            session.Send(FishingPacket.Start(rodItemUid));
        }

        private static void HandleStop(GameSession session)
        {
            session.Send(FishingPacket.Stop());
            session.Send(GuideObjectPacket.Remove(session.FieldPlayer));
        }
    }
}
