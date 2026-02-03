using DefensiveProgrammingFramework;

namespace TorrentClient.EventArgs;

/// <summary>
/// The torrent seeding event arguments.
/// </summary>
public sealed class TorrentSeedingEventArgs : System.EventArgs
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TorrentSeedingEventArgs" /> class.
    /// </summary>
    /// <param name="torrentInfo">The torrent information.</param>
    public TorrentSeedingEventArgs(TorrentInfo torrentInfo)
    {
        torrentInfo.CannotBeNull();

        TorrentInfo = torrentInfo;
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="TorrentSeedingEventArgs"/> class from being created.
    /// </summary>
    private TorrentSeedingEventArgs()
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
