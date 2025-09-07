using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIApp.Services
{
    public class EmailMessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from EmailMessageService via Dependency Injection!";
        }
    }
}