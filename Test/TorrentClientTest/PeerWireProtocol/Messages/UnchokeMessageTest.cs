using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The un-choke message test.
/// </summary>
[TestClass]
public class UnchokeMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void UnchokeMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "0000000101".ToByteArray();

        if (UnchokeMessage.TryDecode(data, ref offset, data.Length, out UnchokeMessage message, out bool isIncomplete))
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