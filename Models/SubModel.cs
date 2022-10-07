using System.Runtime.Serialization;
using Catel.Data;

namespace CatelSpoiledDerialization.Models
{
    public class SubModel : ModelBase
    {
        [DataMember]
        public int SubId { get; set; }

        [DataMember]
        public string SubName { get; set; }
    }
}