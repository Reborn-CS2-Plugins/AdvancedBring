using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CS2TraceRay.Class;
using CS2TraceRay.Enum;
using Vector = CounterStrikeSharp.API.Modules.Utils.Vector;
namespace AdvancedBring;

public class AdvancedBringConfig : BasePluginConfig
{
    [JsonPropertyName("command_permission")] public string CommandPermission { get; set; } = "@css/slay";
    [JsonPropertyName("command_aliases")]
    public List<string> CommandAliases { get; set; } = new()
    {
        "css_agel",
        "css_aimegel"
    };
    [JsonPropertyName("command_aliases_T")]
    public List<string> CommandAliasesT { get; set; } = new()
    {
        "css_agelt",
        "css_aimegelt"
    };
    [JsonPropertyName("command_aliases_CT")]
    public List<string> CommandAliasesCT { get; set; } = new()
    {
        "css_agelct",
        "css_aimegelct"
    };
}

public class AdvancedBring : BasePlugin, IPluginConfig<AdvancedBringConfig>
{
    public override string ModuleName => "AdvancedBring";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "gyro";
    public AdvancedBringConfig Config { get; set; } = new();
    
    public override void Load(bool hotReload)
    {
    }
    
    public void OnConfigParsed(AdvancedBringConfig config)
    {
        Config = config;
        
        foreach (var alias in Config.CommandAliases)
        {
            AddCommand(alias, Localizer["CommandDescription"], OnTracePositionCommand);
        }
        foreach (var alias in Config.CommandAliasesT)
        {
            AddCommand(alias, Localizer["CommandDescriptionT"], OnTracePositionForTeam2);
        }
        foreach (var alias in Config.CommandAliasesCT)
        {
            AddCommand(alias, Localizer["CommandDescriptionCT"], OnTracePositionForTeam3);
        }
    }
    
    public void OnTracePositionForTeam3(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (!AdminManager.PlayerHasPermissions(player, Config.CommandPermission))
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["NoPermission"]);
            return;
        }

        // ALWAYS teleport all CT players, ignore arguments
        var rawTargets = Utilities.GetPlayers().Where(p => p.TeamNum == 3).ToList();
        ProcessTeleport(player, rawTargets);
    }
    
    public void OnTracePositionForTeam2(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (!AdminManager.PlayerHasPermissions(player, Config.CommandPermission))
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["NoPermission"]);
            return;
        }

        // ALWAYS teleport all T players, ignore arguments
        var rawTargets = Utilities.GetPlayers().Where(p => p.TeamNum == 2).ToList();
        ProcessTeleport(player, rawTargets);
    }

    public void OnTracePositionCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (!AdminManager.PlayerHasPermissions(player, Config.CommandPermission))
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["NoPermission"]);
            return;
        }

        // Uses arguments for targeting
        var rawTargets = command.GetArgTargetResult(1).ToList();
        ProcessTeleport(player, rawTargets);
    }

    private void ProcessTeleport(CCSPlayerController player, List<CCSPlayerController> rawTargets)
    {
        if (rawTargets.Count == 0)
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["NoTarget"]);
            return;
        }

        var targets = rawTargets.Count > 1
            ? rawTargets.Where(t => t != player && t.PawnIsAlive).ToList()
            : rawTargets.Where(t => t.PawnIsAlive).ToList();

        if (targets.Count == 0)
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["NoTarget"]);
            return;
        }

        var trace = player.GetGameTraceByEyePosition(TraceMask.MaskAll, Contents.NoDraw, player);

        if (!trace.HasValue)
        {
            player.PrintToChat(Localizer["Prefix"] + Localizer["TraceFailed"]);
            return;
        }

        var hitPos = new Vector(
            trace.Value.Position.X,
            trace.Value.Position.Y,
            trace.Value.Position.Z
        );

        foreach (var p in targets.ToList())
        {
            if (p == null || p.PlayerPawn.Value is not { IsValid: true } playerPawn) continue;
            
            var angles = playerPawn.EyeAngles;
            var velocity = new Vector(
                playerPawn.Velocity.X,
                playerPawn.Velocity.Y,
                playerPawn.Velocity.Z
            );

            if (angles == null || velocity == null) continue;

            playerPawn.Teleport(hitPos, angles, velocity);
        }
        
        if (targets.Count > 1)
            Server.PrintToChatAll(Localizer["Prefix"] + Localizer["TeleportToAimSuccessXPlayers", player.PlayerName, targets.Count]);
        else if (targets.Count == 1)
            Server.PrintToChatAll(Localizer["Prefix"] + Localizer["TeleportToAimSuccess", player.PlayerName, targets.FirstOrDefault()!.PlayerName]);
    }
}