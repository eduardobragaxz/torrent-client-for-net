using DefensiveProgrammingFramework;

namespace TorrentClient.EventArgs;

/// <summary>
/// The torrent stopped event arguments.
/// </summary>
public sealed class TorrentStoppedEventArgs : System.EventArgs
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TorrentStoppedEventArgs" /> class.
    /// </summary>
    /// <param name="torrentInfo">The torrent information.</param>
    public TorrentStoppedEventArgs(TorrentInfo torrentInfo)
    {
        torrentInfo.CannotBeNull();

        TorrentInfo = torrentInfo;
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="TorrentStoppedEventArgs"/> class from being created.
    /// </summary>
    private TorrentStoppedEventArgs()
    {
    }

    #endregion Private Constructors

    #region Public Properties

    /// <summary>
    /// Gets the torrent information.
    /// </summary>
    /// <value>
    /// The torrent information.
    /// </value>
    public TorrentInfo? TorrentInfo
    {
        get;
        private set;
    }

    #endregion Public Properties
}
