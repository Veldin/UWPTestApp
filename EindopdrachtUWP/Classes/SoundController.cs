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
        private Dictionary<String, AudioFileInputNode> sounds;

        public AudioGraph graph;
        private AudioFileInputNode backgroundMusicFileInput;
        private AudioSubmixNode submixNode;
        private AudioDeviceOutputNode deviceOutput;
        
        private bool mutedSFX = false;
        private bool mutedMusic = false;

        private bool initialized = false;

        private List<String> waitTillInitialized;


        public SoundController()
        {
            sounds = new Dictionary<string, AudioFileInputNode>();
            waitTillInitialized = new List<string>();
            mutedSFX = false;
            mutedMusic = false;
            this.initializeSoundSystem();
        }

        private async void initializeSoundSystem()
        {
            await CreateAudioGraph();

            // If another file is already loaded into the FileInput node
            if (backgroundMusicFileInput != null)
            {
                // Release the file and dispose the contents of the node
                backgroundMusicFileInput.Dispose();
            }

            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Sounds\\" + "Soundtrack\\Soundtrack.wav");

            CreateAudioFileInputNodeResult fileInputNodeResult = await graph.CreateFileInputNodeAsync(file);
            if (fileInputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // Cannot read file
                Debug.WriteLine(String.Format("Cannot read input file because {0}", fileInputNodeResult.Status.ToString()));
                return;
            }
                
            backgroundMusicFileInput = fileInputNodeResult.FileInputNode;
            backgroundMusicFileInput.OutgoingGain = 0.3;
            backgroundMusicFileInput.AddOutgoingConnection(deviceOutput);


            graph.Start();
            initialized = true;

            foreach(String filename in waitTillInitialized)
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
            if(sounds == null || sounds[sound] == null || mutedSFX)
            {
                return;
            }
            
            if(sounds[sound].OutgoingConnections.Count == 1)
            {
                sounds[sound].Reset();
            }
            else
            {
                sounds[sound].AddOutgoingConnection(submixNode);
            }
            
        }
        
        private async void LoadSound(String filename)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Debug.WriteLine("test1");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Sounds\\" + filename);

            Debug.WriteLine("test2");
            //Debug.WriteLine(graph == null);
            if (graph == null) {
                initializeSoundSystem();
            }
            CreateAudioFileInputNodeResult fileInputNodeResult = await graph.CreateFileInputNodeAsync(file);
            Debug.WriteLine("test3");
            if (fileInputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // Cannot read file
                Debug.WriteLine(String.Format("Cannot read input file because {0}", fileInputNodeResult.Status.ToString()));
                return;
            }

            Debug.WriteLine("test4");
            sounds.Add(filename, fileInputNodeResult.FileInputNode);
            Debug.WriteLine("test5");
            sounds[filename].OutgoingGain = 1;
            Debug.WriteLine("test6");

            Debug.WriteLine(String.Format("Added sound {0}", filename));
        }
        

        public void muteSFX()
        {
            mutedSFX = true;
        }
        public void unMuteSFX()
        {
            mutedSFX = false;
        }

        public void muteMusic()
        {
            if (backgroundMusicFileInput.OutgoingConnections.Count == 1)
            {
                backgroundMusicFileInput.RemoveOutgoingConnection(deviceOutput);
            }
        }

        public void unMuteMusic()
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
                Debug.WriteLine(String.Format("AudioGraph Creation Error because {0}", result.Status.ToString()));
                return;
            }
            
            graph = result.Graph;

            // Create a device output node
            CreateAudioDeviceOutputNodeResult deviceOutputNodeResult = await graph.CreateDeviceOutputNodeAsync();

            if (deviceOutputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device output node
                Debug.WriteLine(String.Format("Device Output unavailable because {0}", deviceOutputNodeResult.Status.ToString()));
                return;
            }

            deviceOutput = deviceOutputNodeResult.DeviceOutputNode;

            submixNode = graph.CreateSubmixNode();
            submixNode.AddOutgoingConnection(deviceOutput);

            Debug.WriteLine("Device Output Node successfully created");

        }

    }
}
