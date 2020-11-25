using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountPortfolio
    {
        public AccountPortfolio(
            int id,
            string name, 
            string group, 
            string direction, 
            string specialization, 
            string course, 
            string educationForm
            )
        {
            Id = id;
            Name = name;
            Group = group;
            Direction = direction;
            Specialization = specialization;
            Course = course;
            EducationForm = educationForm;
        }

        public int Id { get; }
        public string Name { get; }
        public string Group { get; }
        public string Direction { get; }
        public string Specialization { get; }
        public string Course { get; }
        public string EducationForm { get; }
    }
}
