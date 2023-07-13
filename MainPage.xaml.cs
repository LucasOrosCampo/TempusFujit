namespace TempusFujit;

public partial class MainPage : ContentPage
{
	public Thickness GridMargin = new (100,0,100,0);
	private int clientsAdded = 0;
    public MainPage()
	{
		Application.Current.Resources[nameof(GridMargin)] = GridMargin;
		void ApplyStyleToChildren(object obj, ElementEventArgs args)
		{
			var grid = (Grid)args.Element;
			if (clientsAdded %  2 == 0) 
			{
				grid.BackgroundColor = new Color(208,240,192);
			}
			else
			{
				grid.BackgroundColor = new Color(169, 186, 157);
			} 
			clientsAdded++;
		}
        InitializeComponent();
		clientsCollection.ChildAdded += ApplyStyleToChildren;
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


	void ShowTrashIcon(object sender, PointerEventArgs args)
	{
		var btn =(ImageButton)((Grid)sender).Children[2];
		btn.IsVisible = true;
	}
	void HideTrashIcon(object sender, PointerEventArgs args)
	{
        var btn = (ImageButton)((Grid)sender).Children[2];
        btn.IsVisible = false;
    }


}