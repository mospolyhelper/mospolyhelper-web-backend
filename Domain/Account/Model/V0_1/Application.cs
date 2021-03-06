﻿namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class Application
    {
        public Application(
            string registrationNumber, 
            string name, string dateTime, 
            string status, string department, 
            string note, 
            string info
            )
        {
            RegistrationNumber = registrationNumber;
            Name = name;
            DateTime = dateTime;
            Status = status;
            Department = department;
            Note = note;
            Info = info;
        }

        public string RegistrationNumber { get; }
        public string Name { get; }
        public string DateTime { get; }
        public string Status { get; }
        public string Department { get; }
        public string Note { get; }
        public string Info { get; }
    }
}
