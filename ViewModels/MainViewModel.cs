using System.IO;
using System.Threading.Tasks;
using Catel;
using Catel.IoC;
using Catel.MVVM;
using Catel.Runtime.Serialization;
using Catel.Services;
using CatelSpoiledDerialization.Models;
using CatelSpoiledDerialization.Properties;
using Path = Catel.IO.Path;

namespace CatelSpoiledDerialization.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            MainData = new MainModel()
            {
                DataId = 123,
                DataName = "Main Data Name"
            };
            MainData.DataCollection.Clear();
            for (int i = 0; i < 5; i++)
            {
                MainData.DataCollection.Add(new SubModel(){SubId = 101 + i, SubName = $"SubName {i+1}"});
            }

            LoadFileCommand = new TaskCommand(OnLoadFileCommand);

            //    async () =>
            //{
            //    await OnLoadFileCommand().ConfigureAwait(false);
            //});
            SaveFileCommand = new TaskCommand(OnSaveFileCommand);
        }
        public TaskCommand LoadFileCommand
        {
            get;
        }

        public TaskCommand SaveFileCommand
        {
            get;
        }
        private async Task OnSaveFileCommand()
        {
            var saveFileService = ServiceLocator.Default.ResolveType<ISaveFileService>();
            var context = new DetermineSaveFileContext
            {
                Filter = "*.xml|*.xml",
                Title = $"Choose appropriate {nameof(MainModel)} XML file"
            };

            if (!string.IsNullOrEmpty(FileName))
            {
                context.InitialDirectory = Path.GetDirectoryName(FileName);
                context.FileName = FileName;
            }

            var res = await saveFileService.DetermineFileAsync(context).ConfigureAwait(false);

            if (res.Result)
            {
                // Normally, this should better be encapsulated in a DataModelSerializationService
                var iXmlSer = SerializationFactory.GetXmlSerializer();
                using var fileStream = File.Create(res.FileName);
                iXmlSer.Serialize(MainData, fileStream);
                RaisePropertyChanged(nameof(MainData));
            }
        }

        public string FileName { get; set; }

        private async Task OnLoadFileCommand()
        {
            var openFileService = ServiceLocator.Default.ResolveType<IOpenFileService>();
            var context = new DetermineOpenFileContext
            {
                Filter = "*.xml|*.xml",
                Title = $"Choose appropriate {nameof(MainModel)} XML file",
                IsMultiSelect = false,
                CheckFileExists = true,
            };

            if (!string.IsNullOrEmpty(FileName))
            {
                context.InitialDirectory = Path.GetDirectoryName(FileName);
                context.FileName = FileName;
            }

            var res = await openFileService.DetermineFileAsync(context).ConfigureAwait(false);
            if (res.Result)
            {
                // Normally, this should better be encapsulated in a DataModelSerializationService
                var iXmlSer = SerializationFactory.GetXmlSerializer();
                using var fileStream = File.Open(res.FileName, FileMode.Open);
                
                MainData = iXmlSer.Deserialize<MainModel>(fileStream);
                FileName = res.FileName;
                
                RaisePropertyChanged(nameof(MainData));
            }
        }

        public MainModel MainData { get; set; }

    }
}