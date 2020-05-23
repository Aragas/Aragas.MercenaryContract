/*
using Aragas.MountAndBlade;

using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem
{
    public class MercenaryContractOptions
    {
        [SaveableField(1)]
        private bool _applyRelationshipRulesToNPC = true;
        [SaveableField(2)]
        private int _independentMultiplier = 1;
        [SaveableField(3)]
        private int _mercenaryMultiplier = 1;
        [SaveableField(4)]
        private int _vassalMultiplier = 2;
        [SaveableField(5)]
        private int _leavingLossPeace = -5;
        [SaveableField(6)]
        private int _leavingLossWar = -20;
        [SaveableField(7)]
        private int _releaseAfterBattleCap = 10;
        [SaveableField(8)]
        private int _enemyCap = 25;
        [SaveableField(9)]
        private int _traitCap = 10;
        [SaveableField(10)]
        private int _attackedCap = -10;
        [SaveableField(11)]
        private int _helpedDefenderCap = 10;
        [SaveableField(12)]
        private int _minimumBattleCount = 10;
        [SaveableField(13)]
        private int _contractLengthInDays = 30;
        [SaveableField(14)]
        private int _daysBeforeInfluencePenalty = 7;
        [SaveableField(15)]
        private int _influencePenalty = -1;

        private static MercenaryContractOptions _current => MercenaryContractSubModule.Options;

        /// <summary>
        /// 
        /// </summary>
        public static bool ApplyRelationshipRulesToNPC
        {
            get => _current?._applyRelationshipRulesToNPC == true;
            set
            {
                if (_current != null)
                    _current._applyRelationshipRulesToNPC = value;
            }
        }

        /// <summary>
        /// Relationship multiplier when Hero is independent
        /// </summary>
        public static int IndependentMultiplier
        {
            get => _current?._independentMultiplier ?? 0;
            set
            {
                if (_current != null)
                    _current._independentMultiplier = value;
            }
        }

        /// <summary>
        /// Relationship multiplier when Hero is a mercenary
        /// </summary>
        public static int MercenaryMultiplier
        {
            get => _current?._mercenaryMultiplier ?? 0;
            set
            {
                if (_current != null)
                    _current._mercenaryMultiplier = value;
            }
        }

        /// <summary>
        /// Relationship multiplier when Hero is a vassal
        /// </summary>
        public static int VassalMultiplier
        {
            get => _current?._vassalMultiplier ?? 0;
            set
            {
                if (_current != null)
                    _current._vassalMultiplier = value;
            }
        }


        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at peace
        /// </summary>
        public static int LeavingLossPeace
        {
            get => _current?._leavingLossPeace ?? 0;
            set
            {
                if (_current != null)
                    _current._leavingLossPeace = value;
            }
        }

        /// <summary>
        /// Relationship penalty when leaving a kingdom as a mercenary at war
        /// </summary>
        public static int LeavingLossWar
        {
            get => _current?._leavingLossWar ?? 0;
            set
            {
                if (_current != null)
                    _current._leavingLossWar = value;
            }
        }


        /// <summary>
        /// Maximum relationship to increase to when releasing a prisoner after battle
        /// </summary>
        public static int ReleaseAfterBattleCap
        {
            get => _current?._releaseAfterBattleCap ?? 0;
            set
            {
                if (_current != null)
                    _current._releaseAfterBattleCap = value;
            }
        }

        /// <summary>
        /// Maximum relationship to increase to when killing an enemy of a Hero
        /// </summary>
        public static int EnemyCap
        {
            get => _current?._enemyCap ?? 0;
            set
            {
                if (_current != null)
                    _current._enemyCap = value;
            }
        }

        /// <summary>
        /// Maximum relationship to increase to when killing a Hero of opposite trait
        /// </summary>
        public static int TraitCap
        {
            get => _current?._traitCap ?? 0;
            set
            {
                if (_current != null)
                    _current._traitCap = value;
            }
        }


        /// <summary>
        /// Maximum relationship to decrease to when attacking an enemy of the Kingdom
        /// </summary>
        public static int AttackedCap
        {
            get => _current?._attackedCap ?? 0;
            set
            {
                if (_current != null)
                    _current._attackedCap = value;
            }
        }

        /// <summary>
        /// Maximum relationship to decrease to when joining a defender with strength difference
        /// </summary>
        public static int HelpedDefenderCap
        {
            get => _current?._helpedDefenderCap ?? 0;
            set
            {
                if (_current != null)
                    _current._helpedDefenderCap = value;
            }
        }

        /// <summary>
        /// Minimum amount battles needed to perform to avoid influence penalty
        /// </summary>
        public static int MinimumBattleCount
        {
            get => _current?._minimumBattleCount ?? 0;
            set
            {
                if (_current != null)
                    _current._minimumBattleCount = value;
            }
        }

        /// <summary>
        /// Length of a Mercenary Contract
        /// </summary>
        /// 
        public static int ContractLengthInDays
        {
            get => _current?._contractLengthInDays ?? 0;
            set
            {
                if (_current != null)
                    _current._contractLengthInDays = value;
            }
        }

        /// <summary>
        /// How much days a clan has to perform the minimum amounth of battles per contract before the penalty kicks in. 
        /// </summary>
        public static int DaysBeforeInfluencePenalty
        {
            get => _current?._daysBeforeInfluencePenalty ?? 0;
            set
            {
                if (_current != null)
                    _current._daysBeforeInfluencePenalty = value;
            }
        }

        /// <summary>
        /// Influence penalty for not performing the minimum amount of battles per contract.
        /// </summary>
        public static int InfluencePenalty
        {
            get => _current?._influencePenalty ?? 0;
            set
            {
                if (_current != null)
                    _current._influencePenalty = value;
            }
        }
    }
}
*/