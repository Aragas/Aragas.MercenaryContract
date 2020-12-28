using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.PerSave;

namespace Aragas
{
    public class MercenarySettings : AttributePerSaveSettings<MercenarySettings>
    {
        public override string Id { get; } = "Aragas.MercenaryContract_v1";
        public override string DisplayName { get; } = $"Limited Mercenary Contract {typeof(MercenarySettings).Assembly.GetName().Version.ToString(3)}";
        public override string FolderName { get; } = "Aragas.MercenaryContract";

        [SettingPropertyBool("Enabled", Order = 1)]
        [SettingPropertyGroup("General")]
        public bool Enabled { get; set; } = true;

        [SettingPropertyBool("Apply Relationship Rules to NPC", RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("General")]
        public bool ApplyRelationshipRulesToNPC { get; set; } = false;

        [SettingPropertyInteger("Independent Multiplier", 1, 4, RequireRestart = false, HintText = "Relationship multiplier when Hero is independent.")]
        [SettingPropertyGroup("Multipliers")]
        public int IndependentMultiplier { get; set; } = 1;

        [SettingPropertyInteger("Mercenary Multiplier", 1, 4, RequireRestart = false, HintText = "Relationship multiplier when Hero is a mercenary.")]
        [SettingPropertyGroup("Multipliers")]
        public int MercenaryMultiplier { get; set; } = 1;

        [SettingPropertyInteger("Vassal Multiplier", 1, 4, RequireRestart = false, HintText = "Relationship multiplier when Hero is a vassal.")]
        [SettingPropertyGroup("Multipliers")]
        public int VassalMultiplier { get; set; } = 2;


        [SettingPropertyInteger("Leaving Loss Peace", 1, 4, RequireRestart = false, HintText = "Relationship penalty when leaving a kingdom as a mercenary at peace.")]
        [SettingPropertyGroup("Penalties")]
        public int LeavingLossPeace { get; set; } = -5;

        [SettingPropertyInteger("Leaving Loss War", 1, 4, RequireRestart = false, HintText = "Relationship penalty when leaving a kingdom as a mercenary at war.")]
        [SettingPropertyGroup("Penalties")]
        public int LeavingLossWar { get; set; } = -20;


        [SettingPropertyInteger("Leaving Loss War", 1, 4, RequireRestart = false, HintText = "Maximum relationship to increase to when releasing a prisoner after battle.")]
        [SettingPropertyGroup("Caps")]
        public int ReleaseAfterBattleCap { get; set; } = 10;

        [SettingPropertyInteger("Leaving Loss War", 1, 4, RequireRestart = false, HintText = "Maximum relationship to increase to when killing an enemy of a Hero.")]
        [SettingPropertyGroup("Caps")]
        public int EnemyCap { get; set; } = 25;

        [SettingPropertyInteger("Leaving Loss War", 1, 4, RequireRestart = false, HintText = "Maximum relationship to increase to when killing a Hero of opposite trait.")]
        [SettingPropertyGroup("Caps")]
        public int TraitCap { get; set; } = 10;

        [SettingPropertyInteger("Leaving Loss War", 1, 4, RequireRestart = false, HintText = "Maximum relationship to decrease to when attacking an enemy of the Kingdom.")]
        [SettingPropertyGroup("Caps")]
        public int AttackedCap { get; set; } = -10;

        [SettingPropertyInteger("Helped Defender Cap", 0, 4, RequireRestart = false, HintText = "Maximum relationship to decrease to when joining a defender with strength difference.")]
        [SettingPropertyGroup("Caps")]
        public int HelpedDefenderCap { get; set; } = 10;


        [SettingPropertyInteger("Minimum Battle Count", 0, 50, RequireRestart = false, HintText = "Minimum amount battles needed to perform to avoid influence penalty.")]
        [SettingPropertyGroup("Misc")]
        public int MinimumBattleCount { get; set; } = 10;

        [SettingPropertyInteger("Contract Length In Days", 1, 90, RequireRestart = false, HintText = "Length of a Mercenary Contract.")]
        [SettingPropertyGroup("Misc")]
        public int ContractLengthInDays { get; set; } = 30;


        [SettingPropertyInteger("Days Before Influence Penalty", 0, 30, RequireRestart = false, HintText = "How much days a clan has to perform the minimum amount of battles per contract before the penalty kicks in.")]
        [SettingPropertyGroup("Misc")]
        public int DaysBeforeInfluencePenalty { get; set; } = 7;

        [SettingPropertyInteger("Influence Penalty", -10, 0, RequireRestart = false, HintText = "Influence penalty for not performing the minimum amount of battles per contract.")]
        [SettingPropertyGroup("Misc")]
        public int InfluencePenalty { get; set; } = -1;
    }
}