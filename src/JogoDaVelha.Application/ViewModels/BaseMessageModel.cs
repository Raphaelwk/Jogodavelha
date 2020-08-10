using JogoDaVelha.Application.Validation;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace JogoDaVelha.Application.ViewModels
{
    public class BaseMessageModel
    {
        public string Id { get; set; }
        public string Msg { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        public ValidationAppResult ResultValidation
        {
            get
            {
                if (_ResultValidation == null)
                    _ResultValidation = new ValidationAppResult();

                return _ResultValidation;
            }
            set { _ResultValidation = value; }
        }
        private ValidationAppResult _ResultValidation;
    }
}
