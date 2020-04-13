using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.CloudResourceManager.v1;
using Google.Apis.Auth.OAuth2.Flows;
using static Google.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow;

namespace Ubigrade.Library.Google
{
    public class GetGoogleData
    {
        private static string classroom_apikey = "AIzaSyBq6xSct8OhL2-5zUWMH73wRIN6iFGOg2k";

        public async static Task<List<Course>> GetAllStudentActiveCoursesByAccesstoken(UserCredential credential)
        {

            var service = new ClassroomService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });
            CoursesResource.ListRequest request = service.Courses.List();
            request.CourseStates = CoursesResource.ListRequest.CourseStatesEnum.ACTIVE;
            List<Course> courses = new List<Course>();
            ListCoursesResponse response = await request.ExecuteAsync();
            courses.AddRange(response.Courses);
            return courses;
        }
        public async static Task<List<Course>> GetAllStudentActiveCourses(UserCredential credential)
        {
            var service = new ClassroomService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,

                ApplicationName = "UbiGrade"
            });

            CoursesResource.ListRequest request = service.Courses.List();
            // Restricts returned courses to those in one of the specified states 
            // The default value is ACTIVE, ARCHIVED, PROVISIONED, DECLINED.
            request.CourseStates = CoursesResource.ListRequest.CourseStatesEnum.ACTIVE;
            // Restricts returned courses to those having a teacher with the specified identifier. The identifier can be one of the following:
            // - the numeric identifier for the user
            // - the email address of the user
            // - the string literal "me", indicating the requesting user
            //request.TeacherId = "me";

            // List courses.
            List<Course> courses = new List<Course>();
            ListCoursesResponse response = await request.ExecuteAsync();
            courses.AddRange(response.Courses);
            return courses;
        }

        public async static Task<List<Student>> GetAllStudentsOfCourse(UserCredential credential, string courseid)
        {
            var service = new ClassroomService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });

            CoursesResource.StudentsResource.ListRequest request = service.Courses.Students.List(courseid);
            // List courses.
            List<Student> students = new List<Student>();
            var response = await request.ExecuteAsync();
            students.AddRange(response.Students);
            return students;
        }

        public async static Task<Student> GetStudentDataByUserId(UserCredential credential, string courseid, string userid)
        {
            var service = new ClassroomService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });

            CoursesResource.StudentsResource.GetRequest request = service.Courses.Students.Get(courseid, userid);
            // List courses.
            Student student = new Student();
            var response = await request.ExecuteAsync();
            student = response;
            return student;
        }
        public async static Task<UserProfile> GetClassroomUserProfile(UserCredential credential)
        {
            var service = new ClassroomService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });
            UserProfilesResource.GetRequest request = service.UserProfiles.Get(credential.UserId);
            var result = await request.ExecuteAsync();

            return result;
        }
        public async static Task<User> GetOrgUnits(UserCredential credential, string userid)
        {
            var service = new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });
            UsersResource.GetRequest request = service.Users.Get(userid);
            // List courses. "admin#directory#user"
            request.ViewType = UsersResource.GetRequest.ViewTypeEnum.DomainPublic;
            var x = await request.ExecuteAsync();
            return x;
        }
        public async static Task<Users> GetOrgUnits2(UserCredential credential, string userid)
        {
            var service = new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });
            UsersResource.ListRequest request = service.Users.List();
            request.Customer = "my_customer";
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users.
            IList<User> users = request.Execute().UsersValue;
            var x = await request.ExecuteAsync();
            return x;
        }
        public async static Task<User> GetOrgUnits3(UserCredential credential, string userid)
        {
            //directory api key AIzaSyBq6xSct8OhL2-5zUWMH73wRIN6iFGOg2k
            //"106276604230863721006" amel
            var service = new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApiKey = "AIzaSyBq6xSct8OhL2-5zUWMH73wRIN6iFGOg2k",
                ApplicationName = "UbiGrade"
            });
            UsersResource.GetRequest request = service.Users.Get("Selimovic487a@htlwienwest.at");
            request.ViewType = UsersResource.GetRequest.ViewTypeEnum.DomainPublic;
            //UsersResource.GetRequest request = service.Users.Get("1056718226129");

            // htl orgunit id = 1056718226129

            var x = await request.ExecuteAsync();
            return x;
        }
        public async static Task<Person> GetPeopleInfo(UserCredential credential, string userid)
        {
            //directory api key AIzaSyBq6xSct8OhL2-5zUWMH73wRIN6iFGOg2k
            //people und cloud resource manager api key AIzaSyBtsaZHK0PswRKIcErSX7eH7ojT_rHeKpE
            var service = new PeopleService(new BaseClientService.Initializer
            {
                //HttpClientInitializer = credential,
                ApiKey = "AIzaSyBtsaZHK0PswRKIcErSX7eH7ojT_rHeKpE",
                ApplicationName = "UbiGrade"
            });

            PeopleResource.GetRequest request = service.People.Get("people/me");
            request.BearerToken = credential.Token.AccessToken;
            request.RequestMaskIncludeField = "people.names";

            // htl orgunit id = 1056718226129

            var x = await request.ExecuteAsync();
            return x;
        }
    }
}
