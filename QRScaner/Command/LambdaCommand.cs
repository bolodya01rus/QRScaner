using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRScaner.Command
{
    internal class LambdaCommand : Base.Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool>? _CanExecute;
        public LambdaCommand(Action<object> Execute, Func<object, bool>? CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        public override bool CanExecute(object? parameter)
        {
           // if (parameter == null) throw new ArgumentNullException();
            return _CanExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object? parameter)
        {
            //if (parameter == null) throw new ArgumentNullException();
            if (!CanExecute(parameter)) return;
            _Execute(parameter);

        }

    }
}
