using System.Linq;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem.Extensions
{
    public static class MapEventSideExtensions
    {
        public static int CalculateTotalContribution(this MapEventSide instance)
        {
            var num = 0;
            foreach (var mapEventParty in instance.PartyRecs)
            {
                if (mapEventParty.Party.MemberRoster.Count > 0)
                {
                    num += mapEventParty.ContributionToBattle;
                }
            }
            return num;
        }

        public static float GetPartyContributionRate(this MapEventSide instance, PartyBase partyBase)
        {
            var totalContribution = instance.CalculateTotalContribution();
            if (totalContribution == 0)
                return 0f;

            var partyContribution = instance.PartyRecs.FirstOrDefault(p => p.Party == partyBase)?.ContributionToBattle ?? 0;
            return (float) partyContribution / (float) totalContribution;
        }
    }
}