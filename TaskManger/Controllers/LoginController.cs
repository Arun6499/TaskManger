using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManger.DTO;
using TaskManger.Models;
using TaskManger.Repository;

namespace TaskManger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        ILoginServices _loginServices;

        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        [HttpGet]
        [Route("GetAllLogs")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _loginServices.GetAll();
            return Ok(logs);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> LoginUser([FromBody] LoginDto dto)
        {
            var user=await _loginServices.LoginUser(dto);
            return Ok(user);
        }

        [HttpPost]
        [Route("TaskInsert")]
        public async Task<object> TaskInsertion([FromBody] Models.TaskModule taskDTO)
        {
            var taskinsert=await _loginServices.TaskInsertion(taskDTO); 
            return taskinsert;
        }

        [HttpPost]
        [Route("TaskUpdate")]
        public async Task<object> TaskUpdate([FromBody] TaskModuleUpdate taskDTO)
        {
            var taskupdate = await _loginServices.TaskUpdate(taskDTO);
            return taskupdate;
        }

        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<object> TaskDelete(int taskId)
        {
            var taskdelete = await _loginServices.TaskDelete(taskId);
            return taskdelete;
        }

        [HttpGet]
        [Route("GetAllTask")]
        public async Task<IActionResult> GetTaskDetails()
        {
            var taskdetail = await _loginServices.GetTaskDetails();
            return Ok(taskdetail);
        }

        [HttpGet]
        [Route("GetTaskById")]
        public async Task<object> GetTaskById(int taskId)
        {
            var getbyid=await _loginServices.GetTaskById(taskId);
            return getbyid;
        }
    }
}
