using System;
using System.ComponentModel;

[Serializable]
public enum Gender 
{
    [Description("Boy")]
    Boy,
    [Description("Girl")]
    Girl,
    [Description("Other")]
    Other
}
