using Pdf2Epub.API.Models;

namespace Pdf2Epub.API.Repositories
{
    public class ConvertTaskRepository : BaseRepository<ConvertTaskModel>
    {
        public async Task<Guid> NewTask()
        {
            return (
                await Add(
                    new ConvertTaskModel() {
                        id = Guid.NewGuid(),
                        start_time = DateTime.Now,
                        status = ConvertStatus.UPLOADING
                    }
                )
            ).id;
        }

        public async Task<bool> UpdateFileName(Guid id, string file_name)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    file_name = file_name,
                }
            );
        }

        public async Task<bool> UpdateTaskState(Guid id, ConvertStatus status)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    status = status
                }
            );
        }

        public async Task<string?> GetTaskFilename(Guid id)
        {
            return (await Query(x => x.id == id)).Select(x => x.file_name).First();
        }

        public async Task<ConvertStatus?> GetTaskStatus(Guid id)
        {
            return (await Query(x => x.id == id)).Select(x => x.status).First();
        }

        public async Task<bool> UpdateWorkerId(Guid id, Guid worker_id)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    worker_id = worker_id
                }
            );
        }

        public async Task<bool> UpdateVertical(Guid id, bool is_vertical)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    is_vertical = is_vertical
                }
            );
        }

        public async Task<bool> UpdateEndtime(Guid id)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    end_time = DateTime.Now
                }
            );
        }

        public async Task<bool> UpdateResultFileName(Guid id, string result_file_name)
        {
            return await Update(
                new ConvertTaskModel() {
                    id = id,
                    result_file_name = result_file_name
                }
            );
        }
    }
}
