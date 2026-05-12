using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Session { get; set; } = 1;

        [JsonIgnore]
        public string Caption => $"[{Session}] {Code} {Title}";

    }
}