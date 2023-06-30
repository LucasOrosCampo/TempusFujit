namespace TempusFujit;

public partial class MainPage : ContentPage
{
	public Thickness GridMargin = new (100,10,100,0);
    public MainPage()
	{
		Application.Current.Resources[nameof(GridMargin)] = GridMargin;
		InitializeComponent();
	}

	private void OnGoBackClicked(object sender, EventArgs e)
	{
        Shell.Current.GoToAsync("//LoginPage");
    }

	private void GoToClientFromLabel(object sender, EventArgs args)
	{ 
		var id = ((TappedEventArgs) args).Parameter;
        Shell.Current.GoToAsync($"ClientPage?clientId={id}");
    }



}