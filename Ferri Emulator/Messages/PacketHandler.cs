﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Messages.Requests;
using System.Reflection;
using Ferri_Emulator.Communication;

namespace Ferri_Emulator.Messages
{
    public class PacketHandler
    {
        public Dictionary<short, Action<Message, Session>> Packets = new Dictionary<short, Action<Message, Session>>();

        public PacketHandler()
        {
            Packets.Add(Opcodes.OpcodesIn.ReadRelease, Handshake.ReadRelease);
            Packets.Add(Opcodes.OpcodesIn.InitCrypto, Handshake.InitCrypto);
            Packets.Add(Opcodes.OpcodesIn.InitRC4, Handshake.InitRC4);
            Packets.Add(Opcodes.OpcodesIn.AuthenticateUser, Handshake.AuthenticateUser);
            Packets.Add(Opcodes.OpcodesIn.GetUser, Users.GetUser);
            Packets.Add(Opcodes.OpcodesIn.GetHotelview, Others.GetHotelview);
            Packets.Add(Opcodes.OpcodesIn.GetPurse, Users.GetPurse);
            Packets.Add(Opcodes.OpcodesIn.GetUNK1, Others.UNK1);
            Packets.Add(Opcodes.OpcodesIn.GetCampaignWinners, Others.GetCampaignWinners);
            Packets.Add(Opcodes.OpcodesIn.GetNavigatorFrontpage, Navigator.NavigatorFrontpage);
            Packets.Add(Opcodes.OpcodesIn.GetOwnRooms, Navigator.OwnRooms);
            Packets.Add(Opcodes.OpcodesIn.GetShopTabs, Catalog.CatalogTabs);
            Packets.Add(Opcodes.OpcodesIn.GetRecyclerRewards, Catalog.GetRecyclerRewards);
            Packets.Add(Opcodes.OpcodesIn.GetShopData, Catalog.GetShopData);
            Packets.Add(Opcodes.OpcodesIn.GetGiftWrappings, Catalog.GetGiftWrapping);
            Packets.Add(Opcodes.OpcodesIn.GetBundleDiscount, Catalog.GetBundleDiscount);
            Packets.Add(Opcodes.OpcodesIn.GetShopPage, Catalog.GetShopPage);
            Packets.Add(Opcodes.OpcodesIn.GetVIPBuyDialog, Catalog.GetVIPBuyDialog);
            Packets.Add(Opcodes.OpcodesIn.GetSearchResults, Navigator.SearchedRooms);
            Packets.Add(Opcodes.OpcodesIn.EnterRoom, Rooms.BeginEnterRoom);
            Packets.Add(Opcodes.OpcodesIn.GetRoomModelData, Rooms.GetRoomModeldata);
            Packets.Add(Opcodes.OpcodesIn.GetEndEnterRoom, Rooms.GetEndEnterRoom);
            Packets.Add(Opcodes.OpcodesIn.GetTalentsMeter, Others.GetCitizenship);
            Packets.Add(Opcodes.OpcodesIn.Move, Rooms.Move);
            Packets.Add(Opcodes.OpcodesIn.GetRoomGroupData, Rooms.GetGroupData);
            Packets.Add(Opcodes.OpcodesIn.GetInventory, new Action<Message,Session>(Users.GetInventory));
            Packets.Add(Opcodes.OpcodesIn.GetInventoryBadges, new Action<Message, Session>(Users.GetInventoryBadges));
            Packets.Add(Opcodes.OpcodesIn.UpdateBadges, Users.UpdateBadges);
            Packets.Add(Opcodes.OpcodesIn.Talk, Rooms.Talk);
            Packets.Add(Opcodes.OpcodesIn.GetInventoryBots, Rooms.GetInventoryBots);
            Packets.Add(Opcodes.OpcodesIn.GetRoomUserBadges, Rooms.GetRoomUserBadges);
            Packets.Add(Opcodes.OpcodesIn.GetAchievementList, Users.GetAchievements);
        }

        public void Handle(Message Msg, Session Session)
        {
            short Header = Msg.HeaderId;
            int Length = Msg.Length;

            if (!Packets.ContainsKey(Header))
            {
                Engine.Logging.WriteErrorTagLine(Header.ToString(), "Unregistered! - {0}", Engine.ToReadableString(Encoding.Default.GetString(Msg.GetBytes)));
                return;
            }

            Packets[Header].Invoke(Msg, Session);
        }
    }
}
