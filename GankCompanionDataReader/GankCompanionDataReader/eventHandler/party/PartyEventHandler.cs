using events;
using GankCompanionDataReader.eventHandler.party;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GankCompanionDataReader.eventHandler
{
    public class PartyEventHandler : IEventHandler
    {
        private PartyApiService partyService;
        public PartyEventHandler(string url, IPartyRepository partyRepository)
        {
            this.partyService = new PartyApiService(url, partyRepository);
        }

        public void HandleEvent(EventCodes eventCode, Dictionary<byte, object> parameters)
        {
            if (eventCode == EventCodes.evPartyPlayerJoined)
            {
                Debug.WriteLine(eventCode + " " + parameters.Values);
                JoinedPartyHandler(parameters);
            }
            else if (eventCode == EventCodes.evPartyCreated)
            {
                Debug.WriteLine(eventCode + " " + parameters.Values);
                CreatePartyHandler(parameters);
            }
            else if (eventCode == EventCodes.evPartyPlayerLeft)
            {
                Debug.WriteLine(eventCode + " " + parameters.Values);
                LeftPartyHandler(parameters);
            }
        }

        public void JoinedPartyHandler(Dictionary<byte, object> parameters)
        {

            dynamic playerName = new Object();
            dynamic playerId = new Object();
            parameters.TryGetValue(1, out playerId);
            parameters.TryGetValue(2, out playerName);
            this.partyService.AddPlayerToParty(playerId, playerName.ToString());
        }

        public void CreatePartyHandler(Dictionary<byte, object> parameters)
        {
            dynamic playerArrayId = parameters[4];
            var player1Id = playerArrayId[0];
            var player2Id = playerArrayId[1];

            dynamic playerArrayNames = parameters[5];
            var player1Name = playerArrayNames[0];
            var player2Name = playerArrayNames[1];

            this.partyService.CreateParty(player1Id, player1Name, player2Id, player2Name);
        }

        public void LeftPartyHandler(Dictionary<byte, object> parameters)
        {
            if (parameters.Count == 1)//player deny invite.
            {
                return;
            }
            else if (parameters.Count == 2)//set party as closed
            {
                this.partyService.PlayerCloseParty();
            }
            else if (parameters.Count == 3)//specific player left party.
            {
                dynamic partyMemberByteArray = new Object();
                parameters.TryGetValue(1, out partyMemberByteArray);
                this.partyService.PlayerLeaveParty(partyMemberByteArray);
            }

        }
    }
}
