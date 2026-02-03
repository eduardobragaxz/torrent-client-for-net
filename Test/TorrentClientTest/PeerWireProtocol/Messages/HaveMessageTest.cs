using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The have message test.
/// </summary>
[TestClass]
public class HaveMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void HaveMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "0000000504000000AA".ToByteArray();

        if (HaveMessage.TryDecode(data, ref offset, data.Length, out HaveMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(9, message.Length);
            Assert.AreEqual(170, message.PieceIndex);
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