using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Aragas.CampaignSystem.CampaignBehaviors
{
    // TODO: * If you attack groups larger than you, the same thing with the [Daring] trait.
    // TODO: * Helping out other parties in battles, positive rep with people with the [Generous] trait.
    public class BattleBehavior : CampaignBehaviorBase
    {
        public override void SyncData(IDataStore dataStore) { }

        public override void RegisterEvents()
        {
            MercenaryContractCampaignEvents.BattleEnded.AddNonSerializedListener(this, OnBattleEnded);
        }
        private void OnBattleEnded(MapEvent battle, CampaignTime endingTime)
        {
            var attacker = battle.GetLeaderParty(BattleSideEnum.Attacker);
            var defender = battle.GetLeaderParty(BattleSideEnum.Defender);

            var attackers = battle.PartiesOnSide(BattleSideEnum.Attacker).Where(a => a != attacker).ToList();
            var defenders = battle.PartiesOnSide(BattleSideEnum.Defender).Where(d => d != defender).ToList();

            switch (battle.WinningSide)
            {
                case BattleSideEnum.Attacker:
                    break;
                case BattleSideEnum.Defender:
                    break;
                case BattleSideEnum.None:
                case BattleSideEnum.NumSides:
                    break;
            }
        }
    }
}