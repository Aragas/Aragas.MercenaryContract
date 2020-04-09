using Aragas.MountAndBlade;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractOptions
    {
        public static MercenaryContractOptions Instance => MercenaryContractSubModule.Current.Options;

        public int IndependentMultiplier = 1;
        public int MercenaryMultiplier = 1;
        public int VassalMultiplier = 2;

        public int LeavingLossPeace = -5;
        public int LeavingLossWar = -20;

        public int EnemyCap = 25;
        public int TraitCap = 10;

        public int MinimumBattleCount = 10;

        public int ContractLengthInDays = 30;

        public int InfluencePenalty = -1;
    }
}