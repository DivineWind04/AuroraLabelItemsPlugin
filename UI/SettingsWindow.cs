using System;
using AtopPlugin.State;
using vatsys;

namespace AtopPlugin.UI;

public partial class SettingsWindow : BaseForm
{
    public SettingsWindow()
    {
        InitializeComponent();
        probe.Checked = AtopPluginStateManager.IsConflictProbeEnabled();
    }

    private void probe_CheckedChanged(object sender, EventArgs e)
    {
        AtopPluginStateManager.SetConflictProbe(probe.Checked);
    }
}