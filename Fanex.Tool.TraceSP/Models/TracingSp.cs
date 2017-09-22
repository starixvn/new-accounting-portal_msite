using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Tool.TraceSP.Models
{
    public class TracingSp
    {
        public string Site { get; set; }

        public int TraceTimer { get; set; }
        
        public string TextDataLike { get; set; }
        
        public string TextDataLike2 { get; set; }
        
        public string TextDataLike3 { get; set; }

        public bool TraceStartEvents { get; set; }

        public bool Mode { get; set; }

        public string Server { get; set; }

        public List<Site> Sites { get; set; }

        public List<Server> Servers { get; set; }

        public bool HasError { get; set; }

        public string ErrorMsg { get; set; }
    }
}
