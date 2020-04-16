using Aragas.CampaignSystem.Actions;
using Aragas.CampaignSystem.Extensions;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.CampaignBehaviors
{
    public class BattleBehavior : CampaignBehaviorBase
    {
        private Dictionary<MapEvent, List<PartyBase>> _partiedJoinedMapEvent = new Dictionary<MapEvent, List<PartyBase>>(new MapEventComparer());

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("partiedJoinedMapEvent", ref _partiedJoinedMapEvent);
        }

        public override void RegisterEvents()
        {
            MercenaryContractCampaignEvents.PartyJoinedMapEvent.AddNonSerializedListener(this, OnPartyJoinedMapEvent);
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
        }
        private void OnPartyJoinedMapEvent(MapEvent mapEvent, PartyBase partyBase)
        {
            if (!_partiedJoinedMapEvent.ContainsKey(mapEvent))
                _partiedJoinedMapEvent[mapEvent] = new List<PartyBase>();

            _partiedJoinedMapEvent[mapEvent].Add(partyBase);
        }
        private void OnMapEventEnded(MapEvent mapEvent)
        {
            if (mapEvent.EventType == MapEvent.BattleTypes.None)
                return;

            var attacker = mapEvent.GetLeaderParty(BattleSideEnum.Attacker);
            var defender = mapEvent.GetLeaderParty(BattleSideEnum.Defender);

            var attackers = mapEvent.PartiesOnSide(BattleSideEnum.Attacker).Where(a => a != attacker).ToList();
            var defenders = mapEvent.PartiesOnSide(BattleSideEnum.Defender).Where(d => d != defender).ToList();

            switch (mapEvent.WinningSide)
            {
                // If you attack groups larger than you, the same thing with the [Daring] trait.
                case BattleSideEnum.Attacker:
                {
                    var hero = attacker.LeaderHero;
                    if (hero == null)
                        break;

                    var multiplier = hero.Clan.IsUnderMercenaryService
                        ? MercenaryContractOptions.MercenaryMultiplier
                        : MercenaryContractOptions.VassalMultiplier;

                    var ratio = (mapEvent.StrengthOfSide[0] / mapEvent.StrengthOfSide[1]) - 1f;

                    var value = 0;
                    if (ratio >= 0.33f)
                        value = 1;
                    else if (ratio >= 0.66f)
                        value = 2;
                    else if (ratio >= 0.99f)
                        value = 3;
                    if (value == 0)
                        break;

                    foreach (var kingdomHero in hero.Clan.Kingdom.Heroes)
                    {
                        var valor = kingdomHero.GetTraitLevel(DefaultTraits.Valor);
                        if (valor > 0)
                        {
                            AragasChangeRelationAction.ApplyRelation(
                                hero,
                                kingdomHero,
                                value * multiplier * valor,
                                MercenaryContractOptions.TraitCap,
                                true);
                        }
                    }

                    break;
                }
                // Helping out other parties in battles, positive rep with people with the [Generous] trait.
                case BattleSideEnum.Defender:
                {
                    var ratio = (attacker.TotalStrength / defender.TotalStrength) - 1f;

                    var value = 0;
                    if (ratio >= 0.33f)
                        value = 1;
                    else if (ratio >= 0.66f)
                        value = 2;
                    else if (ratio >= 0.99f)
                        value = 3;
                    if (value == 0)
                        break;

                    if (_partiedJoinedMapEvent.TryGetValue(mapEvent, out var parties))
                    {
                        foreach (var partyBase in parties)
                        {
                            var joinedHero = partyBase.LeaderHero;
                            if (joinedHero == null)
                                continue;

                            var contributionRate = mapEvent.AttackerSide.GetPartyContributionRate(partyBase);
                            if (contributionRate < 0.3f)
                                continue;

                            var multiplier = joinedHero.Clan.IsUnderMercenaryService
                                ? MercenaryContractOptions.MercenaryMultiplier
                                : MercenaryContractOptions.VassalMultiplier;

                            var otherHeroesNotJoined = defenders
                                .Where(p => !parties.Contains(p))
                                .Select(p => p.LeaderHero)
                                .Where(h => h != null)
                                .ToList();

                            foreach (var notJoinedHero in otherHeroesNotJoined)
                            {
                                var generosity = notJoinedHero.GetTraitLevel(DefaultTraits.Generosity);
                                if (generosity > 0)
                                {
                                    AragasChangeRelationAction.ApplyRelation(
                                        joinedHero,
                                        notJoinedHero,
                                        value * multiplier * generosity,
                                        MercenaryContractOptions.HelpedDefenderCap,
                                        true);
                                }
                            }
                        }
                    }
                    break;
                }
                case BattleSideEnum.None:
                case BattleSideEnum.NumSides:
                    break;
            }

            // Clean up storage after event ended
            if (_partiedJoinedMapEvent.TryGetValue(mapEvent, out var list))
            {
                _partiedJoinedMapEvent.Remove(mapEvent);
                list.Clear();
                list = null;
            }
        }
        private void OnDailyTick()
        {
            // Clean up storage if somehow there are already ended battles
            var toRemove = new List<MapEvent>();
            foreach (var (mapEvent, list) in _partiedJoinedMapEvent.Select(x => (x.Key, x.Value)))
            {
                if (mapEvent.IsFinalized && mapEvent.BattleStartTime.ElapsedDaysUntilNow > 7f)
                    toRemove.Add(mapEvent);
            }
            foreach (var mapEvent in toRemove)
            {
                if (_partiedJoinedMapEvent.TryGetValue(mapEvent, out var list))
                {
                    _partiedJoinedMapEvent.Remove(mapEvent);
                    list.Clear();
                    list = null;
                }
            }
            toRemove.Clear();
        }
    }
}