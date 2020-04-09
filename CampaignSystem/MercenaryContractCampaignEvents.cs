using Aragas.MountAndBlade;

using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractCampaignEvents
    {
        private static MethodInfo CalculateInfluenceChangeInvokeMethod { get; } =
            typeof(MbEvent<RefContainer<float>, Clan, StatExplainer>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo BattleEndedInvokeMethod { get; } =
            typeof(MbEvent<MapEvent, CampaignTime>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);

        public static MercenaryContractCampaignEvents Instance => MercenaryContractSubModule.Current.CampaignEvents;

        public static IMbEvent<MapEvent, CampaignTime> BattleEnded => Instance._battleEnded;
        public static IMbEvent<RefContainer<float>, Clan, StatExplainer> CalculateInfluenceChange => Instance._calculateInfluenceChange;

        private readonly MbEvent<RefContainer<float>, Clan, StatExplainer> _calculateInfluenceChange = new MbEvent<RefContainer<float>, Clan, StatExplainer>();
        private readonly MbEvent<MapEvent, CampaignTime> _battleEnded = new MbEvent<MapEvent, CampaignTime>();

        internal void OnCalculateInfluenceChange(RefContainer<float> influence, Clan clan, StatExplainer explainer)
        {
            CalculateInfluenceChangeInvokeMethod.Invoke(_calculateInfluenceChange, new object[] { influence, clan, explainer });
        }
        internal void OnBattleEnded(MapEvent battle, CampaignTime campaignTime)
        {
            BattleEndedInvokeMethod.Invoke(_battleEnded, new object[] { battle, campaignTime });
        }
    }
}