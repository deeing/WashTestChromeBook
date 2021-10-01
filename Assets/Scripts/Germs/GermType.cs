using System.ComponentModel;
public enum GermType
{
    NO_TYPE,
    [Description("Palm")]
    Palm,
    [Description("Fingertips")]
    Fingertips,
    [Description("Back of Left Hand")]
    BackOfHandL,
    [Description("Back of Right Hand")]
    BackOfHandR,
    [Description("Left Wrist")]
    WristL
}
