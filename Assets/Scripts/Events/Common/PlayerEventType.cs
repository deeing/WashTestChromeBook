using System.ComponentModel;
public enum PlayerEventType
{
    [Description("PALMS")]
    PalmSwitch,
    [Description("Palms")]
    PalmScrub,
    [Description("FINGERTIPS - Left")]
    FingertipsLSwitch,
    [Description("Fingertips Left Scrub")]
    FingertipsLScrub,
    [Description("FINGERTIPS - Right")]
    FingertipsRSwitch,
    [Description("Fingertips Right")]
    FingertipsRScrub,
    [Description("BACK - Left")]
    BackOfHandLSwitch,
    [Description("Back Left")]
    BackOfHandLScrub,
    [Description("BACK - Right")]
    BackOfHandRSwitch,
    [Description("Back Right")]
    BackOfHandRScrub,
    [Description("WRIST - Left")]
    WristLSwitch,
    [Description("Left Wrist")]
    WristLScrub,
    [Description("WRIST - Right")]
    WristRSwitch,
    [Description("Right Wrist")]
    WristRScrub,
    [Description("THUMB - Left")]
    ThumbLSwitch,
    [Description("Left Thumb")]
    ThumbLScrub,
    [Description("THUMB - Right")]
    ThumbRSwitch,
    [Description("Right Thumb")]
    ThumbRScrub,
    [Description("BETWEEN")]
    BetweenFingersSwitch,
    [Description("Between Fingers")]
    BetweenFingersScrub,
    [Description("NAILS - Left")]
    FingernailsLSwitch,
    [Description("Left Fingernails")]
    FingernailsLScrub,
    [Description("NAILS - Right")]
    FingernailsRSwitch,
    [Description("Right Fingernails")]
    FingernailsRScrub,
    [Description("Non linear Switch")]
    NonLinearSwitch,
    [Description("Wet")]
    WetSwitch,
    [Description("Wet")]
    WetScrub,
    [Description("Soap")]
    SoapSwitch,
    [Description("Soap")]
    SoapScrub,
    [Description("Rinse")]
    Rinse,
    [Description("Wet")]
    Wet,
    [Description("Towel")]
    Towel
}
