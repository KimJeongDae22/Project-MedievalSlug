/// <summary>
/// StateMachine 기본 상태
/// </summary>
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}