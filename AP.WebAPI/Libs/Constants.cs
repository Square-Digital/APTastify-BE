namespace AP.WebAPI.Libs;

public static class Constants
{
    public const string ConnectionDevelopment = "connection-dev";
    public const string ConnectionStrings = "connection-strings";
    public const string ConnectionProduction = "connection-prod";
    public const string EmailSettingsProd = "email-settings";

    // Configuration Keys

    public const string Connection = "connection";
    public const string SmtpServer = "smtp-server";
    public const string SmtpPort = "smtp-port";
    public const string SmtpUser = "smtp-user";
    public const string FromEmail = "from-email";
    public const string FromName = "from-name";
    public const string SmtpPassword = "password";
    public const string SmtpUsername = "username";

    //

    public const string CorsPolicy = "aptastify-cors-policy";

    // Admitted URLS - CORS
    public const string APTastifyUIALB = "http://aptastify-ui-1364817618.us-east-2.elb.amazonaws.com";
    public const string LocalDev = "http://localhost:4200";
}