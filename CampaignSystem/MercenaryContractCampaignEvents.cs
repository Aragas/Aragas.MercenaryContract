using Aragas.MountAndBlade;

using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractCampaignEvents
    {
        private static MethodInfo CalculateInfluenceChangeInvokeMethod { get; } =
            typeof(MbEvent<RefContainer<float>, Clan, StatExplainer>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo PartyJoinedMapEventInvokeMethod { get; } =
            typeof(MbEvent<MapEvent, PartyBase>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);

        public static MercenaryContractCampaignEvents Instance => MercenaryContractSubModule.Current.CampaignEvents;

        public static IMbEvent<RefContainer<float>, Clan, StatExplainer> CalculateInfluenceChange => Instance._calculateInfluenceChange;
        public static IMbEvent<MapEvent, PartyBase> PartyJoinedMapEvent => Instance._partyJoinedMapEvent;

        private readonly MbEvent<RefContainer<float>, Clan, StatExplainer> _calculateInfluenceChange = new MbEvent<RefContainer<float>, Clan, StatExplainer>();
        private readonly MbEvent<MapEvent, PartyBase> _partyJoinedMapEvent = new MbEvent<MapEvent, PartyBase>();
        
        internal void OnCalculateInfluenceChange(RefContainer<float> influence, Clan clan, StatExplainer explainer)
        {
            CalculateInfluenceChangeInvokeMethod.Invoke(_calculateInfluenceChange, new object[] { influence, clan, explainer });
        }
        internal void OnPartyJoinedMapEvent(MapEvent mapEvent, PartyBase partyBase)
        {
            PartyJoinedMapEventInvokeMethod.Invoke(_calculateInfluenceChange, new object[] { mapEvent, partyBase });
        }
    }
}