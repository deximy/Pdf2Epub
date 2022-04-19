using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pdf2Epub.API.Hubs;
using Pdf2Epub.API.Models;
using Pdf2Epub.API.Repositories;
using Pdf2Epub.API.Services;

namespace Pdf2Epub.API.Controllers
{
    [Route("convert")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private readonly ConvertTaskRepository convert_task_repository_;
        private readonly WorkerService worker_service_;
        private readonly IHubContext<MessageHub> hub_context_;

        public ConvertController(ConvertTaskRepository convert_task_repository, WorkerService worker_service)
        {
            convert_task_repository_ = convert_task_repository;
            worker_service_ = worker_service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromForm] string file_id, [FromForm] bool is_vertical)
        {
            var id = await convert_task_repository_.NewTask();

            await convert_task_repository_.UpdateFileName(id, file_id);
            await convert_task_repository_.UpdateVertical(id, is_vertical);

            if (!System.IO.File.Exists($"/root/{file_id}"))
            {
                await convert_task_repository_.UpdateTaskState(id, ConvertStatus.UPLOAD_FAILED);
                return NotFound();
            }

            await worker_service_.SendTaskToAllWorker(id);

            return Ok(id);
        }

        [HttpPatch("{task_id}")]
        public async Task<IActionResult> FinishTask([FromRoute] Guid task_id, [FromForm] string file_name)
        {
            await convert_task_repository_.UpdateTaskState(task_id, ConvertStatus.CONVERTION_SUCCEED);
            await convert_task_repository_.UpdateEndtime(task_id);
            await convert_task_repository_.UpdateResultFileName(task_id, file_name);
            await hub_context_.Clients.All.SendAsync("ReceiveFrontendMessage", file_name);
            return Ok();
        }

        [HttpGet("{task_id}")]
        public async Task<IActionResult> GetTaskInfo([FromRoute] Guid task_id, [FromQuery] string type)
        {
            if (type == "progess")
            {
                return Ok(
                    (await convert_task_repository_.Query(x => x.id == task_id))
                    .Select(x => x.status)
                    .First()
                    .ToString()
                );
            }
            
            if (type == "result")
            {
                return Ok(
                    (await convert_task_repository_.Query(x => x.id == task_id))
                    ?.Select(x => x.result_file_name)
                    ?.First()
                    ?.ToString()
                );
            }

            return NoContent();
        }
    }
}
