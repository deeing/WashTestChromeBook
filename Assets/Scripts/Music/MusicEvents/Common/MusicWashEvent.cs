using RhythmTool;

public interface MusicWashEvent 
{
    void DoEvent(Beat beat);
    void SetupEvent();
    PlayerEventType GetEventType();
    bool IsFinished();
    MusicWashEvent GetNextWashEvent();
    void EndEvent();
    float GetScore();
}
