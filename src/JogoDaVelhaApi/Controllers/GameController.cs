using JogoDaVelha.Application.IAppService;
using JogoDaVelha.Application.ViewModels;
using System;
using System.Linq;
using System.Web.Http;

namespace JogoDaVelhaApi.Controllers
{
    public class GameController : ApiController
    {
        private readonly IGameAppService _gameAppService;

        public GameController(IGameAppService testeIocAppService)
        {
            _gameAppService = testeIocAppService;
        }


        /// <summary>
        /// Inicia o Jogo da Velha, criando uma nova partida.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public GameMessagemModel NewGame()
        {
            var result = _gameAppService.StartNewGame();

            if (!result.ResultValidation.IsValid)
                result.Msg = result.ResultValidation.GetErros();

            return result;
        }

        /// <summary>
        /// Executa o movimento de cada jogador na partida
        /// </summary>
        /// <remarks>
        /// RN1 - O campo "Id" deverá conter o identificador da partida gerada no méto NewGame.<br/>
        /// RN2 - O campo "Player" deverá conter o valor de cada jogador ("X" ou "O").<br/>
        /// RN3 - O campo "Position" deverá conter as coordenadas [X, Y] de onde será efetuada o movimento.<br/>
        /// <br/>
        /// Posições do Tabuleiro:  
        /// [0,0]   [1,0]	[2,0]<br/>
        /// [0,1]	[1,1]	[2,1]<br/>
        /// [0,2]	[1,2]	[2,2]<br/>
        /// <br/>
        /// Exemplo de Entrada:<br/>
        /// {<br/>
        ///     "Id": "38a7edc7-d15b-4c95-8d0e-af6ecb2120c6",<br/>
        ///     "Player": "x",<br/>
        ///     "Position": {<br/>
        ///         "X": 2,<br/>
        ///         "Y": 1<br/>
        ///     }<br/>
        /// }<br/>
        /// </remarks>
        /// <param name="movement">Objeto contendo as instruções de movimento para um jogador e partida</param>
        /// <returns></returns>
        [HttpPost]
        public MovementMessageModel Movement([FromBody]MovementModel movement)
        {
            var result = _gameAppService.SetMovement(movement);

            if (!result.ResultValidation.IsValid)
                result.Msg = result.ResultValidation.GetErros();

            return result;
        }
    }



}
