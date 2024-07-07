using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class Enrollment : BaseEntity
    {
        public Guid? CourseId { get; set; }
        public Course? Course { get; set; }
        public string CourseDifficulty { get; set; }

    }
}
