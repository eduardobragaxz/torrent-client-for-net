using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The keep alive message test.
/// </summary>
[TestClass]
public class KeepAliveMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void KeepAliveMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "00000000".ToByteArray();

        if (KeepAliveMessage.TryDecode(data, ref offset, out KeepAliveMessage message))
        {
            Assert.AreEqual(4, message.Length);
            CollectionAssert.AreEqual(data, message.Encode());
        }
        else
        {
            Assert.Fail();
        }
    }

    #endregion Public Methods
}