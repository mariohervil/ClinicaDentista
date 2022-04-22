using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ClinicaDental
{
    public class Email
    {
        public string to { get; set; }
        public string cc { get; set; }
        public string cco { get; set; }
        public string subject { get; set; }
        public string body { get; set; }


        public Email(string to, string cc, string cco, string subject, string body)
        {
            this.to = to;
            this.cc = cc;
            this.cco = cco;
            this.subject = subject;
            this.body = body;
        }

    }
}
