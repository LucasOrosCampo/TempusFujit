using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class MainPage : ContentPage
{
    public Thickness GridMargin = new(100, 0, 100, 0);
    private int clientsAdded = 0;
    public MainPage(MainPageViewModel vm)
    {
        Application.Current.Resources[nameof(GridMargin)] = GridMargin;
        void ApplyStyleToChildren(object obj, ElementEventArgs args)
        {
            var grid = (Grid)args.Element;
            if (clientsAdded % 2 == 0)
            {
                grid.BackgroundColor = new Color(208, 240, 192);
            }
            else
            {
                grid.BackgroundColor = new Color(169, 186, 157);
            }
            clientsAdded++;
        }
        InitializeComponent();
        BindingContext = vm;
        clientsCollection.ChildAdded += ApplyStyleToChildren;
    }

    private void OnGoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private void GoToClientFromLabel(object sender, EventArgs args)
    {
        var targetClientId = (int)((TappedEventArgs)args).Parameter;
        Shell.Current.GoToAsync($"ClientOverview?clientId={targetClientId}");
    }


    void ShowTrashIcon(object sender, PointerEventArgs args)
    {
        var btn = (ImageButton)((Grid)sender).Children[2];
        btn.IsVisible = true;
    }
    void HideTrashIcon(object sender, PointerEventArgs args)
    {
        var btn = (ImageButton)((Grid)sender).Children[2];
        btn.IsVisible = false;
    }
}