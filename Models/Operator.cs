using System.Text.Json.Serialization;

namespace OTWebApiWithSql.Models
{
    public class Operator
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [JsonIgnore]
        public List<CallAssignment> CallAssignments { get; set; } = [];
    }
}
