using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;

namespace Aragas
{
    public class MercenarySettings : AttributeSettings<MercenarySettings>
    {
        public override string ModName => "Limited Mercenary Contract";
        public override string ModuleFolderName => "Aragas.MercenaryContract";

        public override string Id { get; set; } = "Aragas.MercenaryContract_v1";


        [SettingProperty("Enabled", true, "")]
        [SettingPropertyGroup("General")]
        public bool Enabled { get; set; } = true;

        [SettingProperty("Apply Relationship Rules to NPC", false, "")]
        [SettingPropertyGroup("General")]
        public bool ApplyRelationshipRulesToNPC { get; set; } = false;

        [SettingProperty("Independent Multiplier", 1, 4, false, "Relationship multiplier when Hero is independent.")]
        [SettingPropertyGroup("Multipliers")]
        public int IndependentMultiplier { get; set; } = 1;

        [SettingProperty("Mercenary Multiplier", 1, 4, false, "Relationship multiplier when Hero is a mercenary.")]
        [SettingPropertyGroup("Multipliers")]
        public int MercenaryMultiplier { get; set; } = 1;

        [SettingProperty("Vassal Multiplier", 1, 4, false, "Relationship multiplier when Hero is a vassal.")]
        [SettingPropertyGroup("Multipliers")]
        public int VassalMultiplier { get; set; } = 2;


        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at peace
        /// </summary>
        public int LeavingLossPeace { get; set; } = -5;

        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at war
        /// </summary>
        public int LeavingLossWar { get; set; } = -20;


        /// <summary>
        /// Maximum relationship to increase to when releasing a prisoner after battle
        /// </summary>
        public int ReleaseAfterBattleCap { get; set; } = 10;

        /// <summary>
        /// Maximum relationship to increase to when killing an enemy of a Hero
        /// </summary>
        public int EnemyCap { get; set; } = 25;

        /// <summary>
        /// Maximum relationship to increase to when killing a Hero of opposite trait
        /// </summary>
        public int TraitCap { get; set; } = 10;


        /// <summary>
        /// Maximum relationship to decrease to when attacking an enemy of the Kingdom
        /// </summary>
        public int AttackedCap { get; set; } = -10;

        /// <summary>
        /// Maximum relationship to decrease to when joining a defender with strength difference
        /// </summary>
        public int HelpedDefenderCap { get; set; } = 10;

        /// <summary>
        /// Minimum amount battles needed to perform to avoid influence penalty
        /// </summary>
        public int MinimumBattleCount { get; set; } = 10;

        /// <summary>
        /// Length of a Mercenary Contract
        /// </summary>
        /// 
        public int ContractLengthInDays { get; set; } = 30;

        /// <summary>
        /// How much days a clan has to perform the minimum amounth of battles per contract before the penalty kicks in. 
        /// </summary>
        public int DaysBeforeInfluencePenalty { get; set; } = 7;

        /// <summary>
        /// Influence penalty for not performing the minimum amount of battles per contract.
        /// </summary>
        public int InfluencePenalty { get; set; } = -1;
    }
}