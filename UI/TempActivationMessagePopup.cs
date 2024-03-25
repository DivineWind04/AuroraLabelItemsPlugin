using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtopPlugin.UI;

public static class TempActivationMessagePopup
{
    private const string AcknowledgeFileName = ".atop_activation_ack";

    public static void PopUpActivationMessageIfFirstTime()
    {
        if (ActivationAckFileExists()) return;
        Task.Run(() => MessageBox.Show(
            """Starting with this version of the ATOP vatSys plugin, you will have to activate your session by clicking the "Activate" button under the "ATOP" menu in order to use the full controlling functionalities.""",
            @"vatSys ATOP Plugin"
        ));
        WriteActivationAckFile();
    }

    private static bool ActivationAckFileExists()
    {
        return File.Exists(GetAckFilePath());
    }

    private static void WriteActivationAckFile()
    {
        File.WriteAllBytes(GetAckFilePath(), new byte[] { });
    }

    private static string GetAckFilePath()
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            AcknowledgeFileName);
    }
}