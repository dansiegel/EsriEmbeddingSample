using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Floor;
using Esri.ArcGISRuntime.Rasters;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace ArcGisEmbeddingSample.MauiControls;

public partial class EsriSamplePage : ContentView
{
	private FloorManager _floorManager;
	private readonly Dictionary<string, FloorLevel> _floorOptions = new Dictionary<string, FloorLevel>();

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
			await MyMapView.Map.LoadAsync();
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

			await MyMapView.Map.FloorManager.LoadAsync();
			_floorManager = MyMapView.Map.FloorManager;

			// Use the dictionary to add the level's name as the key and the FloorLevel object with the associated level's name.
			foreach (FloorLevel level in _floorManager.Facilities[0].Levels)
			{
				_floorOptions.Add(level.ShortName, level);
				floorName.Add(level.ShortName);
			}

			//FloorChooser.ItemsSource = floorName;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex.Message);
		}
	}
}