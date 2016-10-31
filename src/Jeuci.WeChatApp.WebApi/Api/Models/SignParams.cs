﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Api.Models
{
    public class SignParams
    {
        public string Signature { get; set; }

        public long Timestamp { get; set; }

        public int Nonce { get; set; }

        public string Echostr { get; set; }
    }
}
