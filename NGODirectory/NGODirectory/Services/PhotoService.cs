﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace NGODirectory.Services
{
    public class PhotoService
    {
        private static PhotoService _instance;

        public static PhotoService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PhotoService();
                }

                return _instance;
            }
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            try
            {
                MediaFile image = null;
                await CrossMedia.Current.Initialize();
                
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                {
                    return null;
                }
                
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (storageStatus != PermissionStatus.Granted)
                    await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage);

                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 92
                });

                if (file != null)
                {
                    image = file;
                }

                return image;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            try
            {
                MediaFile image = null;
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }

                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (cameraStatus != PermissionStatus.Granted)
                    await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera);

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    AllowCropping = true,
                    CompressionQuality = 92                    
                });

                if (file != null)
                {
                    image = file;
                }

                return image;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }
    }
}
