namespace TaskManger.Models
{
    public class TaskModule
    {
        public int TaskId { get; set; }
        public string? Tasktitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskPriority { get; set; }
        public DateTime? TaskDate { get; set; }
    }
    public class TaskModuleUpdate
    {
        public int TaskId { get; set; }
        public string? Tasktitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskPriority { get; set; }
       // public string? TaskDate { get; set; }
    }

}
