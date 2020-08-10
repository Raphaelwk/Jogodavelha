using JogoDaVelha.Application.ViewModels;

namespace JogoDaVelha.Application.IAppService
{
    public interface IGameAppService
    {
        GameMessagemModel StartNewGame();
        MovementMessageModel SetMovement(MovementModel movementModel);
    }
}
