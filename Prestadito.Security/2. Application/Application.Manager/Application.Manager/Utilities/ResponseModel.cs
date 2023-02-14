namespace Prestadito.Security.Application.Manager.Utilities
{
    public class ResponseModel<T> where T : class
    {
        public T Item { get; set; } = null!;
        public List<T> Items { get; set; } = null!;
        public int TotalItems { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; } = null!;

        public static ResponseModel<T> GetResponse(string errorMessage)
        {
            return new ResponseModel<T>
            {
                Error = true,
                ErrorMessage = errorMessage
            };
        }

        public static ResponseModel<T> GetResponse(T item)
        {
            return new ResponseModel<T>
            {
                Error = false,
                Item = item,
                TotalItems = 1
            };
        }

        public static ResponseModel<T> GetResponse(List<T> items)
        {
            return new ResponseModel<T>
            {
                Error = false,
                Items = items,
                TotalItems = items.Count
            };
        }
    }
}
