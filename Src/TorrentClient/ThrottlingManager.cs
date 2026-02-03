using System;
using System.Diagnostics;
using System.Threading;
using DefensiveProgrammingFramework;
using TorrentClient.Extensions;

namespace TorrentClient;

/// <summary>
/// The throttling manager.
/// </summary>
public sealed class ThrottlingManager
{
    #region Private Fields

    /// <summary>
    /// The minimum read time in milliseconds.
    /// </summary>
    private decimal minReadTime;

    /// <summary>
    /// The minimum write time in milliseconds.
    /// </summary>
    private decimal minWriteTime;

    /// <summary>
    /// The read bytes count.
    /// </summary>
    private long read = 0;

    /// <summary>
    /// The count of bytes in read delta.
    /// </summary>
    private decimal readDelta = 0;

    /// <summary>
    /// The reading thread locker.
    /// </summary>
    private readonly Lock readingLocker = new();

    /// <summary>
    /// The read limit in bytes per second.
    /// </summary>
    private long readLimit;

    /// <summary>
    /// The read stopwatch.
    /// </summary>
    private readonly Stopwatch readStopwatch = new();

    /// <summary>
    /// The count of bytes in write delta.
    /// </summary>
    private decimal writeDelta = 0;

    /// <summary>
    /// The write limit in bytes per second.
    /// </summary>
    private long writeLimit;

    /// <summary>
    /// The write stopwatch.
    /// </summary>
    private readonly Stopwatch writeStopwatch = new();

    /// <summary>
    /// The writing thread locker.
    /// </summary>
    private readonly Lock writingLocker = new();

    /// <summary>
    /// The written bytes count.
    /// </summary>
    private long written = 0;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottlingManager"/> class.
    /// </summary>
    public ThrottlingManager()
    {
        ReadSpeedLimit = int.MaxValue;
        WriteSpeedLimit = int.MaxValue;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets the read speed in bytes per second.
    /// </summary>
    /// <value>
    /// The read speed in bytes per second.
    /// </value>
    public decimal ReadSpeed
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets the read speed limit in bytes per second.
    /// </summary>
    /// <value>
    /// The read speed limit in bytes per seconds.
    /// </value>
    public long ReadSpeedLimit
    {
        get
        {
            return readLimit;
        }

        set
        {
            value.MustBeGreaterThan(0);

            readLimit = value;
            readDelta = value;

            minReadTime = CalculateMinExecutionTime(value);
        }
    }

    /// <summary>
    /// Gets the write speed in bytes per second.
    /// </summary>
    /// <value>
    /// The write speed in bytes per second.
    /// </value>
    public decimal WriteSpeed
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets the write speed limit in bytes per second.
    /// </summary>
    /// <value>
    /// The write speed limit in bytes per seconds.
    /// </value>
    public long WriteSpeedLimit
    {
        get
        {
            return writeLimit;
        }

        set
        {
            value.MustBeGreaterThan(0);

            writeLimit = value;
            writeDelta = value;

            minWriteTime = CalculateMinExecutionTime(value);
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Writes the specified data.
    /// </summary>
    /// <param name="bytesRead">The bytes read count.</param>
    public void Read(long bytesRead)
    {
        bytesRead.MustBeGreaterThanOrEqualTo(0);

        decimal wait;

        lock (readingLocker)
        {
            if (!readStopwatch.IsRunning)
            {
                readStopwatch.Start();
            }

            read += bytesRead;

            if (read > readDelta)
            {
                readStopwatch.Stop();

                ReadSpeed = read / (decimal)readStopwatch.Elapsed.TotalSeconds;

                wait = (read / readDelta) * minReadTime;
                wait -= readStopwatch.ElapsedMilliseconds;

                if (wait > 0)
                {
                    Thread.Sleep((int)Math.Round(wait));
                }

                read = 0;
                readStopwatch.Restart();
            }
        }
    }

    /// <summary>
    /// Writes the specified data.
    /// </summary>
    /// <param name="bytesWritten">The bytes written.</param>
    public void Write(long bytesWritten)
    {
        bytesWritten.MustBeGreaterThanOrEqualTo(0);

        decimal wait;

        lock (writingLocker)
        {
            if (!writeStopwatch.IsRunning)
            {
                writeStopwatch.Start();
            }

            written += bytesWritten;

            if (written > writeDelta)
            {
                writeStopwatch.Stop();

                WriteSpeed = written / (decimal)writeStopwatch.Elapsed.TotalSeconds;

                wait = (written / writeDelta) * minWriteTime;
                wait -= writeStopwatch.ElapsedMilliseconds;

                if (wait > 0)
                {
                    Thread.Sleep((int)Math.Round(wait));
                }

                written = 0;
                writeStopwatch.Restart();
            }
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Calculates the minimum execution time.
    /// </summary>
    /// <param name="speed">The speed in bytes per second.</param>
    /// <returns>The minimal time to process the bytes in milliseconds.</returns>
    private decimal CalculateMinExecutionTime(decimal speed)
    {
        return 1000m * readDelta / speed;
    }

    #endregion Private Methods
}
