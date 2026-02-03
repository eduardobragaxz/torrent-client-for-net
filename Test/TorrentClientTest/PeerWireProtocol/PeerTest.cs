using System.Net;
using System.Net.Sockets;
using TorrentClient;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol;

namespace TorrentClientTest.PeerWireProtocol;

/// <summary>
/// The peer test.
/// </summary>
[TestClass]
public class PeerTest
{
    #region Public Methods

    /// <summary>
    /// Tests the peer transfer.
    /// </summary>
    [TestMethod]
    public void TestPeerTransfer()
    {
        Peer peer;
        IPEndPoint endpoint = new(IPAddress.Parse("192.168.0.10"), 4009);
        TcpClient tcp;
        byte[] data;
        PieceManager pm;
        ThrottlingManager tm;
        PieceStatus[] bitField;

        TorrentInfo.TryLoad(@"C:\Users\aljaz\Desktop\Torrent1\Torrent2.torrent", out TorrentInfo torrent);

        tcp = new TcpClient
        {
            ReceiveBufferSize = 1024 * 1024,
            SendBufferSize = 1024 * 1024
        };
        tcp.Connect(endpoint);

        data = new byte[torrent.Length];

        tm = new ThrottlingManager
        {
            WriteSpeedLimit = 1024 * 1024,
            ReadSpeedLimit = 1024 * 1024
        };

        bitField = new PieceStatus[torrent.PiecesCount];

        pm = new PieceManager(torrent.InfoHash, torrent.Length, torrent.PieceHashes, torrent.PieceLength, torrent.BlockLength, bitField);
        pm.PieceCompleted += (sender, e) => Assert.AreEqual(torrent.PieceHashes.ElementAt(e.PieceIndex), e.PieceData.CalculateSha1Hash().ToHexaDecimalString());

        peer = new Peer(new PeerCommunicator(tm, tcp), pm, "-UT3530-B9731F4C29D30E7DEA1F9FA7");
        peer.CommunicationErrorOccurred += (sender, e) => Assert.Fail();

        Thread.Sleep(1000000);
    }

    #endregion Public Methods
}