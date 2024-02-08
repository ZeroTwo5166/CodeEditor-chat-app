namespace backend.ModelsDTO
{
    public class ProjectDTO
    {
        public Guid? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Room { get; set; }
        public string? Code { get; set; } //Do I really need it? Dont think so
    }
}
