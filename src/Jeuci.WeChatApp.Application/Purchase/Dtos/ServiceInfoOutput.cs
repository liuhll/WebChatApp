using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class ServiceInfoOutput
    {
        public string OrderId { get; set; }

        public string ServiceName { get; set; }

        public string PrepayId { get; set; }

        public decimal OrderPrice { get; set; }

        public string Description { get; set; }

        public string OpenId { get; set; }

        public string Sid { get; set; }
    }
}
