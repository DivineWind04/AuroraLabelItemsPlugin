using System;
using System.Linq;
using AtopPlugin.Logic;
using AtopPlugin.Models;
using vatsys;
using vatsys.Plugin;

namespace AtopPlugin.State;

public class AtopAircraftDisplayState
{
    private static readonly string[] RestrictionLabels = { "AT ", " BY ", "CLEARED TO " };

    public AtopAircraftDisplayState(AtopAircraftState atopAircraftState)
    {
        UpdateFromAtopState(atopAircraftState);
    }

    public string CpdlcAdsbSymbol { get; private set; }
    public string AdsFlag { get; private set; }
    public string LateralFlag { get; private set; }
    public bool IsMntFlagToggled { get; private set; }
    public bool HasAnnotations { get; private set; }
    public bool IsRestrictionsIndicatorToggled { get; private set; }
    public string CurrentLevel { get; private set; }
    public string ClearedLevel { get; private set; }
    public string RequestedLevel { get; private set; }
    public BorderFlags AltitudeBorderFlags { get; private set; }
    public CustomColour? AltitudeColor { get; private set; }
    public string FiledSpeed { get; private set; }
    public string GroundSpeed { get; private set; }

    public void UpdateFromAtopState(AtopAircraftState atopAircraftState)
    {
        CpdlcAdsbSymbol = GetCpdlcAdsbSymbol(atopAircraftState);
        AdsFlag = GetAdsFlag(atopAircraftState);
        LateralFlag = GetLateralFlag(atopAircraftState);
        IsMntFlagToggled = atopAircraftState.Fdr.IsJet();
        HasAnnotations = !string.IsNullOrEmpty(atopAircraftState.Fdr.LabelOpData);
        IsRestrictionsIndicatorToggled = GetRestrictionsIndicatorToggled(atopAircraftState);
        CurrentLevel = GetCurrentLevel(atopAircraftState);
        ClearedLevel = GetClearedLevel(atopAircraftState);
        RequestedLevel = GetRequestedLevel(atopAircraftState);
        AltitudeBorderFlags = GetAltitudeBorderFlags(atopAircraftState);
        AltitudeColor = atopAircraftState.Fdr.RVSM ? null : CustomColors.NonRvsm;
        FiledSpeed = GetFiledSpeed(atopAircraftState);
        GroundSpeed = GetGroundSpeed(atopAircraftState);
    }

    private static string GetCpdlcAdsbSymbol(AtopAircraftState atopAircraftState)
    {
        var adsb = atopAircraftState.Fdr.ADSB;
        var cpdlc = atopAircraftState.CalculatedFlightData.Cpdlc;
        return (adsb, cpdlc) switch
        {
            { adsb: true, cpdlc: true } => Symbols.CpdlcAndAdsb,
            { adsb: true, cpdlc: false } => Symbols.Empty,
            { adsb: false, cpdlc: true } => Symbols.CpdlcNoAdsb,
            { adsb: false, cpdlc: false } => Symbols.NoCpdlcNoAdsb
        };
    }

    private static string GetAdsFlag(AtopAircraftState atopAircraftState)
    {
        return atopAircraftState.CalculatedFlightData switch
        {
            { Adsc: true, Cpdlc: true, Rnp4: true } => Symbols.D30,
            { Adsc: true, Cpdlc: true, Rnp10: true } => Symbols.D50,
            _ => Symbols.Empty
        };
    }

    private static string GetLateralFlag(AtopAircraftState atopAircraftState)
    {
        return atopAircraftState.CalculatedFlightData switch
        {
            { Adsc: true, Cpdlc: true, Rnp4: true } => Symbols.L23,
            { Adsc: true, Cpdlc: true, Rnp10: true } => Symbols.L50,
            _ => Symbols.Empty
        };
    }

    private static bool GetRestrictionsIndicatorToggled(AtopAircraftState atopAircraftState)
    {
        return RestrictionLabels.Any(label => atopAircraftState.Fdr.LabelOpData.Contains(label));
    }

    private static string GetCurrentLevel(AtopAircraftState atopAircraftState)
    {
        var fdr = atopAircraftState.Fdr;
        var altitudeBlock = AltitudeBlock.ExtractAltitudeBlock(fdr);

        var prlHundreds = (int)Math.Round(atopAircraftState.Fdr.PRL / 100.0);
        if (!atopAircraftState.PendingAltitudeChange || AltitudeCalculator.IsWithinThreshold(fdr.PRL, altitudeBlock))
            return prlHundreds == 0 ? "" : (int)Math.Round(prlHundreds / 10.0) + "0";

        return prlHundreds == 0 ? "" : prlHundreds.ToString();
    }

    private static string GetClearedLevel(AtopAircraftState atopAircraftState)
    {
        var fdr = atopAircraftState.Fdr;
        var altitudeBlock = AltitudeBlock.ExtractAltitudeBlock(fdr);

        if (!atopAircraftState.PendingAltitudeChange ||
            AltitudeCalculator.IsWithinThreshold(fdr.PRL, altitudeBlock))
            return Symbols.Empty;

        return altitudeBlock.ToString();
    }

    private static string GetRequestedLevel(AtopAircraftState atopAircraftState)
    {
        return atopAircraftState.Fdr.State == FDP2.FDR.FDRStates.STATE_INACTIVE
            ? (atopAircraftState.Fdr.RFL / 100).ToString()
            : Symbols.Empty;
    }

    private static BorderFlags GetAltitudeBorderFlags(AtopAircraftState atopAircraftState)
    {
        return atopAircraftState.Fdr.State switch
        {
            FDP2.FDR.FDRStates.STATE_PREACTIVE or FDP2.FDR.FDRStates.STATE_COORDINATED => BorderFlags.All,
            _ => BorderFlags.None
        };
    }

    private static string GetFiledSpeed(AtopAircraftState atopAircraftState)
    {
        var temperature = GRIB.FindTemperature(atopAircraftState.Fdr.PRL, atopAircraftState.Fdr.GetLocation(), true);
        var mach = Conversions.CalculateMach(atopAircraftState.Fdr.TAS, temperature);
        return "M" + mach.ToString("F2").Replace(".", "");
    }

    private static string GetGroundSpeed(AtopAircraftState atopAircraftState)
    {
        var gs = atopAircraftState.Fdr.PredictedPosition.Groundspeed;
        return "N" + gs.ToString("000");
    }
}