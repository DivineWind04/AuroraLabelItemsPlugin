using AtopPlugin.Models;
using vatsys;
using vatsys.Plugin;

namespace AtopPlugin.Display;

public static class TrackColorRenderer
{
    public static CustomColour? GetAsdColor(Track track)
    {
        var fdr = track.GetFDR();

        if (fdr == null) return null;

        return fdr.State switch
        {
            FDP2.FDR.FDRStates.STATE_PREACTIVE or FDP2.FDR.FDRStates.STATE_COORDINATED => GetConflictColour(fdr),
            _ => GetConflictColour(fdr) ?? GetDirectionColour(fdr, track)
        };
    }

    public static CustomColour? GetDirectionColour(FDP2.FDR fdr, Track track)
    {
        if (!IsInJurisdiction(track)) return null;
        return fdr.GetAtopState()?.DirectionOfFlight switch
        {
            DirectionOfFlight.Eastbound => CustomColors.EastboundColour,
            DirectionOfFlight.Westbound => CustomColors.WestboundColour,
            _ => null
        };
    }

    private static CustomColour? GetConflictColour(FDP2.FDR fdr)
    {
        return fdr.GetConflicts() switch
        {
            { ActualConflicts.Count: > 0 } or { ImminentConflicts.Count: > 0 } => CustomColors.Imminent,
            { AdvisoryConflicts.Count: > 0 } => CustomColors.Advisory,
            _ => null
        };
    }

    private static bool IsInJurisdiction(Track track)
    {
        return track.State == MMI.HMIStates.Jurisdiction;
    }
}