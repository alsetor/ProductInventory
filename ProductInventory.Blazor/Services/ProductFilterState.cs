namespace ProductInventory.Blazor.Services
{
    public class ProductFilterState
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = 6000;
        public int Page { get; set; } = 1;
        public int TotalCount { get; set; } = 0;

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / 10);

        public void Clear()
        {
            Name = null;
            MinPrice = null;
            MaxPrice = null;
            TotalCount = 0;
        }
    }
}
