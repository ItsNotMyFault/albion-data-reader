using GankCompanion_backend.applicationserivce;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace GankCompanionDataReader.eventHandler.party
{
    public class PartyApiService
    {
        private HttpClient httpClient;
        private static Guid partyId;
        private readonly string URL;
        private readonly IPartyRepository partyRepository;
        public PartyApiService(string apiURL, IPartyRepository partyRepository)
        {
            this.partyRepository = partyRepository;
            this.URL = apiURL;
            httpClient = new HttpClient();
        }

        public void AddPlayerToParty(byte[] playerByteArray, string playerName)
        {
            PartyJoinRequest partyJoinRequest = new PartyJoinRequest
            {
                PartyId = partyRepository.GetPartyIDString(),
                PlayerJoinedId = ConvertByteArrayToString(playerByteArray),
                PlayerJoinedName = playerName,
            };
            string json2 = JsonConvert.SerializeObject(partyJoinRequest);

            StringContent content = new StringContent(json2, Encoding.UTF8, "application/json");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("contentType", "application/json");
            this.httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            this.httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

            string postUrl = URL + "/JoinedParty";
            HttpResponseMessage response = null;
            try
            {
                response = this.httpClient.PostAsync(postUrl, content).Result;
            }
            catch (Exception e)
            {
                var ff = e;
            }

            string responseString = response.Content.ReadAsStringAsync().Result;
            Object accessTokenResponse = JsonConvert.DeserializeObject<Object>(responseString);
        }

        private string ConvertByteArrayToString(byte[] buffer)
        {
            string result = "";
            try
            {
                result = string.Join(";", buffer);
                string[] str = result.Split(";");

                //to convertback
                int[] myInts = str.Select(int.Parse).ToArray();
            }
            catch (Exception e)
            {
                var ff = 0;
            }


            return result;
        }

        public void CreateParty(byte[] player1Id, string playerName, byte[] player2Id, string player2Name)
        {

            string partyId = Guid.NewGuid().ToString();
            this.partyRepository.SetPartyID(partyId);

            PartyCreationRequest partyCreationRequest = new PartyCreationRequest
            {
                PartyId = partyId.ToString(),
                Player1Id = ConvertByteArrayToString(player1Id),
                Player1Name = playerName,
                Player2Id = ConvertByteArrayToString(player2Id),
                Player2Name = player2Name,
                PartyStartTime = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")
            };

            string json2 = JsonConvert.SerializeObject(partyCreationRequest);

            StringContent content = new StringContent(json2, Encoding.UTF8, "application/json");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("contentType", "application/json");
            this.httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            this.httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpResponseMessage response = null;
            string postUrl = URL + "/CreateParty";
            try
            {
                response = this.httpClient.PostAsync(postUrl, content).Result;
            }
            catch (Exception e)
            {
                var ff = e;
            }

            string responseString = response.Content.ReadAsStringAsync().Result;
            Object accessTokenResponse = JsonConvert.DeserializeObject<Object>(responseString);
            var test = 0;
        }

        public void PlayerLeaveParty(byte[] playerId)
        {
            var test = partyRepository.GetPartyIDString();
            PartyLeftRequest partyLeftRequest = new PartyLeftRequest
            {
                PartyId = partyId.ToString(),
                PlayerLeftId = ConvertByteArrayToString(playerId)
            };
 
            string json2 = JsonConvert.SerializeObject(partyLeftRequest);

            StringContent content = new StringContent(json2, Encoding.UTF8, "application/json");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("contentType", "application/json");
            this.httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            this.httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpResponseMessage response = null;

            string postUrl = URL + "/LeftParty";
            try
            {
                response = this.httpClient.PostAsync(postUrl, content).Result;
            }
            catch (Exception e)
            {
                var ff = e;
            }
            string responseString = response.Content.ReadAsStringAsync().Result;
            Object accessTokenResponse = JsonConvert.DeserializeObject<Object>(responseString);
        }

        public void PlayerCloseParty()
        {
            PartyCloseRequest partyCloseRequest = new PartyCloseRequest
            {
                PartyId = partyRepository.GetPartyIDString(),
            };

            string json2 = JsonConvert.SerializeObject(partyCloseRequest);

            StringContent content = new StringContent(json2, Encoding.UTF8, "application/json");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("contentType", "application/json");
            this.httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            this.httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpResponseMessage response = null;

            string postUrl = URL + "/CloseParty";
            try
            {
                response = this.httpClient.PostAsync(postUrl, content).Result;
            }
            catch (Exception e)
            {
                var ff = e;
            }
            string responseString = response.Content.ReadAsStringAsync().Result;
            Object accessTokenResponse = JsonConvert.DeserializeObject<Object>(responseString);
        }
    }
}
