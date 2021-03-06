﻿namespace WebStore.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }
       
        public int Age { get; set; }

        public int Salary { get; set; }

        public string Phone { get; set; }

        public override string ToString() => $"{LastName} {FirstName} {Patronymic} {Age} лет {Salary} {Phone}";
    }
}
