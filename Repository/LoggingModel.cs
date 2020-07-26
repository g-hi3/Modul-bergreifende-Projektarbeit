using System;

namespace Repository
{
    public class LoggingModel : Model
    {
        public string Id { get; set; }
        public string Pod { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }
        public string Severity { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
    
    public class LoggingPrimaryKey : PrimaryKey
    {
        public string ToWhereCondition()
        {
            return $"Id = '{Id}'";
        }

        public string Id { get; }
    }
}