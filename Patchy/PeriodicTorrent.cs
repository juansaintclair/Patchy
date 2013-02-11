﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTorrent.Common;

namespace Patchy
{
    /// <summary>
    /// Periodically checks various properties and fires PropertyChanged
    /// as needed.
    /// </summary>
    public class PeriodicTorrent : INotifyPropertyChanged // TODO: Consider replacing TorrentWrapper entirely with this
    {
        public TorrentWrapper Torrent { get; set; }

        public PeriodicTorrent(TorrentWrapper wrapper)
        {
            Torrent = wrapper;
            Update();
            Name = Torrent.Name;
            Size = Torrent.Size;
        }

        internal void Update()
        {
            State = Torrent.State;
            Progress = Torrent.Progress;
            DownloadSpeed = Torrent.Monitor.DownloadSpeed;
            UploadSpeed = Torrent.Monitor.UploadSpeed;
            if (Torrent.State == TorrentState.Metadata)
                EstimatedTime = TimeSpan.MaxValue;
            else
                EstimatedTime = new TimeSpan((long)((DateTime.Now - Torrent.StartTime).Ticks / (Torrent.Progress / 100)));
            TotalDownloaded = Torrent.Monitor.DataBytesDownloaded;
            TotalUploaded = Torrent.Monitor.DataBytesUploaded;
            DownloadToUploadRatio = (double)Torrent.Monitor.DataBytesUploaded / (double)Torrent.Monitor.DataBytesDownloaded;
            if (Torrent.State == TorrentState.Downloading && files == null)
            {
                files = new PeriodicFile[Torrent.Torrent.Files.Length];
                for (int i = 0; i < files.Length; i++)
                    files[i] = new PeriodicFile(Torrent.Torrent.Files[i]);
                PropertyChanged(this, new PropertyChangedEventArgs("Files"));
            }
            if (Torrent.IsMagnet && Torrent.State == TorrentState.Downloading && Torrent.Size == -1)
                Size = Torrent.Torrent.Files.Select(f => f.Length).Aggregate((a, b) => a + b);
            if (files != null)
            {
                foreach (var file in files)
                    file.Update();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private PeriodicFile[] files;
        public PeriodicFile[] Files
        {
            get
            {
                return files;
            }
        }

        private TorrentState state;
        public TorrentState State 
        {
            get
            {
                return state;
            }
            private set
            {
                var fire = state != value;
                state = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("State"));
            }
        }

        private double progress;
        public double Progress
        {
            get
            {
                return progress;
            }
            private set
            {
                var fire = progress != value;
                progress = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Progress"));
            }
        }

        private int downloadSpeed;
        public int DownloadSpeed
        {
            get
            {
                return downloadSpeed;
            }
            private set
            {
                var fire = downloadSpeed != value;
                downloadSpeed = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DownloadSpeed"));
            }
        }

        private int uploadSpeed;
        public int UploadSpeed
        {
            get
            {
                return uploadSpeed;
            }
            private set
            {
                var fire = uploadSpeed != value;
                uploadSpeed = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UploadSpeed"));
            }
        }

        private TimeSpan estimatedTime;
        public TimeSpan EstimatedTime
        {
            get
            {
                return estimatedTime;
            }
            private set
            {
                var fire = estimatedTime != value;
                estimatedTime = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("EstimatedTime"));
            }
        }

        private long totalDownloaded;
        public long TotalDownloaded
        {
            get
            {
                return totalDownloaded;
            }
            private set
            {
                var fire = totalDownloaded != value;
                totalDownloaded = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalDownloaded"));
            }
        }

        private long totalUploaded;
        public long TotalUploaded
        {
            get
            {
                return totalUploaded;
            }
            private set
            {
                var fire = totalUploaded != value;
                totalUploaded = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalUploaded"));
            }
        }

        private double downloadToUploadRatio;
        public double DownloadToUploadRatio
        {
            get
            {
                return downloadToUploadRatio;
            }
            private set
            {
                var fire = downloadToUploadRatio != value;
                downloadToUploadRatio = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DownloadToUploadRatio"));
            }
        }

        private bool complete;
        public bool Complete
        {
            get
            {
                return complete;
            }
            private set
            {
                var fire = complete != value;
                complete = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Complete"));
            }
        }

        private long size;
        public long Size
        {
            get
            {
                return size;
            }
            private set
            {
                var fire = size != value;
                size = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Size"));
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                var fire = name != value;
                name = value;
                if (fire && PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
    }
}
