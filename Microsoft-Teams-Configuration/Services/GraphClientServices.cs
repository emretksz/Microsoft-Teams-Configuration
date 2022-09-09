using Azure.Identity;
using Microsoft.Graph;

namespace MicrosoftTeams_Configuration_ASPCORE.Services
{
    public class GraphClientServices
    {

        public GraphClientServices()
        {
                
        }
        #region Crud Client
        public async Task<OnlineMeeting> CreateOnlineMeeting(OnlineMeeting onlineMeeting)
        {
            var graphServiceClient = GetGraphClient();
            var userId = await GetUserIdAsync();
            return await graphServiceClient.Users[userId].OnlineMeetings.Request().AddAsync(onlineMeeting);
        }
        public async Task<OnlineMeeting> UpdateOnlineMeeting(OnlineMeeting onlineMeeting)
        {
            var graphServiceClient = GetGraphClient();
            var userId = await GetUserIdAsync();
            var oldMeeting = await graphServiceClient.Users[userId].OnlineMeetings[onlineMeeting.Id].Request().GetAsync();

            //example...
            var onlineMeetingg = new OnlineMeeting()
            {
                StartDateTime = onlineMeeting.StartDateTime,
                EndDateTime = onlineMeeting.EndDateTime,
                Subject = onlineMeeting.Subject,
            };

            var meetingAttendees = new List<MeetingParticipantInfo>();

            meetingAttendees.Add(new MeetingParticipantInfo
            {
                Upn = "emre@outlook.com",
            });

            if (onlineMeetingg.Participants == null)
            {
                onlineMeetingg.Participants = new MeetingParticipants();
            };
            onlineMeetingg.Participants.Attendees = meetingAttendees;

            var result = await graphServiceClient.Users[userId].OnlineMeetings[oldMeeting.Id].Request().UpdateAsync(onlineMeetingg);
            return result;
        }
        public async Task<bool> DeleteOnlineMeeting(string Id)
        {
            try
            {
                var graphServiceClient = GetGraphClient();
                var userId = await GetUserIdAsync();
                await graphServiceClient.Users[userId].OnlineMeetings[Id].Request().DeleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<OnlineMeeting> AddParticipant(OnlineMeeting onlineMeeting,string meetingId)
        {
            var graphServiceClient = GetGraphClient();
            var userId = await GetUserIdAsync();
            var oldMeeting = await graphServiceClient.Users[userId].OnlineMeetings[meetingId].Request().GetAsync();
            //example...
            OnlineMeeting update = new OnlineMeeting();

            if (update.Participants == null)
            {
                update.Participants = new MeetingParticipants();
            };
            update.Participants.Attendees = onlineMeeting.Participants.Attendees;

            var result = await graphServiceClient.Users[userId].OnlineMeetings[oldMeeting.Id].Request().UpdateAsync(update);
            return result;
        }

        #endregion

        #region Get
        public async Task<OnlineMeeting> GetOnlineMeeting(string onlineMeetingId)
        {
            var graphServiceClient = GetGraphClient();
            var userId = await GetUserIdAsync();
 
            var result = await graphServiceClient.Users[userId].OnlineMeetings[onlineMeetingId].Request().GetAsync();
            return result;
        }
        private async Task<string> GetUserIdAsync()
        {
            var meetingOrganizer = "";
            /// creator app user or permission app admin :)
            var filter = $"startswith(userPrincipalName,'{meetingOrganizer}')";
            var graphServiceClient = GetGraphClient();
            var users = await graphServiceClient.Users.Request().Filter(filter).GetAsync();
            return users.CurrentPage[0].Id;
        }



        private GraphServiceClient GetGraphClient()
        {
            string[] scopes = new[] { "https://graph.microsoft.com/.default" };

            //Use azure application, create new app and enter the required properties 
            var tenantId = "tenant id";
            var clientId = "app client Id";
            var clientSecret = " app Secret key";
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            var clientSecretCredential = new ClientSecretCredential(
                   tenantId, clientId, clientSecret, options);

            return new GraphServiceClient(clientSecretCredential, scopes);

        }
        #endregion

    }
}
