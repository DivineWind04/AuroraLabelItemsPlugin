namespace AtopPlugin.Models;

public class SccFlag
{
    private SccFlag(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static SccFlag Rnp => new("RNP");
    public static SccFlag Emg => new("EMG");
    public static SccFlag Rcf => new("RCF");
    public static SccFlag Mti => new("MTI");
    public static SccFlag Or => new("OR"); // out of range flag to represent pilot has has disconnected
}