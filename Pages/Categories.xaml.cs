using TempusFujit.Infra;
using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class Categories : ContentPage
{
    public CategoriesVM vm { get; }
    public Categories()
    {
        InitializeComponent();
        vm = Services.Get<CategoriesVM>() as CategoriesVM;
        BindingContext = vm;
    }
}