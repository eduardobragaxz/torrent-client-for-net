using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The request message test.
/// </summary>
[TestClass]
public class RequestMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void RequestMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "0000000D06000000050000000600000007".ToByteArray();

        if (RequestMessage.TryDecode(data, ref offset, data.Length, out RequestMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(17, message.Length);
            Assert.AreEqual(5, message.PieceIndex);
            Assert.AreEqual(6, message.BlockOffset);
            Assert.AreEqual(7, message.BlockLength);
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