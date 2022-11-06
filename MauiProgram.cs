namespace Lab6Starter;

/**
 * Name: Shabbar & Thomas
 * Date: 11/05/2022
 * Description:This the main page for the Tic-Tac-Toe application
 * Bugs: n/a
 * Reflection: It was fairly simple, but a valueable experience in 
 * forking repos, GitHub, and paired programming.
 */

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}