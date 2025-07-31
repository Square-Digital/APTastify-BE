using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AP.BusinessInterfaces.Data.Error;

public class APWebAPIError
{
    [MaxLength(100)] public required string Message { get; set; }
    public int StatusCode { get; set; }
}