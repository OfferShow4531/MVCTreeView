namespace MVCTreeView.Models.MenuModels
{
    public class Location
    {  
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ParentId { get; set; }
        public int MenuNumber { get; set; }

    }
}
