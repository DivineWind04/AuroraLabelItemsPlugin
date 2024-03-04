using System.ComponentModel;

namespace AtopPlugin.UI;

partial class SettingsWindow
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.probe = new System.Windows.Forms.CheckBox();
        this.SuspendLayout();
        // 
        // probe
        // 
        this.probe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.probe.Appearance = System.Windows.Forms.Appearance.Button;
        this.probe.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.probe.Font = new System.Drawing.Font("Terminus (TTF)", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.probe.Location = new System.Drawing.Point(25, 12);
        this.probe.Name = "probe";
        this.probe.Size = new System.Drawing.Size(100, 50);
        this.probe.TabIndex = 1;
        this.probe.Text = "PROBE";
        this.probe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.probe.UseVisualStyleBackColor = true;
        this.probe.CheckedChanged += new System.EventHandler(this.probe_CheckedChanged);
        // 
        // SettingsWindow
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(146, 72);
        this.Controls.Add(this.probe);
        this.Name = "SettingsWindow";
        this.Text = "ATOP Settings";
        this.TopMost = true;
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.CheckBox probe;

    #endregion
}