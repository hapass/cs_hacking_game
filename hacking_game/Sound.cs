﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace hacking_game
{
    public static class Sound
    {
        public static AudioContext context;
        public static int buffer;
        public static int source;

        // Loads a wave/riff audio file.
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public static void Load()
        {
            context = new AudioContext();
            buffer = AL.GenBuffer();
            source = AL.GenSource();

            byte[] sound_data = LoadWave(Assembly.GetExecutingAssembly().GetManifestResourceStream("hacking_game.assets.print_sound.wav"), out int channels, out int bits_per_sample, out int sample_rate);
            AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
            AL.Source(source, ALSourcei.Buffer, buffer);
        }

        public static void Play()
        {
            AL.SourcePlay(source);
            Thread.Sleep(20);
            AL.SourceStop(source);
        }

        public static void Dispose()
        {
            AL.DeleteSource(source);
            AL.DeleteBuffer(buffer);
            context.Dispose();
        }
    }
}
