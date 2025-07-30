namespace AP.WebAPI.RequestPipeline;

    public static class WebApplicationExtensions
    {
        public static void InitializeApplication(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            // app.UseCors(Constants.CorsPolicyName);
            app.UseAuthentication();
            // app.UseMiddleware<PermissionMiddleware>();
            app.UseForwardedHeaders();
            // app.UseMiddleware<ExceptionHandlerMiddleware>();
        //    app.UseAuthorization();
        
            app.MapControllers();

            app.Run();
        }
    }
