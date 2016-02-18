using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentScheduler;
using FluentScheduler.Model;

using BrockAllen.MembershipReboot.Hierarchical;

using Microsoft.Extensions.Logging;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.MongoStore;

namespace Fsw.Enterprise.AuthCentral.Health
{
    internal static class HealthChecker
    {
        public static void ScheduleHealthCheck(EnvConfig config, ILoggerFactory logFactory, IClientService clientService, UserAccountService<HierarchicalUserAccount> userAccountService)
        {
            ILogger logger = logFactory.CreateLogger(typeof(HealthChecker).ToString());
            var r = new Registry();
            r.Schedule(() =>
            {
                CheckHealth(config, logger, clientService, userAccountService);
            }).ToRunNow().AndEvery(30).Seconds();

            TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
            TaskManager.Initialize(r);
        }

        private static async Task CheckHealth(EnvConfig config, ILogger logger, IClientService clientService, UserAccountService<HierarchicalUserAccount> userAccountService)
        {
            // TODO: trace log correctly
            logger.LogInformation("Checking Health... ");

            HealthContext.IdmDbStatus = CheckUserDatabaseStatus(config, userAccountService, logger);
            HealthContext.IdsDbStatus = await CheckIdServerDatabaseStatus(config, clientService, logger);

            logger.LogInformation(HealthContext.CurrentStatus + Environment.NewLine);
        }

        static void TaskManager_UnobservedTaskException(TaskExceptionInformation info, UnhandledExceptionEventArgs e)
        {
            //TODO: log correctly!!!!!!!!!
            Console.WriteLine("An error happened with a scheduled task: " + e.ExceptionObject);
        }

        private static async Task<string> CheckIdServerDatabaseStatus(EnvConfig config, IClientService clientService, ILogger logger)
        {
            try
            {
                ClientPagingResult result = await clientService.GetPageAsync(1, 10);
                return HealthContext.Good;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return HealthContext.Bad;
            }
        }

        private static string CheckUserDatabaseStatus(EnvConfig config, UserAccountService<HierarchicalUserAccount> userAccountService, ILogger logger)
        {
            try
            {
                bool result = userAccountService.UsernameExists("RobertDabolinaJuniorSenior");
                return HealthContext.Good;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return HealthContext.Bad;
            }
        }

    }
}
