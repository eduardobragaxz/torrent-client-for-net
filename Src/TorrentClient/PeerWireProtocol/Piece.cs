using System;
using System.Globalization;
using DefensiveProgrammingFramework;
using TorrentClient.Extensions;
using TorrentClient.PeerWireProtocol.EventArgs;

namespace TorrentClient.PeerWireProtocol;

/// <summary>
/// The piece.
/// </summary>
public sealed class Piece
{
    #region Private Fields

    /// <summary>
    /// The completed block count.
    /// </summary>
    private int completedBlockCount = 0;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Piece" /> class.
    /// </summary>
    /// <param name="pieceIndex">Index of the piece.</param>
    /// <param name="pieceHash">The piece hash.</param>
    /// <param name="pieceLength">Length of the piece.</param>
    /// <param name="blockLength">Length of the block.</param>
    /// <param name="blockCount">The block count.</param>
    /// <param name="pieceData">The piece data.</param>
    /// <param name="bitField">The bit field.</param>
    public Piece(int pieceIndex, string pieceHash, long pieceLength, int blockLength, int blockCount, byte[] pieceData = null, bool[] bitField = null)
    {
        pieceIndex.MustBeGreaterThanOrEqualTo(0);
        pieceHash.CannotBeNullOrEmpty();
        pieceLength.MustBeGreaterThan(0);
        blockCount.MustBeGreaterThan(0);
        pieceData.IsNotNull().Then(() => pieceData.LongLength.MustBeEqualTo(pieceLength));
        bitField.IsNotNull().Then(() => bitField.LongLength.MustBeEqualTo(blockCount));

        PieceIndex = pieceIndex;
        PieceHash = pieceHash;
        PieceLength = pieceLength;
        PieceData = pieceData ?? new byte[PieceLength];

        BlockLength = blockLength;
        BlockCount = blockCount;

        IsCompleted = false;
        IsCorrupted = false;

        BitField = bitField ?? new bool[blockCount];
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="Piece"/> class from being created.
    /// </summary>
    private Piece()
    {
    }

    #endregion Private Constructors

    #region Public Events

    /// <summary>
    /// Occurs when piece has completed.
    /// </summary>
    public event EventHandler<PieceCompletedEventArgs> Completed;

    /// <summary>
    /// Occurs when piece has become corrupted.
    /// </summary>
    public event EventHandler<System.EventArgs> Corrupted;

    #endregion Public Events

    #region Public Properties

    /// <summary>
    /// Gets the bit field.
    /// </summary>
    /// <value>
    /// The bit field.
    /// </value>
    public bool[] BitField
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the block count.
    /// </summary>
    /// <value>
    /// The block count.
    /// </value>
    public int BlockCount
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the length of the block in bytes.
    /// </summary>
    /// <value>
    /// The length of the block in bytes.
    /// </value>
    public int BlockLength
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether the piece is completed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the piece is completed; otherwise, <c>false</c>.
    /// </value>
    public bool IsCompleted
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether the piece is corrupted.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the piece is corrupted; otherwise, <c>false</c>.
    /// </value>
    public bool IsCorrupted
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the piece data.
    /// </summary>
    /// <value>
    /// The piece data.
    /// </value>
    public byte[] PieceData
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the piece hash.
    /// </summary>
    /// <value>
    /// The piece hash.
    /// </value>
    public string PieceHash
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the index of the piece.
    /// </summary>
    /// <value>
    /// The index of the piece.
    /// </value>
    public int PieceIndex
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the length of the piece.
    /// </summary>
    /// <value>
    /// The length of the piece.
    /// </value>
    public long PieceLength
    {
        get;
        private set;
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Gets the block.
    /// </summary>
    /// <param name="blockOffset">The block offset.</param>
    /// <returns>The block data.</returns>
    public byte[] GetBlock(long blockOffset)
    {
        blockOffset.MustBeGreaterThanOrEqualTo(0);
        blockOffset.MustBeLessThan(PieceLength);

        byte[] data;

        data = new byte[GetBlockLength(blockOffset)];

        Buffer.BlockCopy(PieceData, (int)blockOffset, data, 0, data.Length);

        return data;
    }

    /// <summary>
    /// Gets the index of the block.
    /// </summary>
    /// <param name="blockOffset">The block offset.</param>
    /// <returns>The block index.</returns>
    public int GetBlockIndex(long blockOffset)
    {
        blockOffset.MustBeGreaterThanOrEqualTo(0);
        blockOffset.MustBeLessThanOrEqualTo(PieceLength);
        (blockOffset % BlockLength).MustBeEqualTo(0);

        return (int)(blockOffset / BlockLength);
    }

    /// <summary>
    /// Gets the length of the block.
    /// </summary>
    /// <param name="blockOffset">The block offset.</param>
    /// <returns>The block length.</returns>
    public long GetBlockLength(long blockOffset)
    {
        return Math.Min(BlockLength, PieceLength - blockOffset);
    }

    /// <summary>
    /// Gets the block offset.
    /// </summary>
    /// <param name="blockIndex">Index of the block.</param>
    /// <returns>The block offset.</returns>
    public long GetBlockOffset(int blockIndex)
    {
        blockIndex.MustBeGreaterThanOrEqualTo(0);
        blockIndex.MustBeLessThanOrEqualTo((int)(PieceLength / BlockLength));

        return BlockLength * blockIndex;
    }

    /// <summary>
    /// Puts the block.
    /// </summary>
    /// <param name="blockOffset">Index of the block.</param>
    /// <param name="blockData">The block data.</param>
    public void PutBlock(int blockOffset, byte[] blockData = null)
    {
        blockOffset.MustBeGreaterThanOrEqualTo(0);
        ((long)blockOffset).MustBeLessThan(PieceLength);
        (blockOffset % BlockLength).MustBeEqualTo(0);
        blockData.IsNotNull().Then(() => blockData.CannotBeNullOrEmpty());
        blockData.IsNotNull().Then(() => blockData.Length.MustBeEqualTo((int)GetBlockLength(blockOffset)));

        int blockIndex = GetBlockIndex(blockOffset);

        if (!BitField[blockIndex])
        {
            BitField[blockIndex] = true;

            if (blockData != null)
            {
                Buffer.BlockCopy(blockData, 0, PieceData, blockOffset, blockData.Length);
            }

            completedBlockCount++;

            if (completedBlockCount == BlockCount)
            {
                if (string.Compare(PieceData.CalculateSha1Hash(0, (int)PieceLength).ToHexaDecimalString(), PieceHash, true, CultureInfo.InvariantCulture) == 0)
                {
                    IsCompleted = true;

                    OnCompleted(this, new PieceCompletedEventArgs(PieceIndex, PieceData));
                }
                else
                {
                    IsCorrupted = true;

                    OnCorrupted(this, new PieceCorruptedEventArgs(PieceIndex));
                }
            }
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Called when piece has completed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnCompleted(object sender, PieceCompletedEventArgs e)
    {
        sender.CannotBeNull();
        e.CannotBeNull();

        Completed?.Invoke(sender, e);
    }

    /// <summary>
    /// Called when piece has become corrupted.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnCorrupted(object sender, System.EventArgs e)
    {
        sender.CannotBeNull();
        e.CannotBeNull();

        Corrupted?.Invoke(sender, e);
    }

    #endregion Private Methods
}
