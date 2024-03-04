using AtopPlugin.State;
using vatsys;
using vatsys.Plugin;

namespace AtopPlugin.Display;

public static class LabelItemRenderer
{
    public static CustomLabelItem? RenderLabelItem(string itemType, Track track, FDP2.FDR? fdr,
        RDP.RadarTrack _)
    {
        var renderedItem = RenderLabelItemDelegate(itemType, track, fdr);
        return renderedItem != null ? ExcludeConflictColor(fdr!, track, renderedItem) : null;
    }

    private static CustomLabelItem ExcludeConflictColor(FDP2.FDR fdr, Track track, CustomLabelItem customLabelItem)
    {
        // If we already overrode it, keep it that way
        if (customLabelItem.ForeColourIdentity == Colours.Identities.Custom) return customLabelItem;

        customLabelItem.ForeColourIdentity = Colours.Identities.Custom;
        customLabelItem.CustomForeColour = TrackColorRenderer.GetDirectionColour(fdr, track) ?? CustomColors.ApsBlue;

        return customLabelItem;
    }

    private static CustomLabelItem? RenderLabelItemDelegate(string itemType, Track track, FDP2.FDR? fdr)
    {
        if (fdr?.GetAtopState() == null || fdr.GetDisplayState() == null) return null;

        var atopState = fdr.GetAtopState()!;
        var displayState = fdr.GetDisplayState()!;

        return itemType switch
        {
            LabelConstants.LabelItemSelectHori => track.IsSelected()
                ? new CustomLabelItem { Text = Symbols.Empty, Border = BorderFlags.Bottom }
                : null,

            LabelConstants.LabelItemSelectVert => track.IsSelected()
                ? new CustomLabelItem { Text = Symbols.Empty, Border = BorderFlags.Left }
                : null,

            LabelConstants.LabelItemCommIcon => atopState.DownlinkIndicator
                ? new CustomLabelItem { Text = Symbols.CommDownlink, Border = BorderFlags.All }
                : new CustomLabelItem { Text = Symbols.CommEmpty },

            LabelConstants.LabelItemAdsbCpdlc => fdr.State is FDP2.FDR.FDRStates.STATE_PREACTIVE or
                FDP2.FDR.FDRStates.STATE_COORDINATED
                ? new CustomLabelItem
                {
                    Text = displayState.CpdlcAdsbSymbol, ForeColourIdentity = Colours.Identities.Custom,
                    CustomForeColour = CustomColors.NotCda
                }
                : new CustomLabelItem { Text = displayState.CpdlcAdsbSymbol },

            LabelConstants.LabelItemAdsFlags => new CustomLabelItem { Text = displayState.AdsFlag },

            LabelConstants.LabelItemMntFlags => displayState.IsMntFlagToggled
                ? new CustomLabelItem { Text = Symbols.MntFlag }
                : null,

            LabelConstants.LabelItemScc => atopState.HighestSccFlag != null
                ? new CustomLabelItem
                {
                    Text = atopState.HighestSccFlag!.Value, ForeColourIdentity = Colours.Identities.Custom,
                    CustomForeColour = CustomColors.SpecialConditionCode
                }
                : null,

            LabelConstants.LabelItemAnnotInd => displayState.HasAnnotations
                ? new CustomLabelItem { Text = Symbols.ScratchpadFlag }
                : new CustomLabelItem { Text = Symbols.UntoggledFlag },

            LabelConstants.LabelItemRestr => displayState.IsRestrictionsIndicatorToggled
                ? new CustomLabelItem { Text = Symbols.RestrictionsFlag }
                : null,

            LabelConstants.LabelItemLevel => displayState.AltitudeColor == null
                ? new CustomLabelItem
                {
                    Text = displayState.CurrentLevel.PadLeft(3),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda
                }
                : new CustomLabelItem
                {
                    Text = displayState.CurrentLevel.PadLeft(3),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda,
                    ForeColourIdentity = Colours.Identities.Custom,
                    CustomForeColour = displayState.AltitudeColor
                },

            LabelConstants.LabelItemVmi => displayState.AltitudeColor == null
                ? new CustomLabelItem
                {
                    Text = (atopState.AltitudeFlag?.Value ?? "").PadLeft(1),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda
                }
                : new CustomLabelItem
                {
                    Text = (atopState.AltitudeFlag?.Value ?? "").PadLeft(1),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda,
                    ForeColourIdentity = Colours.Identities.Custom,
                    CustomForeColour = displayState.AltitudeColor
                },

            LabelConstants.LabelItemClearedLevel => displayState.AltitudeColor == null
                ? new CustomLabelItem
                {
                    Text = displayState.ClearedLevel.PadLeft(3),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda
                }
                : new CustomLabelItem
                {
                    Text = displayState.ClearedLevel.PadLeft(3),
                    Border = displayState.AltitudeBorderFlags,
                    BorderColourIdentity = Colours.Identities.Custom,
                    CustomBorderColour = CustomColors.NotCda,
                    ForeColourIdentity = Colours.Identities.Custom,
                    CustomForeColour = displayState.AltitudeColor
                },

            LabelConstants.LabelItemRadarInd => atopState.RadarToggleIndicator
                ? new CustomLabelItem { Text = Symbols.RadarFlag, OnMouseClick = RadarFlagToggleHandler.Handle }
                : new CustomLabelItem { Text = Symbols.UntoggledFlag, OnMouseClick = RadarFlagToggleHandler.Handle },

            LabelConstants.LabelItemInhibitInd => fdr.State == FDP2.FDR.FDRStates.STATE_INHIBITED
                ? new CustomLabelItem { Text = Symbols.Inhibited }
                : null,

            LabelConstants.LabelItemFiledSpeed => new CustomLabelItem { Text = displayState.FiledSpeed },

            LabelConstants.LabelItem3DigitGroundspeed => new CustomLabelItem { Text = displayState.GroundSpeed },

            LabelConstants.LabelItemDestination => new CustomLabelItem { Text = fdr.DesAirport },

            _ => null
        };
    }
}