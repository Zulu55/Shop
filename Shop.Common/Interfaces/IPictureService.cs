namespace Shop.Common.Interfaces
{
    using System;

    public interface IPictureService
    {
        void TakeNewPhoto(Action<byte[]> onSuccess, Action<string> onError);

        void SelectExistingPicture(Action<byte[]> onSuccess, Action<string> onError);
    }
}