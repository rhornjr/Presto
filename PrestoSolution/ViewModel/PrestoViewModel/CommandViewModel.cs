using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PrestoViewModel
{
    public class CommandViewModel
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            this.DisplayName = displayName;
            this.Command = command;
        }

        public string DisplayName { get; set; }
        public ICommand Command { get; private set; }
    }
}
