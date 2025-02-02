﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using NAudio;
using NAudio.Wave;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace OneShot_ModLoader
{
    public static class Audio
    {
        private static List<AudioFile> activeAudio = new List<AudioFile>();

        public static void PlaySound(string sound, bool loop)
        {
            Console.WriteLine("attempting to play sound: " + sound);

            try
            { 
                activeAudio.Add(new AudioFile(new AudioFileReaderWrapped(Static.audioPath + sound), loop));
            }
            catch (Exception ex)
            {
                ExceptionMessage.New(ex, true);
            }
        }

        public static void Stop()
        {
            // dispose the fields of each file
            foreach (AudioFile a in activeAudio)
                a.DisposeStuff(new object(), new StoppedEventArgs());
            activeAudio.Clear();
        }

        private struct AudioFile
        {
            private AudioFileReaderWrapped a;
            private LoopStream loopStream;
            private WaveOutEvent waveOut;

            // initialize a structure called AudioFile that contains an AudioFileReader, LoopStream and WaveOutEvent
            // that can each be disposed when playback stops
            public AudioFile(AudioFileReaderWrapped a, bool loop)
            {
                this.a = a;
                loopStream = new LoopStream(this.a);
                waveOut = new WaveOutEvent();

                if (loop) 
                    waveOut.Init(loopStream);
                else
                {
                    waveOut.Init(a);
                    waveOut.PlaybackStopped += DisposeStuff;
                }

                waveOut.Play();
            }

            public void DisposeStuff(object sender, StoppedEventArgs e)
            {
                waveOut.Dispose();
                a.Dispose();
                loopStream.Dispose();
                waveOut = null;
                a = null;
                loopStream = null;
            }
        }

        private class AudioFileReaderWrapped : AudioFileReader, IDisposable
        {
            // just call base ctor
            public AudioFileReaderWrapped(string fileName)
                : base(fileName) { }

            public new void Dispose() => Dispose(true);
        }
    }
}