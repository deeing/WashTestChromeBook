using System.ComponentModel;
public enum PlayerEventType
{
    [Description("PALMS")]
    PalmSwitch,
    [Description("Palms Scrub")]
    PalmScrub,
    [Description("FINGERTIPS - Left")]
    FingertipsLSwitch,
    [Description("Fingertips Left Scrub")]
    FingertipsLScrub,
    [Description("FINGERTIPS - Right")]
    FingertipsRSwitch,
    [Description("Fingertips Right Scrub")]
    FingertipsRScrub,
    [Description("BACK - Left")]
    BackOfHandLSwitch,
    [Description("Back of Left Hand Scrub")]
    BackOfHandLScrub,
    [Description("BACK - Right")]
    BackOfHandRSwitch,
    [Description("Back of Right Hand Scrub")]
    BackOfHandRScrub,
    [Description("WRIST - Left")]
    WristLSwitch,
    [Description("Left Wrist Scrub")]
    WristLScrub,
    [Description("WRIST - Right")]
    WristRSwitch,
    [Description("Right Wrist Scrub")]
    WristRScrub,
    [Description("THUMB - Left")]
    ThumbLSwitch,
    [Description("Left Thumb Scrub")]
    ThumbLScrub,
    [Description("THUMB - Right")]
    ThumbRSwitch,
    [Description("Right Thumb Scrub")]
    ThumbRScrub,
    [Description("BETWEEN")]
    BetweenFingersSwitch,
    [Description("Between Fingers Scrub")]
    BetweenFingersScrub,
    [Description("NAILS - Left")]
    FingernailsLSwitch,
    [Description("Left Fingernails Scrub")]
    FingernailsLScrub,
    [Description("NAILS - Right")]
    FingernailsRSwitch,
    [Description("Right Fingernails Scrub")]
    FingernailsRScrub,
    [Description("Non linear Switch")]
    NonLinearSwitch
}
