﻿using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Utilitarios.Middleware
{
    public class JwtMiddleware : IFunctionsWorkerMiddleware
    {
        //captura las solicitudes
        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
