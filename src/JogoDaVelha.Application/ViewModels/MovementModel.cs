using System;

namespace JogoDaVelha.Application.ViewModels
{
    public class MovementModel
    {
        public Guid Id { get; set; }
        public string Player { get; set; }
        public PositionModel Position { get; set; }
    }
}