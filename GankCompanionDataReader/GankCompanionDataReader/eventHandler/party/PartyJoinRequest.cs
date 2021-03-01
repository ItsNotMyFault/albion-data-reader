using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.eventHandler.party
{
    public class PartyJoinRequest
    {

        public string PartyId { get; set; }
        public string PlayerJoinedId { get; set; }
        public string PlayerJoinedName { get; set; }

        public PartyJoinRequest()
        {
        }
    }
}
