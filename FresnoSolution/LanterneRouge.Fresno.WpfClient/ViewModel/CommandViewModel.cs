using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="command">The command.</param>
        public CommandViewModel(string displayName, ICommand command)
        {
            base.DisplayName = displayName;
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}
