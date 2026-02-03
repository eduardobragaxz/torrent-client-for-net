using Microsoft.VisualStudio.TestTools.UnitTesting;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.Messages;

namespace TorrentClientTest.PeerWireProtocol.Messages;

/// <summary>
/// The interested message test.
/// </summary>
[TestClass]
public class InterestedMessageTest
{
    #region Public Methods

    /// <summary>
    /// Tests the TryDecode() method.
    /// </summary>
    [TestMethod]
    public void InterestedMessage_TryDecode()
    {
        int offsetFrom = 0;
        byte[] data = "0000000102".ToByteArray();

        if (InterestedMessage.TryDecode(data, ref offsetFrom, data.Length, out InterestedMessage message, out bool isIncomplete))
        {
            Assert.AreEqual(5, message.Length);
            Assert.IsFalse(isIncomplete);
            CollectionAssert.AreEqual(data, message.Encode());
        }
        else
        {
            Assert.Fail();
        }
    }

    #endregion Public Methods
}