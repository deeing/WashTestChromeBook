using System.ComponentModel;
public enum PlayerEventType
{
    [Description("Left Palm Switch")]
    PalmSwitch,
    [Description("Palms")]
    PalmScrub,
    [Description("Fingertips Left Switch")]
    FingertipsLSwitch,
    [Description("Fingertips Left")]
    FingertipsLScrub,
    [Description("Fingertips Right Switch")]
    FingertipsRSwitch,
    [Description("Fingertips Right")]
    FingertipsRScrub,
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
    [Description("Right Wrist Switch")]
    WristRSwitch,
    [Description("Right Wrist")]
    WristRScrub,
    [Description("Left Thumb Switch")]
    ThumbLSwitch,
    [Description("Left Thumb")]
    ThumbLScrub,
    [Description("Right Thumb Switch")]
    ThumbRSwitch,
    [Description("Right Thumb")]
    ThumbRScrub,
    [Description("Between Fingers Switch")]
    BetweenFingersSwitch,
    [Description("Between Fingers")]
    BetweenFingersScrub,
    [Description("Left Fingernails Switch")]
    FingernailsLSwitch,
    [Description("Left Fingernails")]
    FingernailsLScrub,
    [Description("Right Fingernails Switch")]
    FingernailsRSwitch,
    [Description("Right Fingernails")]
    FingernailsRScrub
}
