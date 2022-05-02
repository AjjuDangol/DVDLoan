namespace Coursework.Models.DTO
{
    public class DVDCopyDTO
    {
        public string DvdTitle { get; set; }

        public string FullName { get; set; }

        public DateTime DateOut { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime DateReturned { get; set; }
    }
}

