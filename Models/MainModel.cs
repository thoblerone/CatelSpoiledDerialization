using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Catel.Data;

namespace CatelSpoiledDerialization.Models
{
    public class MainModel : ModelBase
    {
        public MainModel()
        {
            DataCollection = new ObservableCollection<SubModel>();
        }

        [DataMember] public int DataId { get; set; }

        [DataMember] public string DataName { get; set; }

        [DataMember]
        public ObservableCollection<SubModel> DataCollection { get; protected set; }


    }
}