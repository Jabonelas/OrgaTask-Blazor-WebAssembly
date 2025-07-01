namespace Blazor_WebAssembly.Helpers
{
    public class ApiRoutes
    {
        public static string SetandoEndPoint(string _endPoint)
        {
            string endPoint = "";

#if DEBUG
            endPoint = $"{_endPoint}";
#else
            endPoint = $"https://blazor-api.onrender.com{_endPoint}";
#endif
            return endPoint;
        }
    }
}
