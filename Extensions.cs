using AtopPlugin.Conflict;
using AtopPlugin.State;
using vatsys;

namespace AtopPlugin;

public static class Extensions
{
    public static AtopAircraftState? GetAtopState(this FDP2.FDR fdr)
    {
        return AtopPluginStateManager.GetAircraftState(fdr.Callsign);
    }

    public static AtopAircraftDisplayState? GetDisplayState(this FDP2.FDR fdr)
    {
        return AtopPluginStateManager.GetDisplayState(fdr.Callsign);
    }

    public static ConflictProbe.Conflicts? GetConflicts(this FDP2.FDR fdr)
    {
        return AtopPluginStateManager.GetConflicts(fdr.Callsign);
    }

    public static int? GetTransponderCode(this FDP2.FDR fdr)
    {
        return fdr.CoupledTrack?.ActualAircraft.TransponderCode;
    }

    public static bool IsJet(this FDP2.FDR fdr)
    {
        return fdr.PerformanceData?.IsJet ?? false;
    }

    public static bool IsConnected(this FDP2.FDR fdr)
    {
        return fdr.CoupledTrack?.ActualAircraft != null ||
               Network.GetOnlinePilots.Exists(pilot => pilot.Callsign == fdr.Callsign);
    }

    public static bool IsSelected(this Track track)
    {
        return MMI.SelectedTrack == track;
    }
}