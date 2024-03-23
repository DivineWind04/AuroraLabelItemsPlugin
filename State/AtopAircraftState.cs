using AtopPlugin.Logic;
using AtopPlugin.Models;
using vatsys;

namespace AtopPlugin.State;

public class AtopAircraftState
{
    public AtopAircraftState(FDP2.FDR fdr)
    {
        Fdr = fdr;
        UpdateFromFdr(fdr);
        DownlinkIndicator = false;
        RadarToggleIndicator = false;
        WasHandedOff = false;
    }

    public FDP2.FDR Fdr { get; private set; }
    public CalculatedFlightData CalculatedFlightData { get; private set; }
    public DirectionOfFlight DirectionOfFlight { get; private set; }
    public SccFlag? HighestSccFlag { get; private set; }
    public AltitudeFlag? AltitudeFlag { get; private set; }
    public SectorsVolumes.Sector? NextSector { get; private set; }
    public bool DownlinkIndicator { get; set; }
    public bool RadarToggleIndicator { get; set; }
    public bool WasHandedOff { get; private set; }
    public bool PendingAltitudeChange { get; private set; }

    private AltitudeBlock PreviousAltitudeBlock { get; set; }

    public void UpdateFromFdr(FDP2.FDR updatedFdr)
    {
        Fdr = updatedFdr;

        CalculatedFlightData = FlightDataCalculator.GetCalculatedFlightData(updatedFdr);
        DirectionOfFlight = DirectionOfFlightCalculator.GetDirectionOfFlight(updatedFdr);
        WasHandedOff = !WasHandedOff && (updatedFdr.IsHandoff || updatedFdr.ControllingSector == null);
        HighestSccFlag = SccFlagCalculator.CalculateHighestPriorityFlag(updatedFdr, CalculatedFlightData);

        // ensure the bool for altitude change is calculated first since it is used in the altitude flag calculation
        PendingAltitudeChange =
            AltitudeCalculator.CalculateAltitudeChangePending(updatedFdr, PreviousAltitudeBlock, PendingAltitudeChange);
        AltitudeFlag = AltitudeCalculator.CalculateAltitudeFlag(updatedFdr, PendingAltitudeChange);

        NextSector = NextSectorCalculator.GetNextSector(updatedFdr);

        // update this last since so we have the previous value for the next update
        PreviousAltitudeBlock = AltitudeBlock.ExtractAltitudeBlock(updatedFdr);
    }
}