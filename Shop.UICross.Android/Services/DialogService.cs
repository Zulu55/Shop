namespace Shop.UICross.Android.Services
{
    using Common.Interfaces;
    using global::Android.App;
    using MvvmCross;
    using MvvmCross.Platforms.Android;
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
            adb.SetPositiveButton(okbtnText, (sender, args) => { /* some logic */ });
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

            builder.SetNegativeButton(dismissButtonTitle, (senderAlert, args) => {
                if (dismissed != null)
                {
                    dismissed.Invoke();
                }
            });

            builder.SetPositiveButton(okButtonTitle, (senderAlert, args) => {
                if (confirmed != null)
                {
                    confirmed.Invoke();
                }
            });

            builder.Show();
        }
    }
}