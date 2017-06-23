using NGODirectory.Abstractions;
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
    public class OrganizationEditViewModel : BaseViewModel
    {
        public OrganizationEditViewModel(Organization item = null)
        {
            if (item != null)
            {
                Item = item;
                Title = "Editar organización";
                LogoUrl = item.LogoUrl;
                IsEditMode = true;
            }
            else
            {
                Item = new Organization();
                Title = "Nueva organización";
                IsEditMode = false;
            }

            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            PickImageCommand = new Command(async () => await PickImageAsync());

            MessagingCenter.Subscribe<MasterModel>(this, "RefreshLogin", (sender) =>
            {
                if (!(CloudService.IsUserLoggedIn() &&
                      CloudService.GetCurrentUser().UserId.Equals(Item.AdminUser)))
                    NavigationService.Instance.GoToHomePage();
            });
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Organization Item { get; set; }

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
                                        "Por favor, introduce nombre y descripción de la organización",
                                        "Ok");
                }
                else
                {
                    if ((await Application.Current.MainPage.DisplayActionSheet("Se guardarán los cambios realizados. ¿Estás seguro?", "Guardar", "Cancelar")).Equals("Guardar"))
                    {
                        if (Image != null)
                        {
                            LogoUrl = await CloudService.UploadStreamAsync(CloudService.GetCurrentUser().UserId, Image.GetStream());
                            Item.LogoUrl = LogoUrl;
                        }

                        var table = await CloudService.GetTableAsync<Organization>();

                        if (Item.Id == null)
                        {
                            await table.CreateItemAsync(Item);
                        }
                        else
                        {
                            await table.UpdateItemAsync(Item);
                        }

                        await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                        MessagingCenter.Send(this, "ItemsChanged");
                        await NavigationService.Instance.GoToHomePage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateFields()
        {
            return !(string.IsNullOrEmpty(Item.Name) ||
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
                if ((await Application.Current.MainPage.DisplayActionSheet("Se eliminará la organización. ¿Estás seguro?", "Eliminar", "Cancelar")).Equals("Eliminar"))
                {
                    if (Item.Id != null)
                    {
                        var table = await CloudService.GetTableAsync<Organization>();
                        await table.DeleteItemAsync(Item);
                        await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                        MessagingCenter.Send(this, "ItemsChanged");
                    }

                    await NavigationService.Instance.GoToHomePage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string logoUrl;
        public string LogoUrl
        {
            get { return logoUrl; }
            set { SetProperty(ref logoUrl, value, "LogoUrl"); }
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
                LogoUrl = result.Path;
            }
        }

        public bool IsEditMode { get; private set; }
    }
}