using TorrentClient;
using TorrentClient.Extensions;

namespace TorrentClientTest;

/// <summary>
/// The transfer manager test.
/// </summary>
[TestClass]
public class TransferManagerTest
{
    #region Public Methods

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        @"Test\".DeleteDirectoryRecursively();
    }

    /// <summary>
    /// Tests the peer transfer.
    /// </summary>
    [TestMethod]
    public void TestTransferManager()
    {
        PersistenceManager pm;
        ThrottlingManager tm;
        TransferManager transfer;

        TorrentInfo.TryLoad(@"C:\Users\aljaz\Desktop\Torrent1\Torrent2.torrent", out TorrentInfo torrent);

        tm = new ThrottlingManager
        {
            WriteSpeedLimit = 1024 * 1024,
            ReadSpeedLimit = 1024 * 1024
        };

        pm = new PersistenceManager(@"Test\", torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files);

        transfer = new TransferManager(4000, torrent, tm, pm);
        transfer.Start();

        Thread.Sleep(1000000);
    }

    #endregion Public Methods
}