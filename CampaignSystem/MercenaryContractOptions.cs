using Aragas.MountAndBlade;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractOptions
    {
        public static MercenaryContractOptions Instance => MercenaryContractSubModule.Current.Options;

        /// <summary>
        /// Relationship multiplier when Hero is independent
        /// </summary>
        public int IndependentMultiplier { get; } = 1;

        /// <summary>
        /// Relationship multiplier when Hero is a mercenary
        /// </summary>
        public int MercenaryMultiplier { get; } = 1;

        /// <summary>
        /// Relationship multiplier when Hero is a vassal
        /// </summary>
        public int VassalMultiplier { get; } = 2;


        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at peace
        /// </summary>
        public int LeavingLossPeace { get; } = -5;

        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at war
        /// </summary>
        public int LeavingLossWar { get; } = -20;


        /// <summary>
        /// Maximum relationship to increase to when releasing a prisoner after battle
        /// </summary>
        public int ReleaseAfterBattleCap { get; } = 10;

        /// <summary>
        /// Maximum relationship to increase to when killing an enemy of a Hero
        /// </summary>
        public int EnemyCap { get; } = 25;

        /// <summary>
        /// Maximum relationship to increase to when killing a Hero of opposite trait
        /// </summary>
        public int TraitCap { get; } = 10;


        /// <summary>
        /// Maximum relationship to decrease to when attacking an enemy of the Kingdom
        /// </summary>
        public int AttackedCap { get; } = -10;

        /// <summary>
        /// Maximum relationship to decrease to when attacking an enemy of the Kingdom
        /// </summary>
        public int HelpedDefenderCap { get; } = 10;

        /// <summary>
        /// Minimum amount battles needed to perform to avoid influence penalty
        /// </summary>
        public int MinimumBattleCount { get; } = 10;

        /// <summary>
        /// Length of a Mercenary Contract
        /// </summary>
        public int ContractLengthInDays { get; } = 30;

        /// <summary>
        /// How much days a clan has to perform the minimum amounth of battles per contract before the penalty kicks in. 
        /// </summary>
        public int DaysBeforeInfluencePenalty { get; } = 7;

        /// <summary>
        /// Influence penalty for not performing the minimum amount of battles per contract.
        /// </summary>
        public int InfluencePenalty { get; } = -1;
    }
}