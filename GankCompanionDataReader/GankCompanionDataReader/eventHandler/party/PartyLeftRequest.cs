using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.eventHandler.party
{
    public class PartyLeftRequest
    {
        public string PartyId { get; set; }
        public string PlayerLeftId { get; set; }
        public PartyLeftRequest() { }
    }
}
