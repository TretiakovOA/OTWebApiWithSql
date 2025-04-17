using System.Text.Json.Serialization;

namespace OTWebApiWithSql.Models
{
    public class CallAssignment
    {
        public int CallId { get; set; }
        [JsonIgnore]
        public Call? Call { get; set; }

        public int OperatorId { get; set; }
        [JsonIgnore]
        public Operator? Operator { get; set; }

        public DateTime AssignmentDate { get; set; }
    }
}
