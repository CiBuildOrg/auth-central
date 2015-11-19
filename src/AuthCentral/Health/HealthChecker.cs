using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentScheduler;
using FluentScheduler.Model;

using BrockAllen.MembershipReboot.Hierarchical;

using Fsw.Enterprise.AuthCentral.MongoDb;
using Microsoft.Framework.Logging;

namespace Fsw.Enterprise.AuthCentral.Health
{
    internal static class HealthChecker
    {
        public static void ScheduleHealthCheck(EnvConfig config, ILoggerFactory logFactory)
        {
            var logger = logFactory.CreateLogger(typeof(HealthChecker).ToString());
            var r = new Registry();
            r.Schedule(() =>
            {
                CheckHealth(config, logger);
            }).ToRunNow().AndEvery(30).Seconds();

            TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
            TaskManager.Initialize(r);
        }

        private static void CheckHealth(EnvConfig config, ILogger logger)
        {
            // TODO: trace log correctly
            logger.LogInformation("Checking Health... ");

            HealthContext.CurrentStatus = CheckUserDatabaseStatus(config);

            logger.LogInformation(HealthContext.CurrentStatus + Environment.NewLine);
        }

        static void TaskManager_UnobservedTaskException(TaskExceptionInformation info, UnhandledExceptionEventArgs e)
        {
            //TODO: log correctly!!!!!!!!!
            Console.WriteLine("An error happened with a scheduled task: " + e.ExceptionObject);
        }


        private static string CheckUserDatabaseStatus(EnvConfig config)
        {
            try
            {
                var userStore = new MongoDatabase(config.DB.MembershipReboot);
                var repo = new MongoUserAccountRepository<HierarchicalUserAccount>(userStore);

                var test = repo.GetAllTenants();

                if (!test.Any())
                    return HealthContext.Warning;

                return HealthContext.Good;
            }
            catch
            {
                return HealthContext.Bad;
            }
        }

    }
}
