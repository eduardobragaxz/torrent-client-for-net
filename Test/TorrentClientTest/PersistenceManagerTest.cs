using TorrentClient;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol;

namespace TorrentClientTest;

/// <summary>
/// The persistence manager test.
/// </summary>
[TestClass]
public class PersistenceManagerTest
{
    #region Public Methods

    /// <summary>
    /// Tests the persistence manager transfer.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="destPath">The dest path.</param>
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent1.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent2.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent3.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent4.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent5.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent6.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent7.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent8.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent9.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent10.torrent", @".\Test")]
    [TestMethod]
    public void PersistenceManager_Test(string sourcePath, string destPath)
    {
        if (TorrentInfo.TryLoad(sourcePath, out TorrentInfo torrent))
        {
            using (PersistenceManager src = new(Path.GetDirectoryName(sourcePath)!, torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files))
            {
                using PersistenceManager dest = new(destPath, torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files);
                for (int pieceIndex = 0; pieceIndex < torrent.PiecesCount; pieceIndex++)
                {
                    dest.Put(torrent.Files, torrent.PieceLength, pieceIndex, src.Get(pieceIndex));
                }

                Assert.IsTrue(dest.Verify().All(x => x == PieceStatus.Present));
            }

            destPath.DeleteDirectoryRecursively();
        }
        else
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// Tests the persistence manager transfer.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="destPath">The dest path.</param>
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent1.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent2.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent3.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent4.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent5.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent6.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent7.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent8.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent9.torrent", @".\Test")]
    [DataRow(@"..\..\..\TorrentClientTest.Data\Torrent10.torrent", @".\Test")]
    [TestMethod]
    public void PersistenceManager_Test2(string sourcePath, string destPath)
    {
        PersistenceManager src;

        if (TorrentInfo.TryLoad(sourcePath, out TorrentInfo torrent))
        {
            src = new PersistenceManager(Path.GetDirectoryName(sourcePath)!, torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files);

            Assert.IsTrue(src.Verify().All(x => x == PieceStatus.Present));
        }
        else
        {
            Assert.Fail();
        }
    }

    #endregion Public Methods
}