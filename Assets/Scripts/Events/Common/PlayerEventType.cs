using System.ComponentModel;
public enum PlayerEventType
{
    [Description("Palm Switch")]
    PalmSwitch,
    [Description("Palms Scrub")]
    PalmScrub,
    [Description("Fingertips Left Switch")]
    FingertipsLSwitch,
    [Description("Fingertips Left Scrub")]
    FingertipsLScrub,
    [Description("Fingertips Right Switch")]
    FingertipsRSwitch,
    [Description("Fingertips Right Scrub")]
    FingertipsRScrub,
    [Description("Back of Left Hand Switch")]
    BackOfHandLSwitch,
    [Description("Back of Left Hand Scrub")]
    BackOfHandLScrub,
    [Description("Back of Right Hand Switch")]
    BackOfHandRSwitch,
    [Description("Back of Right Hand Scrub")]
    BackOfHandRScrub,
    [Description("Left Wrist Switch")]
    WristLSwitch,
    [Description("Left Wrist Scrub")]
    WristLScrub,
    [Description("Right Wrist Switch")]
    WristRSwitch,
    [Description("Right Wrist Scrub")]
    WristRScrub,
    [Description("Left Thumb Switch")]
    ThumbLSwitch,
    [Description("Left Thumb Scrub")]
    ThumbLScrub,
    [Description("Right Thumb Switch")]
    ThumbRSwitch,
    [Description("Right Thumb Scrub")]
    ThumbRScrub,
    [Description("Between Fingers Switch")]
    BetweenFingersSwitch,
    [Description("Between Fingers Scrub")]
    BetweenFingersScrub,
    [Description("Left Fingernails Switch")]
    FingernailsLSwitch,
    [Description("Left Fingernails Scrub")]
    FingernailsLScrub,
    [Description("Right Fingernails Switch")]
    FingernailsRSwitch,
    [Description("Right Fingernails Scrub")]
    FingernailsRScrub
}
