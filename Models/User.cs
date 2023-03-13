using System;

namespace Job_Service.Models
{
    public class User
    {
        public string Id { get; set; }
        public string title { get; set; }

        public string description { get; set; }

        public int locationId { get; set; }


        public int departmentId { get; set; }


        public DateTime closingDate { get; set; }
    }

}
