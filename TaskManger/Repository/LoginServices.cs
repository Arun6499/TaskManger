using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManger.Data;
using TaskManger.DTO;
using TaskManger.Models;

namespace TaskManger.Repository
{
    public class LoginServices : ILoginServices
    {
        LoginContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _conn;

        public LoginServices(LoginContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _conn = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Login>> GetAll()
        {
            return await _context.Logins.ToListAsync();
        }
        public async Task<object> LoginUser(LoginDto dto)
        {
            try
            {
                var user = _context.Logins.Where(u => u.Username == dto.Username && u.Password == dto.Password && u.Status == true).FirstOrDefault();

                if (user != null)
                {
                    var LoginDetail = new
                    {
                        status = true,
                        message = "Successful Login"
                    };
                    return LoginDetail;
                }
                else
                {
                    var LoginDetails = new
                    {
                        status = false,
                        message = "Invalid UserName and Password"
                    };
                    return LoginDetails;
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    status = false,
                    message = "Something went wrong: " + ex.Message
                };
                return errorResponse;
            }
        }
        public async Task<object> TaskInsertion(Models.TaskModule task)
        {
            int rowsAffected = 0;
            try
            {

                using (SqlConnection conn = new SqlConnection(_conn))
                {
                    await conn.OpenAsync();

                    using (SqlCommand sqlcmd = conn.CreateCommand())
                    {
                        DataSet ds = new DataSet();
                        sqlcmd.CommandText = "ManageTask";
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "INSERT";
                        sqlcmd.Parameters.Add("@TaskID", SqlDbType.VarChar).Value = task.TaskId;
                        sqlcmd.Parameters.Add("@TaskTitle", SqlDbType.VarChar).Value = task.Tasktitle;
                        sqlcmd.Parameters.Add("@TaskDescription", SqlDbType.VarChar).Value = task.TaskDescription;
                        sqlcmd.Parameters.Add("@TaskStatus", SqlDbType.VarChar).Value = task.TaskStatus;
                        sqlcmd.Parameters.Add("@TaskDate", SqlDbType.DateTime).Value = task.TaskDate; //string.IsNullOrEmpty(task.TaskDate) ? DBNull.Value : DateTime.ParseExact(task.TaskDate, "dd-MM-yyyy", null);
                        sqlcmd.Parameters.Add("@TaskPriority", SqlDbType.VarChar).Value = task.TaskPriority;
                        rowsAffected = await sqlcmd.ExecuteNonQueryAsync();
                    }
                }
                if (rowsAffected > 0 || rowsAffected == -1)
                {
                    var BranchDetails = new
                    {
                        status = true,
                        message = "Task Manager Inserted succesfully",
                    };
                    return BranchDetails;
                }
                else
                {
                    var BranchDetails = new
                    {
                        status = false,
                        message = "Something is wrong"
                    };
                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    status = false,
                    message = "Something went wrong: " + ex.Message
                };
                return errorResponse;
            }
        }
        public async Task<object> TaskUpdate(TaskModuleUpdate task)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(_conn))
                {
                    await conn.OpenAsync();

                    using (SqlCommand sqlcmd = conn.CreateCommand())
                    {
                        sqlcmd.CommandText = "ManageTask";
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "UPDATE";
                        sqlcmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = task.TaskId;  // Make sure TaskId is an integer
                        sqlcmd.Parameters.Add("@TaskTitle", SqlDbType.VarChar).Value = task.Tasktitle ?? (object)DBNull.Value;
                        sqlcmd.Parameters.Add("@TaskDescription", SqlDbType.VarChar).Value = task.TaskDescription ?? (object)DBNull.Value;
                        sqlcmd.Parameters.Add("@TaskStatus", SqlDbType.VarChar).Value = task.TaskStatus ?? (object)DBNull.Value;
                        //sqlcmd.Parameters.Add("@TaskDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(task.TaskDate)
                        //    ? (object)DBNull.Value
                        //    : DateTime.ParseExact(task.TaskDate, "dd-MM-yyyy", null);
                        sqlcmd.Parameters.Add("@TaskPriority", SqlDbType.VarChar).Value = task.TaskPriority ?? (object)DBNull.Value;
                        rowsAffected = await sqlcmd.ExecuteNonQueryAsync();
                    }
                }

                if (rowsAffected > 0 || rowsAffected == -1)
                {
                    return new
                    {
                        status = true,
                        message = "Task updated successfully."
                    };
                }
                else
                {
                    return new
                    {
                        status = false,
                        message = "No task found with the provided TaskId or update failed."
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    status = false,
                    message = "Something went wrong: " + ex.Message
                };
            }
        }
        public async Task<object> TaskDelete(int taskId)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(_conn))
                {
                    await conn.OpenAsync();

                    using (SqlCommand sqlcmd = conn.CreateCommand())
                    {
                        sqlcmd.CommandText = "DELETE FROM Task WHERE TaskID = @TaskId";
                        sqlcmd.CommandType = CommandType.Text;

                        sqlcmd.Parameters.Add("@TaskId", SqlDbType.Int).Value = taskId;

                        rowsAffected = await sqlcmd.ExecuteNonQueryAsync();
                        //return rowsAffected > 0; // Return true if a row was deleted
                    }
                    if (rowsAffected > 0 || rowsAffected == -1)
                    {
                        return new
                        {
                            status = true,
                            message = "Task Deleted successfully."
                        };
                    }
                    else
                    {
                        return new
                        {
                            status = false,
                            message = "No task found with the provided TaskId."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    status = false,
                    message = "Something went wrong: " + ex.Message
                };
            }
        }
        public async Task<List<TaskModule>> GetTaskDetails()
        {
            return await _context.TaskModules.ToListAsync(); // Will now query "Task" table
        }
        //public async Task<object> GetTaskById(int taskId)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_conn))
        //        {
        //            await conn.OpenAsync();

        //            using (SqlCommand sqlcmd = conn.CreateCommand())
        //            {
        //                sqlcmd.CommandText = "SELECT * FROM Task WHERE TaskID = @TaskId";
        //                sqlcmd.CommandType = CommandType.Text;

        //                sqlcmd.Parameters.Add("@TaskId", SqlDbType.Int).Value = taskId;

        //                using (SqlDataReader reader = await sqlcmd.ExecuteReaderAsync())
        //                {
        //                    if (await reader.ReadAsync()) // Check if any data is returned
        //                    {
        //                        var task = new TaskModule
        //                        {
        //                            TaskId = reader.GetInt32(reader.GetOrdinal("TaskID")),
        //                            Tasktitle = reader.GetString(reader.GetOrdinal("TaskTitle")),
        //                            TaskDescription = reader.GetString(reader.GetOrdinal("TaskDescription")),
        //                            TaskStatus = reader.GetString(reader.GetOrdinal("TaskStatus")),
        //                            TaskPriority = reader.GetString(reader.GetOrdinal("TaskPriority")),
        //                            TaskDate = reader.GetString(reader.GetOrdinal("TaskDate"))
        //                            };

        //                        return task;
        //                    }
        //                    else
        //                    {
        //                        return null; // No task found with the provided TaskID
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // Log or handle the error appropriately
        //        return null; // Return null if there's an error
        //    }
        //}
        public async Task<object> GetTaskById(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_conn))
                {
                    await conn.OpenAsync();
                    var tasks = new List<Models.TaskModule>();

                    using (SqlCommand sqlcmd = conn.CreateCommand())
                    {
                        sqlcmd.CommandText = "SELECT * FROM Task WHERE TaskID = @TaskId";
                        sqlcmd.CommandType = CommandType.Text;

                        sqlcmd.Parameters.Add("@TaskId", SqlDbType.Int).Value = taskId;

                        using (SqlDataReader reader = await sqlcmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync()) // Check if any data is returned
                            {
                                var task = new Models.TaskModule()
                                {
                                    TaskId = reader.GetInt32(reader.GetOrdinal("TaskID")),
                                    Tasktitle = reader.GetString(reader.GetOrdinal("TaskTitle")),
                                    TaskDescription = reader.GetString(reader.GetOrdinal("TaskDescription")),
                                    TaskStatus = reader.GetString(reader.GetOrdinal("TaskStatus")),
                                    TaskPriority = reader.GetString(reader.GetOrdinal("TaskPriority")),
                                    // Convert TaskDate to string and handle DBNull case
                                    //TaskDate = reader.IsDBNull(reader.GetOrdinal("TaskDate"))
                                    //    ? null
                                    //    : reader.GetDateTime(reader.GetOrdinal("TaskDate")).ToString("yyyy-MM-dd") // You can change the format if needed
                                    TaskDate = reader.IsDBNull(reader.GetOrdinal("TaskDate"))
    ? (DateTime?)null
    : reader.GetDateTime(reader.GetOrdinal("TaskDate"))

                                };
                                tasks.Add(task);

                                //   return task;
                            }
                            if (tasks.Count > 0)
                            {
                                return new
                                {
                                    status = true,
                                    taskDetails = tasks
                                };
                            }

                            else
                            {
                                return new
                                {
                                    status = false,
                                    message = "No data found"
                                };
                                //return null; // No task found with the provided TaskID
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Log or handle the error appropriately
                return null; // Return null if there's an error
            }
        }


    }
}
