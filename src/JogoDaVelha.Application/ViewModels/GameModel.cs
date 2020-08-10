using JogoDaVelha.Application.Lib;
using JogoDaVelha.Application.Validation;
using JogoDaVelha.CrossCutting.Lib.Enumerators;
using System;
using System.Collections.Generic;

namespace JogoDaVelha.Application.ViewModels
{
    public class GameModel
    {

        public Guid Id { get; set; }
        public string PlayerTurn { get; set; }
        public StatusGameEnum StatusGame { get; set; }
        public string[,] Tabuleiro { get; set; }
        public string Winner { get; set; }


        public ValidationAppResult ValidaGame(MovementModel movementModel)
        {
            var appResult = new ValidationAppResult();

            if (StatusGame == StatusGameEnum.TemVencedor || StatusGame == StatusGameEnum.Empate)
            {
                appResult.AddErro($@"Não é possível efetuar a jogada, pois a partida encerrada!");
                return appResult;
            }

            //Verifica player vazio
            if (string.IsNullOrEmpty(movementModel.Player))
                appResult.AddErro($@"Favor informar o Player que esta realizando a jogada!");

            //Verifica player diferente de X ou y
            if (movementModel.Player.ToUpper() != "X" && movementModel.Player.ToUpper() != "O")
                appResult.AddErro($@"Os valores aceitos para o campo Player são 'X' ou 'O'");

            //Verifica o turno do jogador
            if (!movementModel.Player.ToUpper().Equals(PlayerTurn))
                appResult.AddErro($@"Não é o turno do jogador {movementModel.Player.ToUpper()}!");

            //Validar se x < 0 ou x > 2
            if (movementModel.Position.X < 0 || movementModel.Position.X > 2)
                appResult.AddErro($@"Movimento inválido, a posição 'X' tem que estar entre 0 e 2!");

            //Validar se y < 0 ou y > 2
            if (movementModel.Position.X < 0 || movementModel.Position.X > 2)
                appResult.AddErro($@"Movimento inválido, a posição 'Y' tem que estar entre 0 e 2!");

            //Validar se a posição xy já foi utilizada
            if (!Tabuleiro[movementModel.Position.X, movementModel.Position.Y].Equals(string.Empty))
                appResult.AddErro($@"Movimento inválido, a posição 'X'{movementModel.Position.X}'Y'{movementModel.Position.Y} já esta sendo utilizada!");



            return appResult;
        }


        public static class GameModelFactory
        {
            public static GameModel StartNewGame()
            {
                var gameModel = new GameModel()
                {
                    Id = Guid.NewGuid(),
                    PlayerTurn = RandoPlayer(),
                    StatusGame = StatusGameEnum.Iniciada,
                    Tabuleiro = InicializaTabuleiro()
                };

                FileLib.CreateJsonFile(gameModel);

                return gameModel;
            }

            public static void SetMovement(GameModel gameModel, int x, int y, string player)
            {
                gameModel.Tabuleiro[x, y] = player.ToUpper();
                ChangePlayer(gameModel);
                gameModel.StatusGame = StatusGameEnum.EmAndamento;
            }

            private static void ChangePlayer(GameModel gameModel)
            {
                gameModel.PlayerTurn = gameModel.PlayerTurn.Equals("X") ? "O" : "X";
            }

            private static string RandoPlayer()
            {
                var list = new List<string> { "X", "O" };
                Random r = new Random();
                return list[r.Next(list.Count)];
            }

            private static string[,] InicializaTabuleiro()
            {
                var tab = new string[3, 3];

                //Zerando o tabuleiro
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        tab[x, y] = string.Empty;
                    }
                }
                return tab;
            }
        }
    }
}
