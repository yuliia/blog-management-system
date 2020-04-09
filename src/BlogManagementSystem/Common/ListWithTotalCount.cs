namespace BlogManagementSystem.Common
{
    public class ListWithTotalCount<T>
    {
        public ListWithTotalCount(T[] items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public T[] Items { get; }
        public int TotalCount { get; }
    }
}