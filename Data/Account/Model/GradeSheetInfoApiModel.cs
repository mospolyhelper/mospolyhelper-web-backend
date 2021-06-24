namespace Mospolyhelper.Data.Account.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Domain.Account.Model.V0_1;

    public class GradeSheetInfoApiModel
    {

        [JsonPropertyName("num")]
        public string Num { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; }

        [JsonPropertyName("doc_type")]
        public string DocType { get; set; }

        [JsonPropertyName("stat_type")]
        public string StatType { get; set; }

        [JsonPropertyName("exam_type")]
        public string ExamType { get; set; }

        [JsonPropertyName("chair")]
        public string Chair { get; set; }

        [JsonPropertyName("faculty")]
        public string Faculty { get; set; }

        [JsonPropertyName("exam_date")]
        public string ExamDate { get; set; }

        [JsonPropertyName("exam_time")]
        public string ExamTime { get; set; }

        [JsonPropertyName("recieved_date")]
        public string RecievedDate { get; set; }

        [JsonPropertyName("unblock_date")]
        public string UnblockDate { get; set; }

        [JsonPropertyName("close_date")]
        public string CloseDate { get; set; }

        [JsonPropertyName("return_date")]
        public string ReturnDate { get; set; }

        [JsonPropertyName("group_type")]
        public string GroupType { get; set; }

        [JsonPropertyName("posted")]
        public string Posted { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("course")]
        public string Course { get; set; }

        [JsonPropertyName("semestr")]
        public string Semestr { get; set; }

        [JsonPropertyName("grp")]
        public string Grp { get; set; }

        [JsonPropertyName("disc_name")]
        public string DiscName { get; set; }

        [JsonPropertyName("disc_guid")]
        public string DiscGuid { get; set; }

        [JsonPropertyName("edu_form")]
        public string EduForm { get; set; }

        [JsonPropertyName("specnapr")]
        public string Specnapr { get; set; }

        [JsonPropertyName("specnapr_code")]
        public string SpecnaprCode { get; set; }

        [JsonPropertyName("profile")]
        public string Profile { get; set; }

        [JsonPropertyName("pps")]
        public IDictionary<string, PpsApiModel> Pps { get; set; }

        [JsonPropertyName("students")]
        public IDictionary<string, StudentsApiModel> Students { get; set; }

        [JsonPropertyName("num_marks")]
        public string NumMarks { get; set; }

        [JsonPropertyName("exam_date_user_fio")]
        public string ExamDateUserFio { get; set; }

        [JsonPropertyName("exam_date_user_id")]
        public string ExamDateUserId { get; set; }

        [JsonPropertyName("exam_date_user_guid")]
        public string ExamDateUserGuid { get; set; }

        [JsonPropertyName("exam_date_modified")]
        public string ExamDateModified { get; set; }

        [JsonPropertyName("deleted")]
        public string Deleted { get; set; }

        [JsonPropertyName("can_edit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("can_sign")]
        public bool CanSign { get; set; }

        [JsonPropertyName("fixed")]
        public int Fixed { get; set; }

        [JsonPropertyName("mark_field")]
        public string MarkField { get; set; }

        [JsonPropertyName("webex_link")]
        public string WebexLink { get; set; }

        [JsonPropertyName("webex_login")]
        public string WebexLogin { get; set; }

        [JsonPropertyName("webex_password")]
        public string WebexPassword { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("modified")]
        public string Modified { get; set; }
    }

    public class PpsApiModel
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("fio")]
        public string Fio { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("is_head")]
        public int IsHead { get; set; }

        [JsonPropertyName("signed")]
        public int Signed { get; set; }
    }

    public class StudentsApiModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("uid")]
        public object Uid { get; set; }

        [JsonPropertyName("fio")]
        public string Fio { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; }

        [JsonPropertyName("mark")]
        public string Mark { get; set; }

        [JsonPropertyName("mark_guid")]
        public string MarkGuid { get; set; }

        [JsonPropertyName("can_change_mark")]
        public bool CanChangeMark { get; set; }

        [JsonPropertyName("blocked")]
        public int Blocked { get; set; }

        [JsonPropertyName("pps_fio")]
        public string PpsFio { get; set; }

        [JsonPropertyName("pps_id")]
        public string PpsId { get; set; }

        [JsonPropertyName("pps_guid")]
        public string PpsGuid { get; set; }

        [JsonPropertyName("mfield")]
        public string Mfield { get; set; }

        [JsonPropertyName("ticketnum")]
        public string Ticketnum { get; set; }

        [JsonPropertyName("casenum")]
        public string Casenum { get; set; }
    }

    static class GradeSheetInfoApiExt
    {
        public static GradeSheetInfo ToModel(this GradeSheetInfoApiModel apiModel)
        {
            return new GradeSheetInfo(
                id: apiModel.Num,
                guid: apiModel.Guid,
                documentType: apiModel.DocType,
                examType: apiModel.ExamType,
                department: apiModel.Chair,
                school: apiModel.Faculty,
                examDate: apiModel.ExamDate,
                examTime: apiModel.ExamTime,
                closeDate: apiModel.CloseDate,
                year: apiModel.Year,
                course: apiModel.Course,
                semester: apiModel.Semestr,
                group: apiModel.Grp,
                disciplineName: apiModel.DiscName,
                educationForm: apiModel.EduForm,
                direction: apiModel.Specnapr,
                directionCode: apiModel.SpecnaprCode,
                specialization: apiModel.Profile,
                teachers: apiModel.Pps.Select(it => it.Value.ToModel()).ToList(),
                students: apiModel.Students.Select(it => it.Value.ToModel()).ToList(),
                @fixed: apiModel.Fixed != 0,
                modifiedDate: apiModel.Modified
            );
        }

        public static GradeSheetTeacher ToModel(this PpsApiModel apiModel)
        {
            return new GradeSheetTeacher(
                uid: apiModel.Uid,
                name: apiModel.Fio,
                signed: apiModel.Signed != 0
            );
        }

        public static GradeSheetStudent ToModel(this StudentsApiModel apiModel)
        {
            return new GradeSheetStudent(
                name: apiModel.Fio,
                mark: apiModel.Mark,
                ticket: apiModel.Ticketnum,
                recordBook: apiModel.Casenum,
                blocked: apiModel.Blocked != 0
            );
        }
    }
}
