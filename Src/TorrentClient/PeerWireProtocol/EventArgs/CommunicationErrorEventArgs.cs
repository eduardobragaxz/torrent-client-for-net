using DefensiveProgrammingFramework;

namespace TorrentClient.PeerWireProtocol.EventArgs;

/// <summary>
/// The communication error event arguments.
/// </summary>
public sealed class CommunicationErrorEventArgs : System.EventArgs
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicationErrorEventArgs" /> class.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public CommunicationErrorEventArgs(string errorMessage)
    {
        errorMessage.CannotBeNullOrEmpty();

        ErrorMessage = errorMessage;
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="CommunicationErrorEventArgs"/> class from being created.
    /// </summary>
    private CommunicationErrorEventArgs()
    {
    }

    #endregion Private Constructors

    #region Public Properties

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value>
    /// The error message.
    /// </value>
    public string? ErrorMessage
    {
        get;
        private set;
    }

    #endregion Public Properties
}
