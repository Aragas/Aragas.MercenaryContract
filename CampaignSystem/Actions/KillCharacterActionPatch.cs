﻿using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    [HarmonyPatch(typeof(KillCharacterAction))]
    [HarmonyPatch("ApplyInternal")]
    public class KillCharacterActionPatch
    {
        public static void Postfix(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail actionDetail, bool showNotification)
        {
            if (victim.IsAlive || actionDetail != KillCharacterAction.KillCharacterActionDetail.Executed)
                return;

            var isPlayer = killer == Clan.PlayerClan.Leader;

            var multiplier = killer.Clan.IsUnderMercenaryService 
                ? MercenaryContractOptions.Instance.MercenaryMultiplier 
                : MercenaryContractOptions.Instance.VassalMultiplier;

            var contractorKingdom = killer.Clan.Kingdom;

            // If victim was an enemy of a hero of ContractorClan, increase relationship with hero
            foreach (var contractorKingdomHero in contractorKingdom.Heroes)
            {
                if (victim.IsEnemy(contractorKingdomHero))
                {
                    AragasChangeRelationAction.ApplyRelation(
                        killer,
                        contractorKingdomHero,
                        5 * multiplier,
                        MercenaryContractOptions.Instance.EnemyCap,
                        true && isPlayer);
                }
            }

            // If victim was a friend of a hero of ContractorClan, decrease relationship with hero
            // ALREADY IN VANILLA

            // If victim had negative honor, increase relationship with ContractorClan
            var victimHonor = victim.GetTraitLevel(DefaultTraits.Honor);
            if (victimHonor < 0)
            {
                foreach (var contractorKingdomHero in contractorKingdom.Heroes)
                {
                    var contractorKingdomHeroHonor = victim.GetTraitLevel(DefaultTraits.Honor);
                    if (contractorKingdomHeroHonor > 0)
                    {
                        AragasChangeRelationAction.ApplyRelation(
                            killer,
                            contractorKingdomHero,
                            2 * contractorKingdomHeroHonor * multiplier,
                            MercenaryContractOptions.Instance.TraitCap,
                            false && isPlayer);
                    }
                }
            }

            // If victim was merciful, decrease relationship with ContractorClan?
            var victimMercy = victim.GetTraitLevel(DefaultTraits.Mercy);
            if (victimMercy > 0)
            {
                foreach (var contractorKingdomHero in contractorKingdom.Heroes)
                {
                    var contractorKingdomHeroMercy = victim.GetTraitLevel(DefaultTraits.Mercy);
                    if (contractorKingdomHeroMercy > 0)
                    {
                        AragasChangeRelationAction.ApplyRelation(
                            killer,
                            contractorKingdomHero,
                            -2 * contractorKingdomHeroMercy * multiplier,
                            -MercenaryContractOptions.Instance.TraitCap,
                            false && isPlayer);
                    }
                }
            }
        }
    }
}