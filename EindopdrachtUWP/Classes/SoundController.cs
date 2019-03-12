using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media.Audio;
using Windows.Media.Render;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EindopdrachtUWP.Classes
{
    class SoundController
    {
        //private Dictionary<string, Sound> sounds;
        private Dictionary<string, AudioFileInputNode> sounds;

        public AudioGraph graph;
        private AudioFileInputNode backgroundMusicFileInput;
        private AudioSubmixNode submixNode;
        private AudioDeviceOutputNode deviceOutput;
        private Windows.Storage.StorageFile file;
        private Windows.Storage.StorageFolder folder;

        private bool mutedSFX = false;

        private bool initialized = false;

        private List<string> waitTillInitialized;


        public SoundController()
        {
            sounds = new Dictionary<string, AudioFileInputNode>();
            waitTillInitialized = new List<string>();
            mutedSFX = false;
            InitializeSoundSystem();
        }

        private async void InitializeSoundSystem()
        {
            await CreateAudioGraph();

            // If another file is already loaded into the FileInput node
            if (backgroundMusicFileInput != null)
            {
                // Release the file and dispose the contents of the node
                backgroundMusicFileInput.Dispose();
            }

            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            file = await folder.GetFileAsync("Sounds\\" + "Soundtrack\\Soundtrack.wav");

            CreateAudioFileInputNodeResult fileInputNodeResult = await graph.CreateFileInputNodeAsync(file);
            if (fileInputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // Cannot read file
                Debug.WriteLine("Cannot read input file because {0}", fileInputNodeResult.Status.ToString());
                return;
            }
                
            backgroundMusicFileInput = fileInputNodeResult.FileInputNode;
            backgroundMusicFileInput.OutgoingGain = 0.3;
            backgroundMusicFileInput.AddOutgoingConnection(deviceOutput);

            graph.Start();
            initialized = true;

            foreach(string filename in waitTillInitialized)
            {
                AddSound(filename);
            }
        }

        public void AddSound(string sound, double volume = 0.8)
        {
            if (sound == null || sound == "") return;
            if (sounds.ContainsKey(sound)) return;
            if (!initialized)
            {
                waitTillInitialized.Add(sound);
            }
            else
            {
                LoadSound(sound);
            }
        }

        public void PlaySound(string sound)
        {
            if (sounds == null || sounds[sound] == null || mutedSFX)
            {
                return;
            }
            
            if (sounds[sound].OutgoingConnections.Count == 1)
            {
                sounds[sound].Reset();
            }
            else
            {
                sounds[sound].AddOutgoingConnection(submixNode);
            }
        }
        
        private async void LoadSound(string filename)
        {
            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            file = await folder.GetFileAsync("Sounds\\" + filename);

            //Debug.WriteLine(graph == null);
            if (graph == null) {
                InitializeSoundSystem();
            }
            CreateAudioFileInputNodeResult fileInputNodeResult = await graph.CreateFileInputNodeAsync(file);
            if (fileInputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // Cannot read file
                Debug.WriteLine("Cannot read input file because {0}", fileInputNodeResult.Status.ToString());
                return;
            }
            
            sounds.Add(filename, fileInputNodeResult.FileInputNode);
            sounds[filename].OutgoingGain = 1;
        }

        public void MuteSFX()
        {
            mutedSFX = true;
        }
        public void UnMuteSFX()
        {
            mutedSFX = false;
        }

        public void MuteMusic()
        {
            if (backgroundMusicFileInput.OutgoingConnections.Count == 1)
            {
                backgroundMusicFileInput.RemoveOutgoingConnection(deviceOutput);
            }
        }

        public void UnMuteMusic()
        {
            if (backgroundMusicFileInput.OutgoingConnections.Count == 0)
            {
                backgroundMusicFileInput.AddOutgoingConnection(deviceOutput);
                backgroundMusicFileInput.Reset();
            }
        }

        private async Task CreateAudioGraph()
        {
            // Create an AudioGraph with default settings
            AudioGraphSettings settings = new AudioGraphSettings(AudioRenderCategory.Media);
            CreateAudioGraphResult result = await AudioGraph.CreateAsync(settings);

            if (result.Status != AudioGraphCreationStatus.Success)
            {
                // Cannot create graph
                Debug.WriteLine("AudioGraph Creation Error because {0}", result.Status.ToString());
                return;
            }
            
            graph = result.Graph;

            // Create a device output node
            CreateAudioDeviceOutputNodeResult deviceOutputNodeResult = await graph.CreateDeviceOutputNodeAsync();

            if (deviceOutputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device output node
                Debug.WriteLine("Device Output unavailable because {0}", deviceOutputNodeResult.Status.ToString());
                return;
            }

            deviceOutput = deviceOutputNodeResult.DeviceOutputNode;

            submixNode = graph.CreateSubmixNode();
            submixNode.AddOutgoingConnection(deviceOutput);
        }
    }
}
