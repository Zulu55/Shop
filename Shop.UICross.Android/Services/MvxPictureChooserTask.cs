namespace Shop.UICross.Android.Services
{
    using System;
    using System.IO;
    using Common.Interfaces;
    using global::Android.App;
    using global::Android.Content;
    using global::Android.Graphics;
    using global::Android.Provider;
    using MvvmCross;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Android;
    using MvvmCross.Platforms.Android.Views.Base;
    using Uri = global::Android.Net.Uri;

    public class MvxPictureChooserTask : MvxAndroidTask,
        IMvxPictureChooserTask,
        IMvxServiceConsumer<IMvxAndroidGlobals>,
        IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        private Uri _cachedUriLocation;
        private RequestParameters _currentRequestParameters;

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.GrantWriteUriPermission);
            intent.SetFlags(ActivityFlags.GrantPersistableUriPermission);
            _cachedUriLocation = GetNewImageUri();
            intent.PutExtra(MediaStore.ExtraOutput, _cachedUriLocation);
            intent.PutExtra("outputFormat", Bitmap.CompressFormat.Jpeg.ToString());
            intent.PutExtra("return-data", true);
            ChoosePictureCommon(MvxIntentRequestCode.PickFromCamera, intent, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        private Uri GetNewImageUri()
        {
            // Optional - specify some metadata for the picture
            var contentValues = new ContentValues();
            // Specify where to put the image
            return Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
        }

        public void ChoosePictureCommon(MvxIntentRequestCode pickId, Intent intent, int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            if (_currentRequestParameters != null)
            {
                throw new MvxException("Cannot request a second picture while the first request is still pending");
            }

            _currentRequestParameters = new RequestParameters(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
            StartActivityForResult((int)pickId, intent);
        }

        protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            Uri uri = null;
            switch ((MvxIntentRequestCode)result.RequestCode)
            {
                case MvxIntentRequestCode.PickFromFile:
                    uri = (result.Data == null) ? null : result.Data.Data;
                    break;
                case MvxIntentRequestCode.PickFromCamera:
                    uri = _cachedUriLocation;
                    break;
            }
            ProcessPictureUri(result, uri);
        }

        private bool ProcessPictureUri(MvxIntentResultEventArgs result, Uri uri)
        {
            if (_currentRequestParameters == null)
            {
                return false; // we have not handled this - so we return null
            }
            var responseSent = false;
            try
            {
                // Note for furture bug-fixing/maintenance - it might be better to use var outputFileUri = data.GetParcelableArrayExtra("outputFileuri") here?
                if (result.ResultCode != Result.Ok)
                {
                    return true;
                }
                if (uri == null
                || string.IsNullOrEmpty(uri.Path))
                {
                    return true;
                }
                var memoryStream = LoadInMemoryBitmap(uri);
                responseSent = true;
                _currentRequestParameters.PictureAvailable(memoryStream);
                return true;
            }
            finally
            {
                if (!responseSent)
                {
                    _currentRequestParameters.AssumeCancelled();
                }

                _currentRequestParameters = null;
            }
        }

        private MemoryStream LoadInMemoryBitmap(Uri uri)
        {
            var memoryStream = new MemoryStream();
            using (Bitmap bitmap = LoadScaledBitmap(uri))
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, _currentRequestParameters.PercentQuality, memoryStream);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return memoryStream;
        }

        private Bitmap LoadScaledBitmap(Uri uri)
        {
            ContentResolver contentResolver = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.ContentResolver;
            var maxDimensionSize = GetMaximumDimension(contentResolver, uri);
            var sampleSize = (int)Math.Ceiling(maxDimensionSize /
            ((double)_currentRequestParameters.MaxPixelDimension));
            if (sampleSize < 1)
            {
                // this shouldn't happen, but if it does... then trace the error and set sampleSize to 1
                sampleSize = 1;
            }

            return LoadResampledBitmap(contentResolver, uri, sampleSize);
        }

        private Bitmap LoadResampledBitmap(ContentResolver contentResolver, Uri uri, int sampleSize)
        {
            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var optionsDecode = new BitmapFactory.Options { InSampleSize = sampleSize };
                return BitmapFactory.DecodeStream(inputStream, null, optionsDecode);
            }
        }

        private static int GetMaximumDimension(ContentResolver contentResolver, Uri uri)
        {
            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var optionsJustBounds = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = true
                };
                var metadataResult = BitmapFactory.DecodeStream(inputStream, null, optionsJustBounds);
                var maxDimensionSize = Math.Max(optionsJustBounds.OutWidth, optionsJustBounds.OutHeight);
                return maxDimensionSize;
            }
        }

        private class RequestParameters
        {
            public RequestParameters(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
            {
                PercentQuality = percentQuality;
                MaxPixelDimension = maxPixelDimension;
                AssumeCancelled = assumeCancelled;
                PictureAvailable = pictureAvailable;
            }

            public Action<Stream> PictureAvailable { get; private set; }

            public Action AssumeCancelled { get; private set; }

            public int MaxPixelDimension { get; private set; }

            public int PercentQuality { get; private set; }
        }
    }
}