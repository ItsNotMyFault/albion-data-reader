using events;
using GankCompanionDataReader.eventHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GankCompanionDataReader.packethandler
{
    public class AlbionEventHandler
    {

        private PartyEventHandler partyEventHandler;
        
        public AlbionEventHandler(string url, IPartyRepository partyRepository)
        {

            this.partyEventHandler = new PartyEventHandler(url, partyRepository);
        }

        public void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            if (code == 2)
            {
                return;
            }
            object val;
            parameters.TryGetValue(252, out val);
            if (val == null)
            {
                return;
            }
            int iCode = 0;
            if (!int.TryParse(val.ToString(), out iCode))
            {
                return;
            }
            EventCodes eventCode = (EventCodes)iCode;
            HandleEventCode(eventCode, parameters);
        }

        public bool IfIsEventToIgnore(EventCodes eventCode)
        {
            return eventCode == EventCodes.evGuildMemberWorldUpdate
                || eventCode == EventCodes.evPlayerGetonline
                || eventCode == EventCodes.evevLeave
                || eventCode == EventCodes.evOutpostReward
                || eventCode == EventCodes.evDebugDiminishingReturnInfo
                || eventCode == EventCodes.evKilledPlayer
                || eventCode == EventCodes.evAccessStatus
                || eventCode == EventCodes.evSiegeCampScheduleResult
                || eventCode == EventCodes.evZonethroughNewZone
                || eventCode == EventCodes.evStopsMoving
                || eventCode == EventCodes.evLeftChatChannel
                || eventCode == EventCodes.evMatchPlayerJoinedEvent
                || eventCode == EventCodes.evCastHit
                || eventCode == EventCodes.evAchievementProgressInfo
                || eventCode == EventCodes.evUpdateFame
                || eventCode == EventCodes.evAchievementProgressInfo
                || eventCode == EventCodes.evActiveSpellEffectsUpdate
                || eventCode == EventCodes.evDurabilityChanged
                || eventCode == EventCodes.evRegenerationHealthEnergyComboChanged
                || eventCode == EventCodes.evRegenerationHealthChanged
                || eventCode == EventCodes.evResurrectionReply
                || eventCode == EventCodes.evUpdateUnlockedAvatarRings
                || eventCode == EventCodes.evAchievementProgressInfo
                || eventCode == EventCodes.evPlaceableObjectPlaceCancel
                || eventCode == EventCodes.evMove3
                || eventCode == EventCodes.evPlayerLoginOrLogOff;
        }

        public void HandleEventCode(EventCodes eventCode, Dictionary<byte, object> parameters)
        {
            if (IfIsEventToIgnore(eventCode))
            {
                return;
            }
      
            if (eventCode == EventCodes.evZonethroughNewZone)
            {
                Debug.WriteLine("===========NEW ZONE DETECTED ===============");
            }

            if (eventCode == EventCodes.evPartyCreated
               || eventCode == EventCodes.evPartyPlayerJoined
               || eventCode == EventCodes.evPartyPlayerLeave
               || eventCode == EventCodes.evPartyLeaderChanged
               || eventCode == EventCodes.evPartyPlayerLeft
               )
            {
                Console.WriteLine("Event: " + eventCode.ToString());
                this.partyEventHandler.HandleEvent(eventCode, parameters);
            }

        }
    }
}
