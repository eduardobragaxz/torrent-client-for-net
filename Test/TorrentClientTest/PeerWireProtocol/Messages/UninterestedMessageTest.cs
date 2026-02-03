using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The interested message test.
/// </summary>
[TestClass]
public class UninterestedMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void UninterestedMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "0000000103".ToByteArray();

        if (UninterestedMessage.TryDecode(data, ref offset, data.Length, out UninterestedMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(5, message.Length);
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