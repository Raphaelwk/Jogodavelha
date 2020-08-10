using JogoDaVelha.Application.AppService;
using JogoDaVelha.Application.IAppService;
using SimpleInjector;

namespace JogoDaVelha.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static Container MyContainer { get; set; }

        public static void Register(Container container)
        {
            MyContainer = container;

            container.Register<IGameAppService, GameAppService>(Lifestyle.Scoped);
        }

        public static void RegisterServicesStatic(Container container)
        {
            MyContainer = container;

            container.Register<IGameAppService, GameAppService>();
        }
    }
}
