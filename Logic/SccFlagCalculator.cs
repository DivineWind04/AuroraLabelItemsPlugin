using AtopPlugin.Models;
using vatsys;

namespace AtopPlugin.Logic;

public static class SccFlagCalculator
{
    private const int RadioFailureCode = 7600;
    private const int EmergencyCode = 7700;
    private const int MilitaryInterceptCode = 7777;

    public static SccFlag? CalculateHighestPriorityFlag(FDP2.FDR fdr, CalculatedFlightData calculatedFlightData)
    {
        var transponderCode = fdr.GetTransponderCode();
        return transponderCode switch
        {
            EmergencyCode => SccFlag.Emg,
            RadioFailureCode => SccFlag.Rcf,
            MilitaryInterceptCode => SccFlag.Mti,
            _ => calculatedFlightData is { Rnp4: false, Rnp10: false } ? SccFlag.Rnp : null
        };
    }
}