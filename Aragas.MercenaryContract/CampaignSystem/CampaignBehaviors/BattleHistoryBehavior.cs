using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.CampaignBehaviors
{
    public class BattleHistoryBehavior : CampaignBehaviorBase
    {
        private List<BattleHistoryEntry> _currentMonthBattleHistories = new List<BattleHistoryEntry>();
        private Dictionary<MapEvent, List<PartyBase>> _partiedJoinedMapEvent = new Dictionary<MapEvent, List<PartyBase>>(new MapEventComparer());

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("partiedJoinedMapEvent", ref _partiedJoinedMapEvent);
            dataStore.SyncData("battleHistories", ref _currentMonthBattleHistories);
        }

        public override void RegisterEvents()
        {
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
            MercenaryContractCampaignEvents.CalculateInfluenceChange.AddNonSerializedListener(this, OnCalculateInfluenceChange);
            CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, OnWeeklyTick);
        }
        private void OnCalculateInfluenceChange(RefContainer<float> influence, Clan clan, StatExplainer explanation)
        {
            if (clan.IsUnderMercenaryService && !IsContributingToWar(clan))
            {
                var penalty = MercenarySettings.Instance.InfluencePenalty;

                influence.Value += penalty;
                // Null check is critical, explanation can be null sometimes.
                explanation?.AddLine(GameTexts.FindText("str_mercenary_contract_not_contributing_to_war", null).ToString(), penalty);
            }
        }
        private void OnMapEventEnded(MapEvent mapEvent)
        {
            if(mapEvent.EventType == MapEvent.BattleTypes.None)
                return;

            _currentMonthBattleHistories.Add(new BattleHistoryEntry(mapEvent, CampaignTime.Now));
        }
        private void OnWeeklyTick()
        {
            var toRemove = new List<BattleHistoryEntry>();
            foreach (var battleHistoryEntry in _currentMonthBattleHistories)
            {
                if (battleHistoryEntry.Time.ElapsedDaysUntilNow >= MercenarySettings.Instance.ContractLengthInDays)
                    toRemove.Add(battleHistoryEntry);
            }
            foreach (var entry in toRemove)
                _currentMonthBattleHistories.Remove(entry);
            toRemove.Clear();
        }

        private bool IsContributingToWar(Clan mercenaryClan)
        {
            if (!MercenarySettings.Instance.ApplyRelationshipRulesToNPC && mercenaryClan == Clan.PlayerClan)
                return true;

            var isAtWar = FactionManager.GetEnemyFactions(mercenaryClan.Kingdom).Any();
            if (isAtWar)
            {
                var mercenary = mercenaryClan.Leader;
                var mercenaryFaction = mercenary.MapFaction;

                if (mercenaryClan.LastFactionChangeTime.ElapsedDaysUntilNow >= MercenarySettings.Instance.DaysBeforeInfluencePenalty)
                {
                    var days = MercenaryManager.DaysAfterContractStartedOrRenewed(mercenaryClan);
                    return _currentMonthBattleHistories
                        .Where(b => b.Time.ElapsedDaysUntilNow < days)
                        .Count(b =>
                        {
                            if (b.Attacker?.LeaderHero?.MapFaction == null || b.Defender?.LeaderHero?.MapFaction == null)
                                return false;

                            var flag1 = mercenary == b.Attacker.LeaderHero && mercenaryFaction.IsAtWarWith(b.Defender.LeaderHero.MapFaction);
                            var flag2 = mercenary == b.Defender.LeaderHero && mercenaryFaction.IsAtWarWith(b.Attacker.LeaderHero.MapFaction);
                            return flag1 || flag2;
                        }) >= MercenarySettings.Instance.MinimumBattleCount;
                }
                else
                    return true;
            }
            else
                return true;
        }
    }
}