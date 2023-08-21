using System.Runtime.ConstrainedExecution;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Floor;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.Rasters;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace ArcGisEmbeddingSample.MauiControls;

public partial class EsriSamplePage : ContentView
{
	private FloorManager _floorManager;
	private readonly Dictionary<string, FloorLevel> _floorOptions = new Dictionary<string, FloorLevel>();
	private readonly MapView MyMapView = new MapView();

	public EsriSamplePage()
	{
		InitializeComponent();
		_ = Initialize();
	}

	private async Task Initialize()
	{
		try
		{
			// Gets the floor data from ArcGIS Online and creates a map with it.
			Map map = new Map(new Uri("https://ess.maps.arcgis.com/home/item.html?id=f133a698536f44c8884ad81f80b6cfc7"));

			MyMapView.Map = map;

			// Map needs to be loaded in order for floormanager to be used.
			LoadingLabel.Text = "Loading Map....";
			await MyMapView.Map.LoadAsync();
			LoadingLabel.Text = "Checking Floor Manager....";
			List<string> floorName = new List<string>();

			// Checks to see if the layer is floor aware.
			if (MyMapView.Map.FloorDefinition == null)
			{
				System.Diagnostics.Debug.WriteLine("The layer is not floor aware.");
				return;
			}

			if (MyMapView.Map.FloorManager is null)
			{
				await (Application.Current?.MainPage?.DisplayAlert("Null Reference", "The FloorManager is null", "Ok") ?? Task.CompletedTask);
				return;
			}

			LoadingLabel.Text = "Loading Floor Manager...";
			await MyMapView.Map.FloorManager.LoadAsync();
			_floorManager = MyMapView.Map.FloorManager;

			// Use the dictionary to add the level's name as the key and the FloorLevel object with the associated level's name.
			LoadingLabel.Text = "Getting Floor Levels...";
			foreach (FloorLevel level in _floorManager.Facilities[0].Levels)
			{
				_floorOptions.Add(level.ShortName, level);
				floorName.Add(level.ShortName);
			}

			Content = MyMapView;
			//FloorChooser.ItemsSource = floorName;
		}
		catch (Exception ex)
		{
			Content = new ScrollView
			{
				Content = new VerticalStackLayout()
				{
					new Label { Text = "An Unexpected Error Occurred:", FontAttributes = FontAttributes.Bold },
					new Label { Text = ex.GetType().FullName },
					new Label { Text = ex.Message },
					new Label { Text = ex.StackTrace?.ToString() },
				}
			};
			System.Diagnostics.Debug.WriteLine(ex.Message);
		}
	}
}