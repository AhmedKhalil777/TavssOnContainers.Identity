using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.V1.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api/";

        public const string Version = "v1";

        public const string Base = Root + Version + "/";
        public static class User
        {
            public const string RegisterDoctor = Base + "RegisterDoctor";
            public const string RegisterStudent = Base + "RegisterStudent";
            public const string RegisterTA = Base + "RegisterTA";
            public const string RegisterAdmin = Base + "RegisterAdmin";


            public const string DoctorLogin = Base + "doctorLogin";
            public const string AdminLogin = Base + "AdminLogin";
            public const string StudentLogin = Base + "StudentLogin";
            public const string TALogin = Base + "taLogin";
            public const string DeveloperLogin = Base + "developerLogin";

         

        }

        public static class ProfileRoutes
        {
            public const string GetStudents = Base + "GetStudents";
            public const string ChangePassword = Base + "ChangePassword/{Id}";
            public const string UpdateImage = Base + "UpdateImage/{Id}";
            public const string UpdateUser = Base + "UpdateUser";
            public const string DeleteUser = Base + "DeleteUser/{Id}";
            public const string GetStudent = Base + "GetStudent/{Id}";
            public const string GetDoctors = Base + "GetDoctors";
            public const string GetDoctor = Base + "GetDoctor/{Id}";
            public const string GetTAs = Base + "GetTAs";
            public const string GetTA = Base + "GetTA/{Id}";
            public const string GetUsers = Base + "GetUsers";
            public const string GetUser = Base + "GetUser/{Id}";




        }

    }
}
