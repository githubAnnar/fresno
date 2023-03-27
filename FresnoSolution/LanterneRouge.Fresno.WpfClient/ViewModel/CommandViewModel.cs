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
        public CommandViewModel(string displayName, ICommand command, string toolTipText = null)
        {
            base.DisplayName = displayName;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            ToolTipText = toolTipText;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Gets the Tooltip if any
        /// </summary>
        public string ToolTipText { get; private set; }
    }
}
