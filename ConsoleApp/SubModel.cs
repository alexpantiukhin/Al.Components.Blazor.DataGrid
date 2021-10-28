namespace ConsoleApp
{
    internal class SubModel
    {
        public int Id { get; set; }
        public string SubModelName { get; set; }
        public Model ParentModel { get; set; }
    }
}
