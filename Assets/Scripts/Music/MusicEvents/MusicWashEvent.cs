using RhythmTool;

public interface MusicWashEvent 
{
    void DoEvent(Beat beat);
    void SetupEvent();
    PlayerEventType GetEventType();
}
