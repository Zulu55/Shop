namespace Shop.UICross.Android.Services
{
    using Common.Interfaces;
    using global::Android.App;
    using global::Android.Views;
    using global::Android.Widget;
    using MvvmCross;
    using MvvmCross.Platforms.Android;
    using Shop.Common.Models;
    using System;

    public class DialogService : IDialogService
    {
        public void Alert(string title, string message, string okbtnText)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            var adb = new AlertDialog.Builder(act);
            adb.SetTitle(title);
            adb.SetMessage(message);
            adb.SetPositiveButton(okbtnText, (sender, args) => { });
            adb.Create().Show();
        }

        public void Alert(string title, string message, string okbtnText, Action confirmed)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            var adb = new AlertDialog.Builder(act);
            adb.SetTitle(title);
            adb.SetMessage(message);
            adb.SetPositiveButton(okbtnText, (sender, args) =>
            {
                if (confirmed != null)
                {
                    confirmed.Invoke();
                }
            });

            adb.Create().Show();
        }

        public void Confirm(
            string title,
            string message,
            string okButtonTitle,
            string dismissButtonTitle,
            Action confirmed,
            Action dismissed)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            AlertDialog alertdialog = builder.Create();
            builder.SetTitle(title);
            builder.SetMessage(message);

            builder.SetNegativeButton(dismissButtonTitle, (senderAlert, args) =>
            {
                if (dismissed != null)
                {
                    dismissed.Invoke();
                }
            });

            builder.SetPositiveButton(okButtonTitle, (senderAlert, args) =>
            {
                if (confirmed != null)
                {
                    confirmed.Invoke();
                }
            });

            builder.Show();
        }

        public void CustomAlert(
            DialogType dialogType,
            string title,
            string message,
            string okbtnText,
            Action confirmed)
        {
            var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;
            AlertDialog builder = new AlertDialog.Builder(act).Create();
            builder.SetCancelable(false);

            var customView = LayoutInflater.From(act).Inflate(Android.Resource.Layout.CustomAlertPage, null);

            var okButton = customView.FindViewById<Button>(Android.Resource.Id.btnOK);
            var titleTextView = customView.FindViewById<TextView>(Android.Resource.Id.title);
            var imageImageView = customView.FindViewById<ImageView>(Android.Resource.Id.image);
            var messageTextView = customView.FindViewById<TextView>(Android.Resource.Id.textViewMessage);

            imageImageView.SetImageResource(this.GetImageId(dialogType));
            titleTextView.Text = title;
            messageTextView.Text = message;
            okButton.Text = okbtnText;

            okButton.Click += delegate
            {
                if (confirmed != null)
                {
                    confirmed.Invoke();
                }
                else
                {
                    builder.Dismiss();
                }
            };

            builder.SetView(customView);
            builder.Show();
        }

        public void CustomAlert(
            DialogType dialogType,
            string title,
            string message,
            string okButtonTitle,
            string dismissButtonTitle,
            Action confirmed,
            Action dismissed)
        {
            var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;
            AlertDialog builder = new AlertDialog.Builder(act).Create();
            builder.SetCancelable(false);

            var customView = LayoutInflater.From(act).Inflate(Android.Resource.Layout.CustomAlertPage2, null);

            var oneButton = customView.FindViewById<Button>(Android.Resource.Id.btnOne);
            var twoButton = customView.FindViewById<Button>(Android.Resource.Id.btnTwo);
            var titleTextView = customView.FindViewById<TextView>(Android.Resource.Id.title);
            var imageImageView = customView.FindViewById<ImageView>(Android.Resource.Id.image);
            var messageTextView = customView.FindViewById<TextView>(Android.Resource.Id.textViewMessage);

            imageImageView.SetImageResource(this.GetImageId(dialogType));
            titleTextView.Text = title;
            messageTextView.Text = message;
            oneButton.Text = okButtonTitle;
            twoButton.Text = dismissButtonTitle;

            oneButton.Click += delegate
            {
                if (confirmed != null)
                {
                    confirmed.Invoke();
                }
                else
                {
                    builder.Dismiss();
                }
            };

            twoButton.Click += delegate
            {
                if (dismissed != null)
                {
                    dismissed.Invoke();
                }
                else
                {
                    builder.Dismiss();
                }
            };

            builder.SetView(customView);
            builder.Show();
        }

        private int GetImageId(DialogType dialogType)
        {
            switch (dialogType)
            {
                case DialogType.Alert:
                    return (int)typeof(Android.Resource.Drawable).GetField("alert").GetValue(null);
                case DialogType.Information:
                    return (int)typeof(Android.Resource.Drawable).GetField("information").GetValue(null);
                case DialogType.Question:
                    return (int)typeof(Android.Resource.Drawable).GetField("question").GetValue(null);
                case DialogType.Warning:
                    return (int)typeof(Android.Resource.Drawable).GetField("warning").GetValue(null);
                default:
                    return (int)typeof(Android.Resource.Drawable).GetField("alert").GetValue(null);
            }
        }
    }
}