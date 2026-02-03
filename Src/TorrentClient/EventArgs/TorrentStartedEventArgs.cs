using DefensiveProgrammingFramework;

namespace TorrentClient.EventArgs;

/// <summary>
/// The torrent starting event arguments.
/// </summary>
public sealed class TorrentStartedEventArgs : System.EventArgs
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TorrentStartedEventArgs" /> class.
    /// </summary>
    /// <param name="torrentInfo">The torrent information.</param>
    public TorrentStartedEventArgs(TorrentInfo torrentInfo)
    {
        torrentInfo.CannotBeNull();

        TorrentInfo = torrentInfo;
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="TorrentStartedEventArgs"/> class from being created.
    /// </summary>
    private TorrentStartedEventArgs()
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
