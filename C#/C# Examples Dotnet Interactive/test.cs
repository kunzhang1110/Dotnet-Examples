[HttpGet]
[ResponseCache(Duration = 60)] // Using ResponseCache attribute for simplicity
public IActionResult Get()
{
    Response.Headers["Cache-Control"] = "public,max-age=3600"; // Cache publicly for 1 hour
    Response.Headers["Expires"] = DateTime.UtcNow.AddHours(1).ToString("R"); // Expiry time
    return Ok("Cached response data");
}



builder.Services.AddResponseCaching();
// UseCors must be called before UseResponseCaching
//app.UseCors();
app.UseResponseCaching();