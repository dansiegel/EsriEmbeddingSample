using Esri.ArcGISRuntime.Maui;

namespace ArcGisEmbeddingSample;

public static class AppBuilderExtensions
{
	public static MauiAppBuilder UseMauiControls(this MauiAppBuilder builder)
	{
#if __ANDROID__
		Esri.ArcGISRuntime.UI.Controls.SceneView.MemoryLimit = 2 * 1073741824L; // 2Gb
#endif

		return builder
			.UseArcGISRuntime()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("ArcGisEmbeddingSample/Assets/Fonts/OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("ArcGisEmbeddingSample/Assets/Fonts/OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("ArcGisEmbeddingSample/Assets/Fonts/calcite-ui-icons-24.ttf", "calcite-ui-icons-24");
			});
	}
}