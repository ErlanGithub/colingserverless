﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingRepositorioAutenticacion.Contratos
{
    public interface ITokenData
    {
        public DateTime Expira {  get; set; }
        public string Token { get; set; }
    }
}
