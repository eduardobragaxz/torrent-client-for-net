using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The piece message test.
/// </summary>
[TestClass]
public class PieceMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void TestTryDecodePieceMessge()
    {
        int offset = 0;
        byte[] data = "0000000B070000000500000006ABCD".ToByteArray();

        if (PieceMessage.TryDecode(data, ref offset, data.Length, out PieceMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(15, message.Length);
            Assert.AreEqual(5, message.PieceIndex);
            Assert.AreEqual(6, message.BlockOffset);
            Assert.AreEqual(171, message.Data[0]);
            Assert.AreEqual(205, message.Data[1]);
            Assert.IsFalse(isIncomplete);
            Assert.AreEqual(data.Length, offset);
            CollectionAssert.AreEqual(data, message.Encode());
        }
        else
        {
            Assert.Fail();
        }
    }

    #endregion Public Methods
}