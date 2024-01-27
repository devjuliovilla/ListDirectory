namespace ListDirectory.Models
{
    public class FileViewModel
    {
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileType { get; set; }
        public double FileSizeMB => FileSizeBytes / (1024.0 * 1024.0);
        public DateTime CreationDate { get; set; }
    }
}
