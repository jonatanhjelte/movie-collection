using Microsoft.AspNetCore.Components;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class BasePageModel : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
    }
}
