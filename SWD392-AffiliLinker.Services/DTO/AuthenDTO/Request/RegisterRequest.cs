using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request
{
	public class RegisterRequest
	{
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^0([0-9]{9})$", ErrorMessage = "Invalid Phone Number! Must be 10 digits and start with 0")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format!")]
        public DateTime DOB { get; set; }
    }


    public class AdvertiserRegisterRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^0([0-9]{9})$", ErrorMessage = "Invalid Phone Number! Must be 10 digits and start with 0")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format!")]
        public DateTime DOB { get; set; }
        public virtual AdvertiserRequest AdvertiserRequest { get; set; }
    }

    public class AdvertiserRequest
    {
        [Required(ErrorMessage = "Company Name is required")]
        public string CampanyName { get; set; }

        [Required(ErrorMessage = "Company Address is required")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Invalid Website URL format")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Since date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format!")]
        public DateTime Since { get; set; }

        [Required(ErrorMessage = "Business License is required")]
        public string BussinessLicense { get; set; }
    }

    public class PublisherRegisterRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^0([0-9]{9})$", ErrorMessage = "Invalid Phone Number! Must be 10 digits and start with 0")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format!")]
        public DateTime DOB { get; set; }
        public virtual PublisherRequest PublisherRequest { get; set; }
    }

    public class PublisherRequest
    {

        [Required(ErrorMessage = "Tax Code is required")]
        public string TaxCode { get; set; }

    }
}
