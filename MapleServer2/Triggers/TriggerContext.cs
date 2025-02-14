﻿using Maple2.Trigger;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Triggers;

public partial class TriggerContext : ITriggerContext
{
    public int NextTick;
    public TriggerState SkipSceneState;

    private readonly FieldManager Field;
    private readonly ILogger Logger;

    public TriggerContext(FieldManager field, ILogger logger)
    {
        Field = field;
        Logger = logger;
    }

    public void WriteLog(string arg1, int arg2, string arg3, byte arg4, string arg5)
    {
    }

    public void DebugString(string message, string feature)
    {
        Logger.Debug(message);
    }

    public int GetDungeonFirstUserMissionScore()
    {
        return 0;
    }

    public int GetDungeonId()
    {
        return 0;
    }

    public int GetDungeonLevel()
    {
        return 3;
    }

    public int GetDungeonMaxUserCount()
    {
        return 1;
    }

    public int GetDungeonPlayTime()
    {
        return 0;
    }

    public int GetDungeonRoundsRequired()
    {
        return int.MaxValue;
    }

    public string GetDungeonState()
    {
        return string.Empty;
    }

    public bool GetDungeonVariable(int id)
    {
        return false;
    }

    public float GetNpcDamageRate(int spawnPointId)
    {
        return 1.0f;
    }

    public int GetNpcExtraData(int spawnPointId, string extraDataKey)
    {
        return 0;
    }

    public float GetNpcHpRate(int spawnPointId)
    {
        return 1.0f;
    }

    public int GetScoreBoardScore()
    {
        return 0;
    }

    public int GetShadowExpeditionPoints()
    {
        return 0;
    }

    public int GetUserCount(int boxId, int userTagId)
    {
        if (boxId == 0)
        {
            return Field.State.Players.Values.Count;
        }

        MapTriggerBox box = MapEntityMetadataStorage.GetTriggerBox(Field.MapId, boxId);
        if (box is null)
        {
            return 0;
        }

        int userCount = 0;
        foreach (IFieldActor<Player> player in Field.State.Players.Values)
        {
            if (FieldManager.IsPlayerInBox(box, player))
            {
                userCount++;
            }
        }

        return userCount;
    }

    public int GetUserValue(string key)
    {
        // TODO: Fix when Mob AI infra is ready.
        // This is a hack. This probably is handled from mob AI.
        if (key == "battleStop")
        {
            foreach (KeyValuePair<int, Npc> mob in Field.State.Mobs)
            {
                // check if mob is 50% hp or less
                if (mob.Value.Stats[StatAttribute.Hp].Total <= mob.Value.Stats[StatAttribute.Hp].Base / 2)
                {
                    return 1;
                }
            }
        }

        IFieldObject<Player> player = Field.State.Players.Values.FirstOrDefault(x => x.Value.Triggers.Any(y => y.Key == key));
        if (player == null)
        {
            return 0;
        }

        return player.Value.Triggers.FirstOrDefault(x => x.Key == key)?.Value ?? 0;
    }
}
