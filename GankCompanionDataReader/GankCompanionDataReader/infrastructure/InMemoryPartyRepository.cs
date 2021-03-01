using GankCompanionDataReader.eventHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.infrastructure
{
    public class InMemoryPartyRepository : IPartyRepository
    {
        private Guid partyId;
        private string pary;
        public InMemoryPartyRepository()
        {
            this.pary = "test";
            this.partyId = Guid.NewGuid();
            
        }

        public Guid GetPartyID()
        {
            return this.partyId;
        }

        public string GetPartyIDString()
        {
            return this.partyId.ToString();
        }

        public void SetPartyID(string partyId)
        {
            this.partyId = new Guid(partyId);
        }
    }
}
