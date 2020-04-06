using Google.Apis.Classroom.v1.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubigrade.Application.Models.Course
{
    public class CourseViewModel
    {
        //
        // Summary:
        //     The email address of a Google group containing all teachers of the course. This
        //     group does not accept email and can only be used for permissions. Read-only.
        [JsonProperty("teacherGroupEmail")]
        public virtual string TeacherGroupEmail { get; set; }
        //
        // Summary:
        //     Information about a Drive Folder that is shared with all teachers of the course.
        //     This field will only be set for teachers of the course and domain administrators.
        //     Read-only.
        [JsonProperty("teacherFolder")]
        public virtual DriveFolder TeacherFolder { get; set; }
        //
        // Summary:
        //     Section of the course. For example, "Period 2". If set, this field must be a
        //     valid UTF-8 string and no longer than 2800 characters.
        [JsonProperty("section")]
        public virtual string Section { get; set; }
        //
        // Summary:
        //     Optional room location. For example, "301". If set, this field must be a valid
        //     UTF-8 string and no longer than 650 characters.
        [JsonProperty("room")]
        public virtual string Room { get; set; }
        //
        // Summary:
        //     The identifier of the owner of a course. When specified as a parameter of a create
        //     course request, this field is required. The identifier can be one of the following:
        //     * the numeric identifier for the user * the email address of the user * the string
        //     literal `"me"`, indicating the requesting user This must be set in a create request.
        //     Admins can also specify this field in a patch course request to transfer ownership.
        //     In other contexts, it is read-only.
        [JsonProperty("ownerId")]
        public virtual string OwnerId { get; set; }
        //
        // Summary:
        //     Name of the course. For example, "10th Grade Biology". The name is required.
        //     It must be between 1 and 750 characters and a valid UTF-8 string.
        [JsonProperty("name")]
        public virtual string Name { get; set; }
        //
        // Summary:
        //     Identifier for this course assigned by Classroom. When creating a course, you
        //     may optionally set this identifier to an alias string in the request to create
        //     a corresponding alias. The `id` is still assigned by Classroom and cannot be
        //     updated after the course is created. Specifying this field in a course update
        //     mask results in an error.
        [JsonProperty("id")]
        public virtual string Id { get; set; }
        //
        // Summary:
        //     Enrollment code to use when joining this course. Specifying this field in a course
        //     update mask results in an error. Read-only.
        [JsonProperty("enrollmentCode")]
        public virtual string EnrollmentCode { get; set; }
        //
        // Summary:
        //     Optional heading for the description. For example, "Welcome to 10th Grade Biology."
        //     If set, this field must be a valid UTF-8 string and no longer than 3600 characters.
        [JsonProperty("descriptionHeading")]
        public virtual string DescriptionHeading { get; set; }
        //
        // Summary:
        //     Optional description. For example, "We'll be learning about the structure of
        //     living creatures from a combination of textbooks, guest lectures, and lab work.
        //     Expect to be excited!" If set, this field must be a valid UTF-8 string and no
        //     longer than 30,000 characters.
        [JsonProperty("description")]
        public virtual string Description { get; set; }
        //
        // Summary:
        //     Creation time of the course. Specifying this field in a course update mask results
        //     in an error. Read-only.
        [JsonProperty("creationTime")]
        public virtual object CreationTime { get; set; }
        //
        // Summary:
        //     State of the course. If unspecified, the default state is `PROVISIONED`.
        [JsonProperty("courseState")]
        public virtual string CourseState { get; set; }
        //
        // Summary:
        //     Sets of materials that appear on the "about" page of this course. Read-only.
        [JsonProperty("courseMaterialSets")]
        public virtual IList<CourseMaterialSet> CourseMaterialSets { get; set; }
        //
        // Summary:
        //     The email address of a Google group containing all members of the course. This
        //     group does not accept email and can only be used for permissions. Read-only.
        [JsonProperty("courseGroupEmail")]
        public virtual string CourseGroupEmail { get; set; }
        //
        // Summary:
        //     The Calendar ID for a calendar that all course members can see, to which Classroom
        //     adds events for course work and announcements in the course. Read-only.
        [JsonProperty("calendarId")]
        public virtual string CalendarId { get; set; }
        //
        // Summary:
        //     Absolute link to this course in the Classroom web UI. Read-only.
        [JsonProperty("alternateLink")]
        public virtual string AlternateLink { get; set; }
        //
        // Summary:
        //     Time of the most recent update to this course. Specifying this field in a course
        //     update mask results in an error. Read-only.
        [JsonProperty("updateTime")]
        public virtual object UpdateTime { get; set; }
        //
        // Summary:
        //     The ETag of the item.
        public virtual string ETag { get; set; }
    }
}
