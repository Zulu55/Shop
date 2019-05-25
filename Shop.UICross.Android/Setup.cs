namespace Shop.UICross.Android
{
    using Common;
    using Common.Interfaces;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Core;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.LazyConstructAndRegisterSingleton<INetworkProvider, NetworkProvider>();
            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.IoCProvider.RegisterType<IPictureService, PictureService>();
            Mvx.IoCProvider.RegisterType<IMvxPictureChooserTask, MvxPictureChooserTask>();
            Mvx.IoCProvider.RegisterType<IMvxSimpleFileStoreService, MvxAndroidFileStore>();

            base.InitializeFirstChance();
        }

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var assemblies = base.GetPluginAssemblies().ToList();
            assemblies.Add(typeof(MvvmCross.Plugin.Visibility.Platforms.Android.Plugin).Assembly);
            return assemblies;
        }
    }
}