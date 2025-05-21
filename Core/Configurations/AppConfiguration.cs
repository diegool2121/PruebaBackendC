public static class AppConfiguration
{
    public static void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthorization();
        app.MapControllers();
        app.Urls.Add("http://0.0.0.0:80");
    }
}