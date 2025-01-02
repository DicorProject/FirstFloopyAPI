using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class Auth
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        [StringLength(400)]
        public string? Login { get; set; }

        [Column("password")]
        [StringLength(400)]
        public string? Password { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("usertype")]
        [StringLength(50)]
        public string? UserType { get; set; }

        [Column("no")]
        public int? No { get; set; }

        [Column("Loggedin")]
        [StringLength(5)]
        public string? LoggedIn { get; set; }

        [Column("MenuId")]
        [StringLength(5000)]
        public string? MenuId { get; set; }

        [Column("BranchId")]
        [StringLength(200)]
        public string? BranchId { get; set; }

        [Column("Utype")]
        [StringLength(50)]
        public string? Utype { get; set; }

        [Column("branches")]
        [StringLength(200)]
        public string? Branches { get; set; }

        [Column("AccountCode")]
        public int? AccountCode { get; set; }

        [Column("pwdDate")]
        public DateTime? PwdDate { get; set; }

        [Column("AuthPwd")]
        [StringLength(50)]
        public string? AuthPwd { get; set; }

        [Column("SchoolId")]
        public int? SchoolId { get; set; }

        [Column("compid")]
        public int? Compid { get; set; }

        [Column("UserIds")]
        [StringLength(100)]
        public string? UserIds { get; set; }

        [Column("DateTimeTempNo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DateTimeTempNo { get; set; }

        [Column("DateTimeTempDate")]
        public DateTime? DateTimeTempDate { get; set; }

        [Column("EMailId")]
        [StringLength(500)]
        public string? EmailId { get; set; }

        [Column("regid")]
        public int? RegId { get; set; }

        [Column("Godown")]
        public string? Godown { get; set; }

        [Column("GodownId")]
        public string? GodownId { get; set; }

        [Column("deviceid")]
        public string? DeviceId { get; set; }

        [Column("oldmobileno")]
        [StringLength(200)]
        public string? OldMobileNo { get; set; }

        [Column("CompanyId")]
        [StringLength(200)]
        public string? CompanyId { get; set; }

        [Column("Companies")]
        [StringLength(500)]
        public string? Companies { get; set; }

        [Column("ProfileID")]
        public int? ProfileID { get; set; }

        [Column("mobileverifystatus")]
        public int? MobileVerifyStatus { get; set; }

        [Column("profilestatus")]
        public int? ProfileStatus { get; set; }

        [Column("loginstatus")]
        public int? LoginStatus { get; set; }

        [Column("forgototp")]
        [StringLength(200)]
        public string? ForgotOtp { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [Column("appbranchid")]
        [StringLength(200)]
        public string? AppBranchId { get; set; }

        [Column("loginby")]
        [StringLength(100)]
        public string? LoginBy { get; set; }

        [Column("registrationid")]
        public int? RegistrationId { get; set; }

        [Column("userid")]
        public int? UserId { get; set; }

        [Column("mobileno")]
        [StringLength(200)]
        public string? MobileNo { get; set; }

        [Column("Image")]
        [StringLength(int.MaxValue)]
        public string? Image { get; set; }

        [Column("Address")]
        [StringLength(255)]
        public string? Address { get; set; }

        [Column("Locality")]
        [StringLength(255)]
        public string? Locality { get; set; }

        [Column("City")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("State")]
        [StringLength(100)]
        public string? State { get; set; }

        [Column("Pincode")]
        [StringLength(20)]
        public string? Pincode { get; set; }

        [Column("OtpExpireTime")]
        public DateTime? OtpExpireTime { get; set; }    

	}


}
