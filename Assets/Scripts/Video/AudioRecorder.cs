﻿using System.Collections;
using UnityEngine;
using System.IO; // for FileStream
using System; // for BitConverter and Byte Type
using Experiment;
using Constants;

namespace Assets.Scripts.Video
{
    public class AudioRecorder : MonoBehaviour
    {
        private int bufferSize;
        private int numBuffers;
        private int outputRate = 44100;
        // private string fileName = "recTest.wav";
        private int headerSize = 44; //default for uncompressed wav

        private bool recOutput;

        private FileStream fileStream;
        private IExperiment experiment;

        private void Awake()
        {
            AudioSettings.outputSampleRate = outputRate;
            experiment = GameObject.Find(GameObjectNames.Experiment).GetComponent<IExperiment>();
        }

        private void Start()
        {
            AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
        }

        private void Update()
        {
           /* if (Input.GetKeyDown("r"))
            {
                print("rec");
                if (recOutput == false)
                {
                    StartWriting(Path.Combine(experiment.OutputLocation, "Participant Audio"));
                    recOutput = true;
                }
                else
                {
                    recOutput = false;
                    WriteHeader();
                    print("rec stop");
                }
            }*/
        }

        public void StartRecording()
        {
            if (recOutput == false)
            {
                // TODO centralize
                if (!Directory.Exists(experiment.OutputLocation))
                    Directory.CreateDirectory(experiment.OutputLocation);

                StartWriting(Path.Combine(experiment.OutputLocation, "Participant Audio.wav"));
                recOutput = true;
            }
        }

        public void StopRecording()
        {
            if (recOutput == true)
            {
                recOutput = false;
                WriteHeader();
                print("rec stop");
            }
        }

        private void StartWriting(string name)
        {
            fileStream = new FileStream(name, FileMode.Create);
            var emptyByte = new byte();

            for (int i = 0; i < headerSize; i++) //preparing the header
            {
                fileStream.WriteByte(emptyByte);
            }
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (recOutput)
            {
                ConvertAndWrite(data); //audio data is interlaced
            }
        }

        private void ConvertAndWrite(float[] dataSource)
        {

            Int16[] intData = new Int16[dataSource.Length];
            //converting in 2 steps : float[] to Int16[], //then Int16[] to Byte[]

            Byte[] bytesData = new Byte[dataSource.Length * 2];
            //bytesData array is twice the size of
            //dataSource array because a float converted in Int16 is 2 bytes.

            var rescaleFactor = 32767; //to convert float to Int16

            for (var i = 0; i < dataSource.Length; i++)
            {
                intData[i] = (short)(dataSource[i] * rescaleFactor);
                var byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fileStream.Write(bytesData, 0, bytesData.Length);
        }

        private void WriteHeader()
        {
            fileStream.Seek(0, SeekOrigin.Begin);

            var riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            var chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
            fileStream.Write(chunkSize, 0, 4);

            var wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            var fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            var subChunk1 = BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            UInt16 two = 2;
            UInt16 one = 1;

            var audioFormat = BitConverter.GetBytes(one);
            fileStream.Write(audioFormat, 0, 2);

            var numChannels = BitConverter.GetBytes(two);
            fileStream.Write(numChannels, 0, 2);

            var sampleRate = BitConverter.GetBytes(outputRate);
            fileStream.Write(sampleRate, 0, 4);

            var byteRate = BitConverter.GetBytes(outputRate * 4);
            // sampleRate * bytesPerSample*number of channels, here 44100*2*2

            fileStream.Write(byteRate, 0, 4);

            UInt16 four = 4;
            var blockAlign = BitConverter.GetBytes(four);
            fileStream.Write(blockAlign, 0, 2);

            UInt16 sixteen = 16;
            var bitsPerSample = BitConverter.GetBytes(sixteen);
            fileStream.Write(bitsPerSample, 0, 2);

            var dataString = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(dataString, 0, 4);

            var subChunk2 = BitConverter.GetBytes(fileStream.Length - headerSize);
            fileStream.Write(subChunk2, 0, 4);

            fileStream.Close();
        }
    }
}