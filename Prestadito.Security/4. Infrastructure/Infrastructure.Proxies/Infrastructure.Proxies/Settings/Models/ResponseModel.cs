namespace Prestadito.Security.Infrastructure.Proxies.Settings.Models
{
    public class ResponseProxyModel<T> where T : class
    {
        public T Item { get; set; } = null!;
        public List<T> Items { get; set; } = null!;
        public int TotalItems { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; } = null!;
    }
}
