﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountInfo
    {
        public AccountInfo(
            string name, 
            string status, 
            string sex, 
            string birthDate, 
            string studentCode, 
            string faculty, 
            string course, 
            string group, 
            string direction, 
            string specialization, 
            string educationPeriod, 
            string educationForm, 
            string financingType, 
            string educationLevel, 
            string admissionYear, 
            IList<string> orders
            )
        {
            Name = name;
            Status = status;
            Sex = sex;
            BirthDate = birthDate;
            StudentCode = studentCode;
            Faculty = faculty;
            Course = course;
            Group = group;
            Direction = direction;
            Specialization = specialization;
            EducationPeriod = educationPeriod;
            EducationForm = educationForm;
            FinancingType = financingType;
            EducationLevel = educationLevel;
            AdmissionYear = admissionYear;
            Orders = orders;
        }

        public string Name { get; }
        public string Status { get; }
        public string Sex { get; }
        public string BirthDate { get; }
        public string StudentCode { get; }
        public string Faculty { get; }
        public string Course { get; }
        public string Group { get; }
        public string Direction { get; }
        public string Specialization { get; }
        public string EducationPeriod { get; }
        public string EducationForm { get; }
        public string FinancingType { get; }
        public string EducationLevel { get; }
        public string AdmissionYear { get; }
        public IList<string> Orders { get; }
    }
}
