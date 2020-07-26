using Google.Protobuf;

namespace Repository
{
    public class LocationModel : Model
    {
        
        public string Id { get; set; }
        
        public string AddressId { get; set; }
        
        public string DeviceId { get; set; }
        
        public string Name { get; set; }
        
    }

    public class LocationPrimaryKey : PrimaryKey
    {
        public string ToWhereCondition()
        {
            return $"Id = {Id}";
        }

        public string Id { get; }
    }
}