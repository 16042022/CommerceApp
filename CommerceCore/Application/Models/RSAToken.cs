﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Models
{
    public class RSAToken
    {
        public string PrivateKey { get; set; } = "";
        public string PublicKey { get; set; } = "";
    }
}
