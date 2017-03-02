using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class ServiceInfoOutput : ServiceInfoOutputBase
    {
        public string PrepayId { get; set; }

        public string OpenId { get; set; }
    }
}
