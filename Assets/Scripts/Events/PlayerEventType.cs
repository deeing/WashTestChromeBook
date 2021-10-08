using System.ComponentModel;
public enum PlayerEventType
{
    [Description("Left Palm Switch")]
    PalmSwitch,
    [Description("Palms")]
    PalmScrub,
    PalmRScrub,
    [Description("Fingertips Switch")]
    FingertipsSwitch,
    [Description("Fingertips")]
    FingertipsScrub,
    [Description("Back of Left Hand Switch")]
    BackOfHandLSwitch,
    [Description("Back of Left Hand")]
    BackOfHandLScrub,
    [Description("Back of Right Hand Switch")]
    BackOfHandRSwitch,
    [Description("Back of Right Hand")]
    BackOfHandRScrub,
    [Description("Left Wrist Switch")]
    WristLSwitch,
    [Description("Left Wrist")]
    WristLScrub,
    [Description("Left Thumb Switch")]
    ThumbLSwitch,
    [Description("Left Thumb")]
    ThumbLScrub,
    [Description("Between Fingers Switch")]
    BetweenFingersSwitch,
    [Description("Between Fingers")]
    BetweenFingersScrub
}
