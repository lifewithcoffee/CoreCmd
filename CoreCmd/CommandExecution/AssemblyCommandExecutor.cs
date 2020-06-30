using CoreCmd.CommandLoading;
using CoreCmd.Help;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreCmd.CommandExecution
{
    public interface IAssemblyCommandExecutor
    {
        void Execute(string[] args, Action<IServiceCollection> configService);
        Task ExecuteAsync(string[] args, Action<IServiceCollection> configService);
    }

    public class AssemblyCommandExecutor : IAssemblyCommandExecutor
    {
        IHelpInfoService _helpSvc = new HelpInfoService();
        public List<Assembly> additionalAssemblies = new List<Assembly>();  // mainly for unit test

        public void SetAdditionalSearchAssembly(Assembly assembly)
        {
            this.additionalAssemblies.Add(assembly);
        }

        public AssemblyCommandExecutor() { }

        public AssemblyCommandExecutor(string configFileName)
        {
            Global.ConfigFileName = configFileName;
        }

        public AssemblyCommandExecutor(params Type[] types)
        {
            if (types != null)
            {
                foreach(var type in types)
                    this.SetAdditionalSearchAssembly(Assembly.GetAssembly(type));
            }
        }

        private IServiceProvider ConfigureServices(IEnumerable<Type> types, Action<IServiceCollection> configureMoreServicesAction)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            foreach(var type in types)
            {
                serviceCollection.AddTransient(type);
            }
            configureMoreServicesAction?.Invoke(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        public async Task ExecuteAsync(string[] args, Action<IServiceCollection> configureMoreServicesAction = null )
        {
            ICommandClassLoader _loader = new CommandClassLoader();
            ICommandExecutorCreate _commandFinder = new CommandExecutorCreator();

            try
            {
                var allClassTypes = _loader.LoadAllCommandClasses(additionalAssemblies);
                IServiceProvider serviceProvider = ConfigureServices(allClassTypes, configureMoreServicesAction);

                if (args.Length > 0)
                {
                    var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, args);
                    if (singleCommandExecutor != null)
                        await singleCommandExecutor.ExecuteAsync(serviceProvider).ConfigureAwait(false);
                }
                else   // print all available commands
                {
                    Console.WriteLine("Subcommand is missing, please specify a subcommand:");
                    _helpSvc.PrintClassHelp(allClassTypes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Execute(string[] args, Action<IServiceCollection> services = null)
        {
            ExecuteAsync(args, services).Wait();
        }
    }
}
