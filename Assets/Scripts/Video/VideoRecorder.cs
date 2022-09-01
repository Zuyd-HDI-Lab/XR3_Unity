using System.IO;
using Constants;
using Experiment;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.Audio;

namespace Video
{
    public static class VideoRecorder
    {
        static RecorderController TestRecorderController;
        public static bool recording;        

        public static void Record(/*AudioListener audioListener*/)
        {
            recording = true;
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            var experiment = GameObject.Find(GameObjectNames.Experiment).GetComponent<IExperiment>();

            TestRecorderController = new RecorderController(controllerSettings);

            var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
            videoRecorder.name = "Session Recorder";
            videoRecorder.Enabled = true;
            videoRecorder.VideoBitRateMode = VideoBitrateMode.High;
            videoRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
            videoRecorder.OutputFile = Path.Combine(experiment.OutputLocation,"Participant View");


            videoRecorder.ImageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 1920,
                OutputHeight = 1080
            };

            videoRecorder.AudioInputSettings.PreserveAudio = false;
            //videoRecorder.OutputFile; // Change this to change the output file name (no extension)

            controllerSettings.AddRecorderSettings(videoRecorder);
            //controllerSettings.SetRecordModeToFrameInterval(0, 59); // 2s @ 30 FPS
            controllerSettings.SetRecordModeToManual();
            controllerSettings.FrameRate = 90;

            RecorderOptions.VerboseMode = false;
            TestRecorderController.PrepareRecording();
            TestRecorderController.StartRecording();

            //RecordAudio(experiment.OutputLocation, null);
        }

/*        private static void RecordAudio(string outputPath, AudioListener listener)
        {
            AudioClip AudioClip = AudioClip.Create("participant audio", 1, 2, 44000, false);

            AudioListener.GetOutputData()

            AudioClip.SetData()
        }*/

        /*        private static void RecordAudio(string outputPath)
                {
                    Directory.CreateDirectory(outputPath);
                    var outputFilePath = Path.Combine(outputPath, "Participant Audio.wav");
                    var mmdevice = new MMde
                    capture = new WasapiLoopbackCapture();
                    // optionally we can set the capture waveformat here: e.g. capture.WaveFormat = new WaveFormat(44100, 16,2);
                    var writer = new WaveFileWriter(outputFilePath, capture.WaveFormat);

                    capture.DataAvailable += (s, a) =>
                    {
                        writer.Write(a.Buffer, 0, a.BytesRecorded);
                        if (writer.Position > capture.WaveFormat.AverageBytesPerSecond * 20)
                        {
                            capture.StopRecording();
                        }
                    };

                    capture.RecordingStopped += (s, a) =>
                    {
                        writer.Dispose();
                        writer = null;
                        capture.Dispose();
                    };

                    capture.StartRecording();
                }*/

        public static void Stop()
        {
            recording = false;
            TestRecorderController.StopRecording();
        }
    }
}
