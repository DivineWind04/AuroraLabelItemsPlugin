using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using vatsys;

namespace AtopPlugin.State;

public static class FdrPropertyChangesListener
{
    private static readonly string[] RelevantProperties = { "CFLUpper", "CFLLower" };
    private static readonly HashSet<string> RegisteredFdrs = new();

    public static void RegisterAllHandlers()
    {
        FDP2.GetFDRs.ForEach(RegisterHandler);
    }

    public static void RegisterHandler(FDP2.FDR fdr)
    {
        var newEntry = RegisteredFdrs.Add(fdr.Callsign);
        if (!newEntry) return;
        fdr.PropertyChanged += Handle;
    }

    private static async void Handle(object sender, PropertyChangedEventArgs eventArgs)
    {
        if (sender is not FDP2.FDR fdr) return;
        if (!RelevantProperties.Contains(eventArgs.PropertyName)) return;
        await AtopPluginStateManager.ProcessFdrUpdate(fdr);
        await AtopPluginStateManager.ProcessDisplayUpdate(fdr);
        await AtopPluginStateManager.RunConflictProbe(fdr);
    }
}