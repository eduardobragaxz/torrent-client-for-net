using System.Collections.Generic;
using DefensiveProgrammingFramework;
using TorrentClient.Extensions;

namespace TorrentClient.PeerWireProtocol.Messages;

/// <summary>
/// The peer wire protocol message base class.
/// </summary>
public abstract class PeerMessage : Message
{
    #region Public Fields

    /// <summary>
    /// The default block length in bytes (16kB)
    /// </summary>
    public const int DefaultBlockLength = 16384;

    #endregion Public Fields

    #region Public Methods

    /// <summary>
    /// Decodes the messages in the buffer.
    /// </summary>
    /// <param name="buffer">The data.</param>
    /// <param name="offsetStart">The offset.</param>
    /// <param name="offsetEnd">The offset end.</param>
    /// <returns>
    /// The list of messages.
    /// </returns>
    public static IEnumerable<PeerMessage> Decode(byte[] buffer, ref int offsetStart, int offsetEnd)
    {
        buffer.CannotBeNull();
        offsetStart.MustBeGreaterThanOrEqualTo(0);
        offsetStart.MustBeLessThanOrEqualTo(buffer.Length);
        offsetEnd.MustBeGreaterThan(0);
        offsetEnd.MustBeLessThanOrEqualTo(buffer.Length);

        List<PeerMessage> messages = [];
        int offset = offsetStart;

        // walk through the array and try to decode messages
        while (offset <= offsetEnd)
        {
            if (TryDecode(buffer, ref offset, offsetEnd, out PeerMessage message, out bool isIncomplete))
            {
                // successfully decoded message
                messages.Add(message);

                // remember where we left off
                offsetStart = offset;
            }
            else if (isIncomplete)
            {
                // message of variable length is present but incomplete -> stop advancing
                break;
            }
            else
            {
                // move to next byte
                offset++;
            }
        }

        return messages;
    }

    /// <summary>
    /// Decodes the message.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="offsetFrom">The offset from.</param>
    /// <param name="offsetTo">The offset to.</param>
    /// <param name="message">The message.</param>
    /// <param name="isIncomplete">if set to <c>true</c> the message is incomplete.</param>
    /// <returns>
    /// True if decoding was successful; false otherwise.
    /// </returns>
    public static bool TryDecode(byte[] buffer, ref int offsetFrom, int offsetTo, out PeerMessage message, out bool isIncomplete)
    {
        byte messageId;
        int messageLength;
        int offset2 = offsetFrom;

        message = null;
        isIncomplete = false;

        if (buffer.IsNotNullOrEmpty() &&
            buffer.Length >= offsetFrom + Message.IntLength + Message.ByteLength)
        {
            messageLength = Message.ReadInt(buffer, ref offset2);
            messageId = Message.ReadByte(buffer, ref offset2);

            offset2 = offsetFrom; // reset offset

            ////if (messageLength == 0)
            ////{
            ////	KeepAliveMessage message2;
            ////	KeepAliveMessage.TryDecode(buffer, ref offset2, out message2);

            ////	message = message2;
            ////}
            if (messageId == ChokeMessage.MessageId)
            {
                ChokeMessage.TryDecode(buffer, ref offset2, offsetTo, out ChokeMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == UnchokeMessage.MessageId)
            {
                UnchokeMessage.TryDecode(buffer, ref offset2, offsetTo, out UnchokeMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == InterestedMessage.MessageId)
            {
                InterestedMessage.TryDecode(buffer, ref offset2, offsetTo, out InterestedMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == UninterestedMessage.MessageId)
            {
                UninterestedMessage.TryDecode(buffer, ref offset2, offsetTo, out UninterestedMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == HaveMessage.MessageId)
            {
                HaveMessage.TryDecode(buffer, ref offset2, offsetTo, out HaveMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == BitFieldMessage.MessageId)
            {
                BitFieldMessage.TryDecode(buffer, ref offset2, offsetTo, out BitFieldMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == RequestMessage.MessageId)
            {
                RequestMessage.TryDecode(buffer, ref offset2, offsetTo, out RequestMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == CancelMessage.MessageId)
            {
                CancelMessage.TryDecode(buffer, ref offset2, offsetTo, out CancelMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == PortMessage.MessageId)
            {
                PortMessage.TryDecode(buffer, ref offset2, offsetTo, out PortMessage message2, out isIncomplete);

                message = message2;
            }
            else if (messageId == PieceMessage.MessageId)
            {
                PieceMessage.TryDecode(buffer, ref offset2, offsetTo, out PieceMessage message2, out isIncomplete);

                message = message2;
            }
            else
            {
                HandshakeMessage.TryDecode(buffer, ref offset2, offsetTo, out HandshakeMessage message2, out isIncomplete);

                message = message2;
            }
        }

        if (message != null)
        {
            offsetFrom = offset2;
        }

        return message != null;
    }

    #endregion Public Methods
}
