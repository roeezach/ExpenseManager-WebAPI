using System.Threading.Tasks;
using ExpensesManager.Common.Messaging;
using ExpensesManager.ReaderService.Data;
using ExpensesManager.ReaderService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.ReaderService.Services
{
    public class ExpenseReadService
    {
        private readonly ReaderDbContext _db;
        private readonly IKafkaProducer _producer;

        public ExpenseReadService(ReaderDbContext db, IKafkaProducer producer)
        {
            _db = db;
            _producer = producer;
        }

        public async Task<UploadedFile> SaveUploadedFileAsync(UploadedFile file)
        {
            _db.UploadedFiles.Add(file);
            await _db.SaveChangesAsync();

            var ev = new
            {
                eventType = "ExpenseUploaded",
                payload = new
                {
                    uploadedFileId = file.Id,
                    fileName = file.FileName,
                    userId = file.UserID,
                    fileType = file.FileType,
                    linkedMonth = file.LinkedMonth,
                    linkedYear = file.LinkedYear,
                    uploadedAt = file.UploadDate
                }
            };

            await _producer.ProduceAsync("ExpenseUploaded", ev);
            return file;
        }
    }
}
