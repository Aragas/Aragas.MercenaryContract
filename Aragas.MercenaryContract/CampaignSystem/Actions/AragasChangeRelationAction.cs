using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace Aragas.CampaignSystem.Actions
{
    public class AragasChangeRelationAction
    {
        private static void ApplyInternal(Hero hero, Hero toHero, int relation, int cap, bool showQuickNotification = true)
        {
            if(!MercenarySettings.Instance!.ApplyRelationshipRulesToNPC && hero != Hero.MainHero)
                return;

            //var contractorKingdomHeroRelation = hero.GetRelation(toHero);
            var contractorKingdomHeroRelation = CharacterRelationManager.GetHeroRelation(hero, toHero);
            if (contractorKingdomHeroRelation + relation < cap)
            {
                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, toHero, relation, showQuickNotification && hero == Hero.MainHero);
            }
        }

        public static void ApplyRelation(Hero hero, Hero toHero, int relation, int cap, bool showQuickNotification = true)
        {
            ApplyInternal(hero, toHero, relation, cap, showQuickNotification);
        }
    }
}