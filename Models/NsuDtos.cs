﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaijaStartupWeb.Models
{
    public class NsuDtos
    {
        public class User : IdentityUser
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string Address { get; set; }
            public bool IsActive { get; set; }
            public string Role { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public bool IsDeleted { get; set; }
            public string DeletionUserId { get; set; }
            public DateTime DeletionTime { get; set; }
        }

        public class Package
        {
            public int Id { get; set; }
            public string PackageName { get; set; }
            public Decimal Price { get; set; }

            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class Incentives
        {
            public int Id { get; set; }
            public string IncentiveName { get; set; }
            public string Description { get; set; }

            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }
        public class Contact
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Message { get; set; }

            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class Company_Registration
        {
            [Key]
            public Guid Id { get; set; }
            public string CacRegistrationNumber { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string AlternateCompanyName { get; set; }

            public List<Payments> Payments { get; set; }
            public List<Comp_Incentives> Comp_Incentives { get; set; }
            public string AlternateCompanyType { get; set; }
            public string FinancialYearEnd { get; set; }

            public int PackageId { get; set; }
            public decimal PackagePrice { get; set; }
            [ForeignKey("User")]
            public string UserId { get; set; }
            public User User { get; set; }
            public string BusinessActivity { get; set; }
            public string ApprovalStatus { get; set; }
            public string SndBusinessActivity { get; set; }
            public bool IsAddressRegistered { get; set; }
            public bool LocalResidentDirector { get; set; }
            public decimal LocalResidentDirectorPrice { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Postcode { get; set; }
            public bool LocalDirector { get; set; }
            public string CompanyCapitalCurrency { get; set; }
            public string ShareHolderName { get; set; }
            public int NoOfSharesIssue { get; set; }
            public decimal TotalAmount { get; set; }
            public bool RegCompleted { get; set; }
            public bool IsCacAvailable { get; set; }
            public Decimal SharePrice { get; set; }
            public Decimal SharesAllocated { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
            public List<AddOnService> addOnServices { get; set; }
            public List<Company_Officers> company_Officers { get; set; }
        }

       
        public class AddOnService
        {
            [Key]
            public Guid Id { get; set; }
            public Guid Registration_Id { get; set; }
            public string ServiceName {get;set;}
            public decimal Price { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class Payments
        {
            [Key]
            public Guid Id { get; set; }
            public string ApiRequest { get; set; }
            public string ApiResponse { get; set; }
            public bool Status { get; set; }
            public string Message { get; set; }
            public string PaymentType { get; set; }
            public decimal Tax { get; set; }
            public decimal Discount { get; set; }
            public decimal Total { get; set; }
            public decimal Amount { get; set; }
            public Guid RegistrationId { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }
        public class Company_Officers
        {
            [Key]
            public Guid Id { get; set; }
            public Company_Registration Registration { get; set; }
            public string FullName { get; set; }
            public string Gender { get; set; }
            public string Designation { get; set; }
            public string Id_Type { get; set; }
            public string Id_Number { get; set; }
            public string Nationality { get; set; }
            public string Birth_Country { get; set; }
            public string Phone_No { get; set; }
            public string Dob { get; set; }
            public string Email { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string PostalCode { get; set; }
            public string MobileNo { get; set; }
            public byte[] Identification { get; set; }
            public byte[] Proficiency { get; set; }
            public byte[] CerficationOfBirth { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }
        public class Comp_Incentives
        {
            public int Id { get; set; }
            public int Incentive_Id { get; set; }
            public Company_Registration Registration { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class ChatHeader
        {
            public Guid Id { get; set; }
            public string UserId { get; set; }
            public Guid CompanyId { get; set; }
            public string PostIncooperationName {get;set;}
            public string Group { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool IsTicket { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }


        public class ChatThread
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public Guid ChatId { get; set; }
            public string Body { get; set; }
            public bool IsRead { get; set; }
            public byte[] document { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class Settings
        {
            public int Id { get; set; }
            public string code { get; set; }
            public string description { get; set; }
            public string field1 { get; set; }
            public string field2 { get; set; }
            public DateTime CreationTime { get; set; }
            public string CreatorUserId { get; set; }
            public DateTime ModificationTime { get; set; }
            public string ModificationUserId { get; set; }
            public DateTime DeletionTime { get; set; }
            public string DeletionUserId { get; set; }
            public bool IsDeleted { get; set; }
        }

    }
}
