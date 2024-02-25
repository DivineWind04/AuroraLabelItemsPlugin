﻿#nullable enable
using AuroraLabelItemsPlugin.Logic;
using AuroraLabelItemsPlugin.Models;
using vatsys;

namespace AuroraLabelItemsPlugin.State;

public class AtopAircraftState
{
    public AtopAircraftState(FDP2.FDR fdr)
    {
        Fdr = fdr;
        UpdateFromFdr(fdr);
        DownlinkIndicator = false;
        RadarToggleIndicator = false;
        PreviouslyTracked = false;
    }

    public FDP2.FDR Fdr { get; private set; }
    public CalculatedFlightData CalculatedFlightData { get; private set; }
    public DirectionOfFlight DirectionOfFlight { get; private set; }
    public SccFlag? HighestSccFlag { get; private set; }
    public AltitudeFlag? AltitudeFlag { get; private set; }
    public bool DownlinkIndicator { get; set; }
    public bool RadarToggleIndicator { get; set; }
    public bool PreviouslyTracked { get; set; }
    public bool PendingAltitudeChange { get; private set; }

    private AltitudeBlock PreviousAltitudeBlock { get; set; }

    public void UpdateFromFdr(FDP2.FDR updatedFdr)
    {
        Fdr = updatedFdr;

        CalculatedFlightData = FlightDataCalculator.GetCalculatedFlightData(updatedFdr);
        DirectionOfFlight = DirectionOfFlightCalculator.GetDirectionOfFlight(updatedFdr);
        HighestSccFlag = SccFlagCalculator.CalculateHighestPriorityFlag(updatedFdr);

        // ensure the bool for altitude change is calculated first since it is used in the altitude flag calculation
        PendingAltitudeChange =
            AltitudeCalculator.CalculateAltitudeChangePending(updatedFdr, PreviousAltitudeBlock, PendingAltitudeChange);
        AltitudeFlag = AltitudeCalculator.CalculateAltitudeFlag(updatedFdr, PendingAltitudeChange);

        // update this last since so we have the previous value for the next update
        PreviousAltitudeBlock = AltitudeBlock.ExtractAltitudeBlock(updatedFdr);
    }
}