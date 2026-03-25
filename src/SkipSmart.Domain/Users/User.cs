using System.Security.Cryptography.X509Certificates;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users.Events;

namespace SkipSmart.Domain.Users;

public class User : Entity {
     public FirstName FirstName { get; private set; }
     public LastName LastName { get; private set; }
     public Email Email { get; private set; }
     public Password Password { get; private set; }
     
     public int LanguageSubgroup { get; private set; }
     
     public int FacultySubgroup { get; private set; }
     
     public bool IsEmailVerified { get; private set; }
     public EmailVerificationCode? EmailVerificationCode { get; set; }
     public DateTime EmailVerificationSentAt { get; set; }
     
     public Guid GroupId { get; private set; }
     
     private User(Guid id, FirstName firstName, LastName lastName, Email email, int languageSubgroup, int facultySubgroup, bool isEmailVerified, Password password, Guid groupId)
         : base(id) 
     {
         FirstName = firstName;
         LastName = lastName;
         Email = email;
         LanguageSubgroup = languageSubgroup;
         FacultySubgroup = facultySubgroup;
         Password = password;
         GroupId = groupId;
         IsEmailVerified = isEmailVerified;
     }
     
     private User() {
     }
     
     public static User Create(Guid userId, FirstName firstName, LastName lastName, Email email, int languageSubgroup, int facultySubgroup, Password password, Guid groupId) {
         var user = new User(userId, firstName, lastName, email, languageSubgroup, facultySubgroup, false, password, groupId);
         
         user.RaiseDomainEvent(new UserCreatedDomainEvent(userId));
         
         return user;
     }
     
     public Result VerifyEmail() {
         IsEmailVerified = true;
         
         RaiseDomainEvent(new UserEmailVerifiedDomainEvent(Id));

         return Result.Success();
     }
     
     public void ChangeGroup(Guid newGroupId) {
         GroupId = newGroupId;
     }

     public void ChangeLanguageSubgroup(int newLanguageSubgroup) {
         LanguageSubgroup = newLanguageSubgroup;
     }
     
     public void ChangeFacultySubgroup(int newFacultySubgroup) {
         FacultySubgroup = newFacultySubgroup;
     }
     
     public void SetEmailVerificationCode(int code, DateTime sentAt) {
         EmailVerificationCode = new EmailVerificationCode(code);
         EmailVerificationSentAt = sentAt;
     }
}