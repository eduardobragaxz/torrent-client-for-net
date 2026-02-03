using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The port message test.
/// </summary>
[TestClass]
public class PortMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void PortMessage_TryDecode()
    {
        int offset = 0;
        byte[] data = "00000003090FAC".ToByteArray();

        if (PortMessage.TryDecode(data, ref offset, data.Length, out PortMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(7, message.Length);
            Assert.AreEqual(4012, message.Port);
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