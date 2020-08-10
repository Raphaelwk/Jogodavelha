using JogoDaVelha.Application.IAppService;
using JogoDaVelha.Application.Lib;
using JogoDaVelha.Application.ViewModels;
using JogoDaVelha.CrossCutting.Lib.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoDaVelha.Application.AppService
{
    public class GameAppService : IGameAppService
    {
        public GameMessagemModel StartNewGame()
        {
            var result = new GameMessagemModel();
            try
            {
                var game = GameModel.GameModelFactory.StartNewGame();

                //Prepara resultado
                result.Id = game.Id.ToString();
                result.FirstPlayer = game.PlayerTurn;
                result.Status = game.StatusGame.GetDescription();

                return result;
            }
            catch (Exception ex)
            {
                result.ResultValidation.AddErro($@"Erro ao iniciar o jogo. {ex.Message}");
                return result;
            }
        }

        public MovementMessageModel SetMovement(MovementModel movementModel)
        {
            var result = new MovementMessageModel();
            result.Id = movementModel.Id.ToString();

            try
            {
                //Recupera o jogo
                var gameModel = FileLib.GetJsonFile(movementModel.Id);


                //Executa a validação dos objetos
                result.ResultValidation = gameModel.ValidaGame(movementModel);


                //Se inválido retorna o resultado da validação
                if (!result.ResultValidation.IsValid)
                    return result;


                //Executa o movimento
                GameModel.GameModelFactory.SetMovement(gameModel, movementModel.Position.X, movementModel.Position.Y, movementModel.Player);


                //TODO: Verifica o Termino da partida
                VerificaStatusPartida(gameModel);

                //Atualiza o jsonFile
                FileLib.CreateJsonFile(gameModel);


                result.PlayerTurn = gameModel.PlayerTurn;
                result.Winner = gameModel.Winner;
                result.Status = gameModel.StatusGame.GetDescription();

                return result;
            }
            catch (Exception ex)
            {
                result.ResultValidation.AddErro($@"Erro ao executar movimento. {ex.Message}");
                return result;
            }
        }


        public void VerificaStatusPartida(GameModel gameModel)
        {
            //Verifica combinação nas linhas
            for (int row = 0; row < 3; row++)
            {
                if (string.IsNullOrEmpty(gameModel.Tabuleiro[row, 0]) ||
                    string.IsNullOrEmpty(gameModel.Tabuleiro[row, 1]) ||
                    string.IsNullOrEmpty(gameModel.Tabuleiro[row, 2]))
                    continue;

                if (gameModel.Tabuleiro[row, 0].Equals(gameModel.Tabuleiro[row, 1]) &&
                    gameModel.Tabuleiro[row, 1].Equals(gameModel.Tabuleiro[row, 2]))
                {
                    gameModel.StatusGame = StatusGameEnum.TemVencedor;
                    gameModel.Winner = gameModel.Tabuleiro[row, 0].ToString();
                    return;
                }
            }


            //Verifica combinação nas colunas
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(gameModel.Tabuleiro[0, col]) ||
                    string.IsNullOrEmpty(gameModel.Tabuleiro[1, col]) ||
                    string.IsNullOrEmpty(gameModel.Tabuleiro[2, col]))
                    continue;

                if (gameModel.Tabuleiro[0, col].Equals(gameModel.Tabuleiro[1, col]) &&
                    gameModel.Tabuleiro[1, col].Equals(gameModel.Tabuleiro[2, col]))
                {
                    gameModel.StatusGame = StatusGameEnum.TemVencedor;
                    gameModel.Winner = gameModel.Tabuleiro[0, col].ToString();
                    return;
                }
            }


            //Verifica combinação nas diagonais
            if (!string.IsNullOrEmpty(gameModel.Tabuleiro[0, 0]) &&
                !string.IsNullOrEmpty(gameModel.Tabuleiro[1, 1]) &&
                !string.IsNullOrEmpty(gameModel.Tabuleiro[2, 2]) &&
                gameModel.Tabuleiro[0, 0].Equals(gameModel.Tabuleiro[1, 1]) &&
                gameModel.Tabuleiro[1, 1].Equals(gameModel.Tabuleiro[2, 2]))
            {
                gameModel.StatusGame = StatusGameEnum.TemVencedor;
                gameModel.Winner = gameModel.Tabuleiro[0, 0].ToString();
                return;
            }

            if (!string.IsNullOrEmpty(gameModel.Tabuleiro[0, 2]) &&
                !string.IsNullOrEmpty(gameModel.Tabuleiro[1, 1]) &&
                !string.IsNullOrEmpty(gameModel.Tabuleiro[2, 0]) &&
                gameModel.Tabuleiro[0, 2].Equals(gameModel.Tabuleiro[1, 1]) &&
                gameModel.Tabuleiro[1, 1].Equals(gameModel.Tabuleiro[2, 0]))
            {
                gameModel.StatusGame = StatusGameEnum.TemVencedor;
                gameModel.Winner = gameModel.Tabuleiro[0, 2].ToString();
                return;
            }



            //Verifica se possui jogadas pendentes
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (string.IsNullOrEmpty(gameModel.Tabuleiro[x, y]))
                    {
                        gameModel.StatusGame = StatusGameEnum.EmAndamento;
                        gameModel.Winner = string.Empty;
                        return;
                    }
                }
            }


            gameModel.StatusGame = StatusGameEnum.Empate;
            gameModel.Winner = "Draw";
        }

    }
}
