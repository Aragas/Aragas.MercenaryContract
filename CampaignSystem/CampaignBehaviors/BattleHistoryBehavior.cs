using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.CampaignBehaviors
{
    public class BattleHistoryBehavior : CampaignBehaviorBase
    {
        private List<BattleHistoryEntry> _currentMonthBattleHistories = new List<BattleHistoryEntry>();
        
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("battleHistories", ref _currentMonthBattleHistories);
        }

        public override void RegisterEvents()
        {
            MercenaryContractCampaignEvents.BattleEnded.AddNonSerializedListener(this, OnBattleEnded);
            MercenaryContractCampaignEvents.CalculateInfluenceChange.AddNonSerializedListener(this, OnCalculateInfluenceChange);
            CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, OnWeeklyTick);
        }
        private void OnCalculateInfluenceChange(RefContainer<float> influence, Clan clan, StatExplainer explanation)
        {
            if (clan.IsUnderMercenaryService && !IsContributingToWar(clan))
            {
                var penalty = MercenaryContractOptions.Instance.InfluencePenalty;

                influence.Value += penalty;
                // Null check is critical, explanation will be null sometimes.
                explanation?.AddLine(GameTexts.FindText("str_mercenary_contract_not_contributing_to_war", null).ToString(), penalty);
            }
        }
        private void OnBattleEnded(MapEvent battle, CampaignTime time)
        {
            _currentMonthBattleHistories.Add(new BattleHistoryEntry(battle, time));
        }
        private void OnWeeklyTick()
        {
            var toRemove = new List<BattleHistoryEntry>();
            foreach (var battleHistoryEntry in _currentMonthBattleHistories)
            {
                if (battleHistoryEntry.Time.ElapsedDaysUntilNow >= MercenaryContractOptions.Instance.ContractLengthInDays)
                    toRemove.Add(battleHistoryEntry);
            }
            foreach (var entry in toRemove)
                _currentMonthBattleHistories.Remove(entry);
            toRemove.Clear();
        }

        private bool IsContributingToWar(Clan mercenaryClan)
        {
            var isAtWar = FactionManager.GetEnemyFactions(mercenaryClan.Kingdom).Any();

            if (isAtWar)
            {
                var mercenary = Clan.PlayerClan.Leader;
                var mercenaryFaction = mercenary.MapFaction;

                if (mercenaryClan.LastFactionChangeTime.ElapsedDaysUntilNow >= 7F)
                {
                    var days = MercenaryManager.DaysAfterContractStartedOrRenewed(mercenaryClan);
                    return _currentMonthBattleHistories
                        .Where(b => b.Time.ElapsedDaysUntilNow < days)
                        .Count(b =>
                        {
                            var flag1 = mercenary == b.Attacker.LeaderHero && mercenaryFaction.IsAtWarWith(b.Defender.LeaderHero.MapFaction);
                            var flag2 = mercenary == b.Defender.LeaderHero && mercenaryFaction.IsAtWarWith(b.Attacker.LeaderHero.MapFaction);
                            return flag1 || flag2;
                        }) >= MercenaryContractOptions.Instance.MinimumBattleCount;
                }
                else
                    return true;
            }
            else
                return true;
        }
    }
}