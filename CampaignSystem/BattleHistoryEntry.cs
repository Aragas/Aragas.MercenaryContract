using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace Aragas.CampaignSystem
{
    public class BattleHistoryEntry
    {
        [SaveableField(1)]
        public PartyBase Attacker;

        [SaveableField(2)]
        public PartyBase Defender;

        [SaveableField(3)]
        public MapEvent.BattleTypes Type;

        [SaveableField(4)]
        public BattleState State;

        [SaveableField(5)]
        public CampaignTime Time;

        public BattleHistoryEntry(MapEvent battle, CampaignTime endTime)
        {
            Attacker = battle.GetLeaderParty(BattleSideEnum.Attacker);
            Defender = battle.GetLeaderParty(BattleSideEnum.Defender);
            Type = battle.EventType;
            State = battle.BattleState;
            Time = endTime;
        }
    }
}