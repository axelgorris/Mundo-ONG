﻿using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using NGODirectory.Services;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class AnnouncementEditViewModel : BaseViewModel
    {
        public AnnouncementEditViewModel(Announcement item = null)
        {
            if (item != null)
            {
                Item = item;
                Title = "Editar noticia";
                ImageUrl = item.ImageUrl;
                IsEditMode = true;
            }
            else
            {
                Item = new Announcement();
                Title = "Nueva noticia";
                IsEditMode = false;
            }

            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            PickImageCommand = new Command(async () => await PickImageAsync());

            MessagingCenter.Subscribe<SettingsViewModel>(this, "RefreshLogin", (sender) =>
            {
                if (!(CloudService.IsUserLoggedIn() &&
                      CloudService.GetCurrentUser().UserId.Equals(Item.Author)))
                    NavigationService.Instance.GoToHomePage();
            });
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Announcement Item { get; set; }

        public Command SaveCommand { get; }
        async Task SaveAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!ValidateFields())
                {
                    await Application.Current.MainPage.DisplayAlert("Campos requeridos",
                                        "Por favor, introduce título y descripción de la notícia",
                                        "Ok");
                }
                else
                {
                    if ((await Application.Current.MainPage.DisplayActionSheet("Se guardarán los cambios realizados. ¿Estás seguro?", "Guardar", "Cancelar")).Equals("Guardar"))
                    {
                        if (Image != null)
                        {
                            ImageUrl = await CloudService.UploadStreamAsync(CloudService.GetCurrentUser().UserId, Image.GetStream());
                            Item.ImageUrl = ImageUrl;
                        }

                        var table = await CloudService.GetTableAsync<Announcement>();

                        if (Item.Id == null)
                        {
                            await table.CreateItemAsync(Item);
                        }
                        else
                        {
                            await table.UpdateItemAsync(Item);
                        }

                        await CloudService.SyncOfflineCacheAsync<Announcement>(overrideServerChanges: true);
                        MessagingCenter.Send(this, "ItemsChanged");
                        await NavigationService.Instance.GoToHomePage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AnnouncementEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateFields()
        {
            return !(string.IsNullOrEmpty(Item.Title) ||
                    string.IsNullOrEmpty(Item.Description));
        }

        public Command DeleteCommand { get; }
        async Task DeleteAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if ((await Application.Current.MainPage.DisplayActionSheet("Se eliminará la notica. ¿Estás seguro?", "Eliminar", "Cancelar")).Equals("Eliminar"))
                {
                    if (Item.Id != null)
                    {
                        var table = await CloudService.GetTableAsync<Announcement>();
                        await table.DeleteItemAsync(Item);
                        await CloudService.SyncOfflineCacheAsync<Announcement>(overrideServerChanges: true);
                        MessagingCenter.Send(this, "ItemsChanged");
                    }

                    await NavigationService.Instance.GoToHomePage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AnnouncementEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value, "ImageUrl"); }
        }

        private MediaFile image;
        public MediaFile Image
        {
            get { return image; }
            set { SetProperty(ref image, value, "Image"); }
        }

        public Command PickImageCommand { get; }
        private async Task PickImageAsync()
        {
            var result = await PhotoService.Instance.PickPhotoAsync();

            if (result != null)
            {
                Image = result;
                ImageUrl = result.Path;
            }
        }

        public bool IsEditMode { get; private set; }
        
    }
}
