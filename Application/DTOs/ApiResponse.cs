namespace AgroSolutions.Identity.Web.Application.DTOs;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new();
}
