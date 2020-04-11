using Aragas.MountAndBlade;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractCampaignEvents
    {
        public static MercenaryContractCampaignEvents Instance => MercenaryContractSubModule.Current.CampaignEvents;

        public static IMbEvent<RefContainer<float>, Clan, StatExplainer> CalculateInfluenceChange => Instance._calculateInfluenceChange;
        public static IMbEvent<MapEvent, PartyBase> PartyJoinedMapEvent => Instance._partyJoinedMapEvent;

        private readonly AragasMbEvent<RefContainer<float>, Clan, StatExplainer> _calculateInfluenceChange = new AragasMbEvent<RefContainer<float>, Clan, StatExplainer>();
        private readonly AragasMbEvent<MapEvent, PartyBase> _partyJoinedMapEvent = new AragasMbEvent<MapEvent, PartyBase>();
        
        internal void OnCalculateInfluenceChange(RefContainer<float> influence, Clan clan, StatExplainer explainer)
        {
            _calculateInfluenceChange.Invoke(influence, clan, explainer);
        }
        internal void OnPartyJoinedMapEvent(MapEvent mapEvent, PartyBase partyBase)
        {
            _partyJoinedMapEvent.Invoke(mapEvent, partyBase);
        }
    }
}