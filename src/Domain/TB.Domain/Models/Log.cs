using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Domain.Models
{
    public class Log
    {
        public Log()
        {
                
        }

        [Key]
        public int Id { get; set; }
        public string TimeStamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Time { get; set; }
        public string RequestId { get; set; }
        public string RequestPath{ get; set; }
        public string ConnectionId { get; set; }
        public string CorrelationId { get; set; }
        public string MachineName { get; set; }
        public string IPAddress { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


    }
}
