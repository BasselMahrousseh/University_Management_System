namespace Univ_Manage.ViewModels
{
    public class PageViewModel<T>
    {
        public List<T> Records { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public IEnumerator<T> GetEnumerator()
        {
            return this.Records.GetEnumerator();
        }
    }
}
