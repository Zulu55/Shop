namespace Shop.UICross.Android.Services
{
    using System.IO;
    using global::Android.Content;
    using MvvmCross;
    using MvvmCross.Platforms.Android;

    public class MvxAndroidFileStore : MvxBaseFileStoreService
    {
        private Context _context;
        private Context Context
        {
            get
            {
                if (_context == null)
                {
                    _context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;
                }
                return _context;
            }
        }

        protected override string FullPath(string path)
        {
            return Path.Combine(Context.FilesDir.Path, path);
        }
    }
}