namespace Shop.UICross.Android.Services
{
    using System;
    using System.IO;
    using Common.Interfaces;
    using MvvmCross;

    public class PictureService : IPictureService,
        IMvxServiceConsumer<IMvxPictureChooserTask>,
        IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        private const int MaxPixelDimension = 1024;
        private const int DefaultJpegQuality = 92;

        public void TakeNewPhoto(Action<byte[]> onSuccess, Action<string> onError)
        {
            Mvx.Resolve<IMvxPictureChooserTask>().TakePicture(
            PictureService.MaxPixelDimension,
            PictureService.DefaultJpegQuality,
            pictureStream =>
            {
                var memoryStream = new MemoryStream();
                pictureStream.CopyTo(memoryStream);
                onSuccess(memoryStream.GetBuffer());
            },
            () => { /* cancel is ignored */ });
        }

        public void SelectExistingPicture(Action<byte[]> onSuccess, Action<string> onError)
        {
            Mvx.Resolve<IMvxPictureChooserTask>().ChoosePictureFromLibrary(
            PictureService.MaxPixelDimension,
            PictureService.DefaultJpegQuality,
            pictureStream =>
            {
                var memoryStream = new MemoryStream();
                pictureStream.CopyTo(memoryStream);
                onSuccess(memoryStream.GetBuffer());
            },
            () => { /* cancel is ignored */ });
        }

        private string Save(Stream stream)
        {
            string fileName = null;
            try
            {
                fileName = Guid.NewGuid().ToString("N");
                var fileService = Mvx.Resolve<IMvxSimpleFileStoreService>();
                fileService.WriteFile(fileName, stream.CopyTo);
            }
            catch (Exception)
            {
                fileName = null;
            }
            return fileName;
        }
    }
}