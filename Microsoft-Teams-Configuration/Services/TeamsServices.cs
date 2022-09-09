using Microsoft.Graph;
using MicrosoftTeams_Configuration_ASPCORE.Models;

namespace MicrosoftTeams_Configuration_ASPCORE.Services
{
    public class TeamsServices
    {
        private readonly GraphClientServices _graphClientServices;
        public TeamsServices(GraphClientServices graphClientServices)
        {
            _graphClientServices = graphClientServices;
        }
        public async Task <OnlineMeeting> CreateTeamsMeeting( string meeting, DateTimeOffset begin,DateTimeOffset end)
        {
            var onlineMeeting = new OnlineMeeting
            {
                StartDateTime = begin,
                EndDateTime = end,
                Subject = meeting,
                
                //Default Lobby Setting...
                LobbyBypassSettings = new LobbyBypassSettings
                {
                    Scope = LobbyBypassScope.Everyone
                },
            };
         var result = await _graphClientServices.CreateOnlineMeeting(onlineMeeting);
            return result;
        }

        public async  Task<OnlineMeeting> AddMeetingParticipants(string meetingId, List<string> attendees)
        {
            OnlineMeeting onlineMeeting = new OnlineMeeting();
            var meetingAttendees = new List<MeetingParticipantInfo>();
            foreach (var attendee in attendees)
            {
                if (!string.IsNullOrEmpty(attendee))
                {
                        meetingAttendees.Add(new MeetingParticipantInfo
                        {
                            Upn = attendee.Trim(),
                            Role = OnlineMeetingRole.Coorganizer,
                            Identity = new IdentitySet
                            {
                                User = new Identity
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    DisplayName = attendee.Trim(),
                                }
                            }
                        });
                }
            }

            if (onlineMeeting.Participants == null)
            {
                onlineMeeting.Participants = new MeetingParticipants();
            };
            onlineMeeting.Participants.Attendees = meetingAttendees;

           var result = await _graphClientServices.AddParticipant(onlineMeeting,meetingId);
            return result;
        }

        public async Task<bool>UpdateMeeting(TeamsConfigurationDTO dto)
        {
            OnlineMeeting meeting = new OnlineMeeting
            {
                Id = dto.MeetingId,
                StartDateTime = dto.Start,
                EndDateTime = dto.End,
                Subject = dto.Subject,
                AllowedPresenters = dto.AllowIsPresent,
                // LobbyBypassSettings= new LobbyBypassSettings {Scope = LobbyBypassScope.Everyone },
            };
            var result= await _graphClientServices.UpdateOnlineMeeting(meeting);
            // more controll ... 
            return true;

        }
        public async Task<bool> Delete(string meetingId)
        {
            
            var result = await _graphClientServices.DeleteOnlineMeeting(meetingId);
            if (!result)
                return true;
            return false;
       ;

        }
    }
}
