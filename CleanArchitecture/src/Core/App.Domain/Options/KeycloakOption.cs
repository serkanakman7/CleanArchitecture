using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Options
{
    public class KeycloakOption
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }
}
