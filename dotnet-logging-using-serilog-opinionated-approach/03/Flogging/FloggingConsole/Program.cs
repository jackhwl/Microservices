using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloggingConsole
{
	class Program
	{
		static void Main(string[] args)
		{
		}
        private static FlogDetail GetFlogDetail(string message, Exception ex)
        {
            const string Product = "Flogger";
            const string Location = "FloggerConsole"; // this application
            const string Layer = "Job"; // unattended executable invoked somehow
            var user = Environment.UserName;
            var hostname = Environment.MachineName;

            return new FlogDetail
            {
                Product = Product,
                Location = Location,
                Layer = Layer,
                UserName = user,
                Hostname = hostname,
                Message = message,
                Exception = ex
            };
        }
	}
}
