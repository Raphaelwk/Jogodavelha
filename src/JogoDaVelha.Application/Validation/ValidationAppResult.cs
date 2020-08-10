using System;
using System.Collections.Generic;
using System.Linq;

namespace JogoDaVelha.Application.Validation
{
    public class ValidationAppResult
    {
        private readonly List<ValidationAppError> _errors = new List<ValidationAppError>();

        public string Mensagem { get; set; }
        public bool IsValid
        {
            get { return _errors.Count == 0; }
            set
            {
                var b = value;
            }
        }

        public ICollection<ValidationAppError> Erros { get { return this._errors; } }

        public void AddErro(string message)
        {
            Erros.Add(new ValidationAppError(message));
        }

        public string GetErros()
        {
            string erros = string.Empty;
            Erros.ToList().ForEach(x => erros = erros + x.Message + "  " + Environment.NewLine);
            return erros;
        }
    }
}
