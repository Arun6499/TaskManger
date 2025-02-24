using TaskManger.DTO;
using TaskManger.Models;

namespace TaskManger.Repository
{
    public interface ILoginServices
    {
        Task<List<Login>> GetAll();
        Task<List<TaskModule>> GetTaskDetails();
        Task<object> LoginUser(LoginDto Dto);
        Task<object> TaskInsertion(Models.TaskModule task);
        Task<object> TaskUpdate(TaskModuleUpdate task);
        Task<object> TaskDelete(int taskid);
        Task<object> GetTaskById(int taskId);
    }
}
