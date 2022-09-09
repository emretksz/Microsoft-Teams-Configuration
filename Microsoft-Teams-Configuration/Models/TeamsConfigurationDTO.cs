using Microsoft.Graph;

namespace MicrosoftTeams_Configuration_ASPCORE.Models
{
    public class TeamsConfigurationDTO
    {
        public string MeetingId { get; set; }
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public LobbyBypassSettings LobbyBypassSettings { get; set; }
        public OnlineMeetingPresenters AllowIsPresent { get; set; }
  
    }
}
