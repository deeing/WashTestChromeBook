using System.ComponentModel;
public enum GermType
{
    NO_TYPE,
    [Description("Palms")]
    Palm,
    [Description("Fingertips")]
    Fingertips,
    [Description("Back of Left Hand")]
    BackOfHandL,
    [Description("Back of Right Hand")]
    BackOfHandR,
    [Description("Left Wrist")]
    WristL,
    [Description("Left Thumb")]
    ThumbL,
    [Description("Right Thumb")]
    ThumbR,
    [Description("Between Fingers")]
    BetweenFingers,
    [Description("Left Fingernails")]
    FingernailsL,
    [Description("Right Fingernails")]
    FingernailsR,
}
