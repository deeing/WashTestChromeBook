using System.ComponentModel;
public enum GermType
{
    NO_TYPE,
    [Description("Palms")]
    Palm,
    [Description("Fingertips Left")]
    FingertipsL,
    [Description("Fingertips Right")]
    FingertipsR,
    [Description("Back of Left Hand")]
    BackOfHandL,
    [Description("Back of Right Hand")]
    BackOfHandR,
    [Description("Left Wrist")]
    WristL,
    [Description("Right Wrist")]
    WristR,
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
