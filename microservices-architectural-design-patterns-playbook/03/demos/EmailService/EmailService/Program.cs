using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Autofac;

namespace EmailService
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(DIContainer.Setup());

                x.Service<EmailServiceRunner>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(esr => esr.Start());
                    s.WhenStopped(esr => esr.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("Email Service");
                x.SetDisplayName("Email Service");
                x.SetServiceName(String.Format("EmailService-Instance-{0}",
                    Config.InstanceNumber));
                x.OnException(ex => Logger.LogError(ex));
            });


            Console.ReadLine();
        }
    }
}
