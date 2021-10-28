namespace ConsoleApp
{
    internal class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SubModel> SubModels { get; set; }

    }
}
