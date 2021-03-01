using Microsoft.VisualStudio.TestTools.UnitTesting;
using GankCompanionDataReader.eventHandler.party;
using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.eventHandler.party.Tests
{
    [TestClass()]
    public class PartyServiceTests
    {
        [TestMethod()]
        public void ConvertByteArrayToStringTest()
        {
            PartyApiService gg = new PartyApiService();
            var ggg = gg.ConvertByteArrayToString(null);
            var ff = 0;
        }
    }
}